using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        ShipController ship = other.GetComponent<ShipController>();

        if (ship != null)
        {
            ship.UpdateHealth(-damage);
            // TODO: explosion here
            Destroy(gameObject);
        }
    }
}