using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    [Header("BasicElements")]
    [SerializeField] private float health = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float cargo = 0;
    [SerializeField] private float cargoCapacity = 1_000f;

    [Header("Thrust")]
    [SerializeField] private float thrustForce = 50f;

    [Header("Rotation")]
    [SerializeField] private float pitchTorque = 20f;
    [SerializeField] private float yawTorque = 20f;

    [Header("Modules")]
    [SerializeField] private AttachPoint[] attachPoints;

    private ShipControls _controls;
    private Rigidbody _rb;
    
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
    }
}