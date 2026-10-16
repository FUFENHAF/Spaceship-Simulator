using UnityEngine;

public class FractureAsteroid : MonoBehaviour
{
    [Tooltip("\"Fractured\" is the object that this will break into")]
    public GameObject fractured;

    [Header("Explosion")]
    [SerializeField] private float fragmentSpeed = 2f;
    [SerializeField] private float fragmentSpin = 1f;


    [Header("Visual")]
    [SerializeField] private GameObject asteroidExplosionVisualPrefab;

    [Header("Cleanup")]
    [SerializeField] private float fragmentLifetimeMin = 5f;
    [SerializeField] private float fragmentLifetimeMax = 10f;

    public void FractureObject()
    {
        GameObject broken = Instantiate(fractured, transform.position, transform.rotation);

        Rigidbody ownRb = GetComponent<Rigidbody>();

        Color color = Color.white;
        Asteroid asteroid = GetComponent<Asteroid>();
        if (asteroid != null)
            color = asteroid.CurrentColor;

        foreach (Rigidbody rb in broken.GetComponentsInChildren<Rigidbody>())
        {
            rb.linearVelocity = ownRb != null ? ownRb.linearVelocity : Vector3.zero;

            Vector3 away = (rb.position - transform.position).normalized;
            rb.linearVelocity += away * fragmentSpeed;
            rb.angularVelocity = Random.insideUnitSphere * fragmentSpin;

            if (rb.TryGetComponent<Renderer>(out Renderer r))
                r.material.color = color;
        }

        float lifetime = Random.Range(fragmentLifetimeMin, fragmentLifetimeMax);
        Destroy(broken, lifetime);
        
        Instantiate(asteroidExplosionVisualPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}