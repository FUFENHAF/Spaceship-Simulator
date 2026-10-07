using UnityEngine;

public class FractureAsteroid : MonoBehaviour
{
    [Tooltip("\"Fractured\" is the object that this will break into")]
    public GameObject fractured;

    [Header("Explosion")]
    [SerializeField] private float fragmentSpeed = 2f;      // jak rychle odletí
    [SerializeField] private float fragmentSpin = 1f;     // jak se roztočí
    [SerializeField] private float fragmentLifetime = 30f; // kdy zmizí (úklid scény)

    public void FractureObject()
    {
        GameObject broken = Instantiate(fractured, transform.position, transform.rotation);

        Rigidbody ownRb = GetComponent<Rigidbody>();

        foreach (Rigidbody rb in broken.GetComponentsInChildren<Rigidbody>())
        {
            rb.linearVelocity = ownRb != null ? ownRb.linearVelocity : Vector3.zero;

            Vector3 away = (rb.position - transform.position).normalized;
            rb.linearVelocity += away * fragmentSpeed;
            rb.angularVelocity = Random.insideUnitSphere * fragmentSpin;

            Destroy(rb.gameObject, fragmentLifetime + Random.Range(0f, 5f));
        }

        Destroy(gameObject);
    }
}