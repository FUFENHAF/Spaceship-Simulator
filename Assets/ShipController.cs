using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    [Header("BasicElements")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float cargo = 0f;
    [SerializeField] private float cargoCapacity = 1_000f;

    [Header("Thrust")]
    [SerializeField] private float thrustForce = 50f;

    [Header("Rotation")]
    [SerializeField] private float pitchTorque = 20f;
    [SerializeField] private float yawTorque = 20f;

    [Header("Modules")]
    [SerializeField] private AttachPoint[] attachPoints;
    
    [Header("User Interface")]
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private GameObject gameManager;

    [Header("VisualEffect")]
    [SerializeField] private GameObject shipDestructionEffectPrefab;

    private ShipControls _controls;
    private Rigidbody _rb;
    private HudController _hudController;
    private GameManager _gameManager;

    // Flight assist slows the ship down when necessary - only for spinning!
    private bool _assistEnabled = true;
    private float _angularDragStrong = 1f;
    private float _angularDragWeak = 0.1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _controls = new ShipControls();
        _controls.Ship.ToggleFlightAssist.performed += OnToggleFlightAssist;
        _controls.Ship.ToggleLeftEngine.performed += OnToggleLeftEngine;
        _controls.Ship.ToggleRightEngine.performed += OnToggleRightEngine;

        // Loading UI settings
        _hudController = uiCanvas.GetComponent<HudController>();
        _gameManager = gameManager.GetComponent<GameManager>();
    }

    private void OnDestroy()
    {
        _controls.Ship.ToggleFlightAssist.performed -= OnToggleFlightAssist;
        _controls.Ship.ToggleLeftEngine.performed -= OnToggleLeftEngine;
        _controls.Ship.ToggleRightEngine.performed -= OnToggleRightEngine;
    }

    private void OnEnable()
    {
        _controls.Ship.Enable();
    }

    private void OnDisable()
    {
        _controls.Ship.Disable();
    }

    public void OnToggleFlightAssist(InputAction.CallbackContext ctx) => _assistEnabled = !_assistEnabled;
    private void OnToggleLeftEngine(InputAction.CallbackContext ctx)  => ToggleEngine(0);
    private void OnToggleRightEngine(InputAction.CallbackContext ctx) => ToggleEngine(1);

    private void ToggleEngine(int index)
    {
        if (attachPoints.Length <= index) return;
        if (attachPoints[index].Module is Engine engine)
            engine.Toggle();
    }

    private void FixedUpdate()
    {
        float throttle = _controls.Ship.Throttle.ReadValue<float>();
        Vector2 rotation = _controls.Ship.Rotation.ReadValue<Vector2>();
        float miningInput = _controls.Ship.Mine.IsPressed() ? 1f : 0f;

        _rb.AddRelativeTorque(new Vector3(
            rotation.y * pitchTorque,
            rotation.x * yawTorque,
            0f));

        _rb.angularDamping = _assistEnabled ? _angularDragStrong : _angularDragWeak;

        foreach (AttachPoint point in attachPoints)
            switch (point.Module)
            {
                case Engine engine:
                    engine.SetThrottle(throttle);
                    break;
                case MiningLaser laser:
                    laser.SetThrottle(miningInput);
                    break;
            }

        if (_controls.Ship.MainMenu.WasPressedThisFrame() || _controls.UI.Resume.WasPressedThisFrame())
            _gameManager.TogglePause();
    }

    public void UpdateHealth(float healthDifference)
    {
        health += healthDifference;
        if (health <= 0)
        {
            DestroyShip();
        }
        _hudController.SetHullIntegrity(health, maxHealth);
    }

    public void UpdateCargo(float amount)
    {
        cargo += amount;
        _hudController.SetCargo(cargo, cargoCapacity);
        if (cargo >= cargoCapacity)
            _gameManager.WinGame();
    }

    private void DestroyShip()
    {
        Instantiate(shipDestructionEffectPrefab, transform.position, Quaternion.identity);
        _gameManager.LoseGame();
        Destroy(gameObject);
    }
}