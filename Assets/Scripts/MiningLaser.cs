using UnityEngine;

public class MiningLaser : MonoBehaviour, IShipModule
{
    [Header("Mining")]
    [SerializeField] private float range = 60f;
    [SerializeField] private float mineRate = 5f;
    [SerializeField] private GameObject cargoHolderObject;
    
    [Header("Visual")]
    [SerializeField] private LineRenderer beam;
    [SerializeField] private ParticleSystem hitSparks;
    [SerializeField] private Transform beamOrigin;

    [Header("Audio")]
    [SerializeField] private GameObject audioManagerObject;

    [Header("Aiming")]
    [SerializeField] private float maxAimAngle = 30f;

    private Rigidbody _shipRb;
    private float _throttle;
    private ShipController _cargoHolder;
    private AudioManager _audioManager;

    private void Awake()
    {
        GetComponentInParent<AttachPoint>()?.Attach(this);
        if (beamOrigin == null) beamOrigin = transform;

        _cargoHolder = cargoHolderObject.GetComponent<ShipController>();

        _audioManager = audioManagerObject.GetComponent<AudioManager>();
    }

    public void SetThrottle(float throttle) => _throttle = throttle;
    public void Toggle() { }

    private void Update()
    {
        Mine();
    }

    private void Mine()
    {
        bool firing = _throttle > 0.5f;

        _audioManager.SetLaserActive(firing);

        if (!firing) { beam.enabled = false; return; }

        Ray screenRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(screenRay, out RaycastHit hit, range))
        {
            // The following code was generated with Vibe Code (by Mistral AI) as an overhaul of my initial system
            Vector3 laserDirection = beamOrigin.forward;
            Vector3 toTarget = (hit.point - beamOrigin.position).normalized;
            float angle = Vector3.Angle(laserDirection, toTarget);

            if (angle <= maxAimAngle) // The laser can reach the crosshair
            {
                beam.enabled = true;
                beam.SetPosition(0, beamOrigin.position);
                beam.SetPosition(1, hit.point);

                if (hit.collider.GetComponentInParent<Asteroid>() is Asteroid asteroid)
                    asteroid.Mine(mineRate * Time.deltaTime);
            }
            else
            {
                beam.enabled = false;
            }
        }
        else
        {
            beam.enabled = false;
        }
    }
}
