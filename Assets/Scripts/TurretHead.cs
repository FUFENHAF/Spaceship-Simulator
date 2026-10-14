using UnityEngine;

public class TurretHead : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float range = 30f;
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private string playerTag = "Player";

    [Header("Shooting")]
    [SerializeField] private Transform rocketSpawnPoint;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float aimConeAngle = 30f;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private float rocketSpeed = 15f;

    private Transform _player;
    private float _cooldownTimer;

    private void Update()
    {
        if (_player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag(playerTag);
            if (found != null) _player = found.transform;
            else return;
        }

        _cooldownTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance > range) return;

        Vector3 toPlayer = _player.position - transform.position;
        toPlayer.y = 0f;

        if (toPlayer.sqrMagnitude < 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        TryFire();
    }

    private void TryFire()
    {
        if (rocketPrefab == null || rocketSpawnPoint == null) return;
        if (_cooldownTimer > 0f) return;

        Vector3 aimDirection = rocketSpawnPoint.position - transform.position;
        Vector3 playerDirection = _player.position - transform.position;

        if (Vector3.Angle(aimDirection, playerDirection) > aimConeAngle) return;

        GameObject rocket = Instantiate(rocketPrefab, rocketSpawnPoint.position, rocketSpawnPoint.rotation);
        if (rocket.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = (aimDirection.normalized) * rocketSpeed;
        }
        else
        {
            rocket.AddComponent<Rigidbody>();
            rocket.GetComponent<Rigidbody>().linearVelocity = aimDirection.normalized * rocketSpeed;
        }

        _cooldownTimer = fireCooldown;
    }
}