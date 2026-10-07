using UnityEngine;

public class BreakableRock : MonoBehaviour
{
    [Header("Fragments")]
    [SerializeField] private GameObject[] fragmentPrefabs;

    [Header("Explosion")]
    [SerializeField] private float explosionForce = 60f;
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float upwardModifier = 0.3f;

    [SerializeField] private float breakMinImpulse = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= breakMinImpulse)
            Break();
    }

    public void Break()
    {
        Rigidbody ownRb = GetComponent<Rigidbody>();
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        foreach (GameObject prefab in fragmentPrefabs)
        {
            if (prefab == null) continue;

            GameObject fragment = Instantiate(prefab, position, rotation);
            Rigidbody fragmentRb = fragment.GetComponent<Rigidbody>();
            if (fragmentRb == null) continue;

            fragmentRb.linearVelocity = ownRb != null ? ownRb.linearVelocity : Vector3.zero;
            fragmentRb.angularVelocity = ownRb != null ? ownRb.angularVelocity : Vector3.zero;
            fragmentRb.AddExplosionForce(explosionForce, position, explosionRadius, upwardModifier, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}
