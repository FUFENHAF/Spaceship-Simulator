using UnityEngine;

public class Engine : MonoBehaviour, IShipModule
{
    [Header("Thrust")]
    [SerializeField] private float thrustForce = 2.5f;
    
    [Header("Visual")]
    [SerializeField] private GameObject thrusterEffectForward;
    [SerializeField] private GameObject thrusterEffectBackward;

    [Header("Audio")]
    [SerializeField] private GameObject audioManagerObject;

    [Header("UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private int engineIndex;

    private Rigidbody _shipRb;
    private float _throttle;
    private bool _running = true;
    private HudController _hudController;
    private AudioManager _audioManager;

    private void Awake()
    {
        _shipRb = GetComponentInParent<Rigidbody>();
        GetComponentInParent<AttachPoint>()?.Attach(this);
        _hudController = canvas.GetComponent<HudController>();
        
        _audioManager = audioManagerObject.GetComponent<AudioManager>();
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
        bool emittingForward = _running && _throttle > 0.01f;
        bool emittingBackward = _running && _throttle < - 0.01f;

        if (thrusterEffectForward.activeSelf != emittingForward)
            thrusterEffectForward.SetActive(emittingForward);

        if (thrusterEffectBackward.activeSelf != emittingBackward)
            thrusterEffectBackward.SetActive(emittingBackward);

        // Scaling the throttle for greater visual cue
        float scaleForward = Mathf.Lerp(0.4f, 1f, _throttle);
        thrusterEffectForward.transform.localScale = Vector3.one * scaleForward;

        float scaleBackward = Mathf.Lerp(0.4f, 1f, - _throttle);
        thrusterEffectBackward.transform.localScale = Vector3.one * scaleBackward;

        _audioManager.SetThrust(Mathf.Abs(_throttle));
    }
}
