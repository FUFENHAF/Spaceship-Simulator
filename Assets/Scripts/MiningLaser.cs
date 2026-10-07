using UnityEngine;

public class MiningLaser : MonoBehaviour, IShipModule
{
    [Header("Mining")]
    [SerializeField] private float range = 60f;
    [SerializeField] private float mineRate = 5f;
    
    [Header("Visual")]
    [SerializeField] private LineRenderer beam;
    [SerializeField] private ParticleSystem hitSparks;
    [SerializeField] private Transform beamOrigin;

    private Rigidbody _shipRb;
    private float _throttle;

    private void Awake()
    {
        GetComponentInParent<AttachPoint>()?.Attach(this);
        if (beamOrigin == null) beamOrigin = transform;
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
        if (!firing) { beam.enabled = false; return; }

        Vector3 origin = beamOrigin.position;
        Debug.DrawRay(origin, beamOrigin.forward * range, Color.red);   // A: vidíš čáru ve Scene

        if (Physics.Raycast(origin, beamOrigin.forward, out RaycastHit hit, range))
        {
            Debug.Log($"LASER HIT: {hit.collider.name} at {hit.distance}");   // B
            beam.enabled = true;
            beam.SetPosition(0, origin);
            beam.SetPosition(1, hit.point);

            if (hit.collider.GetComponentInParent<Asteroid>() is Asteroid asteroid)
            {
                Debug.Log("ASTEROID FOUND, mining");   // C
                asteroid.Mine(mineRate * Time.deltaTime);
            }
        }
        else
        {
            beam.enabled = false;
        }
    }
}
