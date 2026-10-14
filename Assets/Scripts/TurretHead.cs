using UnityEngine;

public class TurretHead : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float range = 30f;
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private string playerTag = "Player";

    private Transform _player;

    private void Update()
    {
        if (_player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag(playerTag);
            if (found != null) _player = found.transform;
            else return;
        }

        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance > range) return;

        Vector3 toPlayer = _player.position - transform.position;
        toPlayer.y = 0f;

        if (toPlayer.sqrMagnitude < 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}