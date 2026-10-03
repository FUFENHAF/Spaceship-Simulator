using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    [Header("Thrust")]
    [SerializeField] private float thrustForce = 50f;

    [Header("Rotation")]
    [SerializeField] private float pitchTorque = 20f;
    [SerializeField] private float yawTorque = 20f;

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
    }

    private void OnDestroy()
    {
        _controls.Ship.ToggleFlightAssist.performed -= OnToggleFlightAssist;
    }

    private void OnEnable()
    {
        _controls.Ship.Enable();
    }

    private void OnDisable()
    {
        _controls.Ship.Disable();
    }

    public void OnToggleFlightAssist(InputAction.CallbackContext ctx)
    {
        _assistEnabled = !_assistEnabled;
    }

    private void FixedUpdate()
    {
        float throttle = _controls.Ship.Throttle.ReadValue<float>();
        Vector2 rotation = _controls.Ship.Rotation.ReadValue<Vector2>();

        _rb.AddRelativeForce(Vector3.forward * (throttle * thrustForce));
        _rb.AddRelativeTorque(new Vector3(
            rotation.y * pitchTorque,
            rotation.x * yawTorque,
            0f));

        _rb.angularDamping = _assistEnabled ? _angularDragStrong : _angularDragWeak;
    }
}