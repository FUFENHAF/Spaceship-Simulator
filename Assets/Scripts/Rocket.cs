using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject explosionPrefab;

    public AudioManager audioManager;

    private void OnTriggerEnter(Collider other)
    {
        ShipController ship = other.GetComponent<ShipController>();
        Asteroid asteroid = other.GetComponentInParent<Asteroid>();

        if (ship != null)
        {
            ship.UpdateHealth(-damage);
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
                    
            audioManager.PlayExplosion();
            Destroy(gameObject);
        }
        else if (asteroid != null)
        {
            asteroid.Mine(damage);
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

                    
            audioManager.PlayExplosion();
            Destroy(gameObject);
        }
    }
}