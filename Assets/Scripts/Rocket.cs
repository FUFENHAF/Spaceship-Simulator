using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject explosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        ShipController ship = other.GetComponent<ShipController>();

        if (ship != null)
        {
            ship.UpdateHealth(-damage);
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}