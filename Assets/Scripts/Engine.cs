using UnityEngine;

public class Engine : MonoBehaviour, IShipModule
{
    [Header("Thrust")]
    [SerializeField] private float thrustForce = 25f;
    
    [Header("Visual")]
    [SerializeField] private ParticleSystem thrusterParticles;

    [Header("UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private int engineIndex;

    private Rigidbody _shipRb;
    private float _throttle;
    private bool _running = true;
    private HudController _hudController;

    private void Awake()
    {
        _shipRb = GetComponentInParent<Rigidbody>();
        GetComponentInParent<AttachPoint>()?.Attach(this);
        _hudController = canvas.GetComponent<HudController>();
    }

    public void SetThrottle(float throttle)
    {
        _throttle = throttle;
    }

    public void Toggle() {
         _running = !_running;
        _hudController.SetMotorState(engineIndex, _running);
    }

    private void FixedUpdate()
    {
        if (!_running) return;

        Vector3 force = transform.forward * (_throttle * thrustForce);
        _shipRb.AddForceAtPosition(force, transform.position, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        bool emitting = _running && _throttle > 0.01f;

        if (emitting && !thrusterParticles.isEmitting)
            thrusterParticles.Play();
        else if (!emitting && thrusterParticles.isEmitting)
            thrusterParticles.Stop();
    }
}
