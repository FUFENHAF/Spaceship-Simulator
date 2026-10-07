using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Ore")]
    [SerializeField] private float ore = 10f;
    [SerializeField] private float maxOre = 10f;

    [Header("Heat")]
    [SerializeField] private float heatPerSecond = 0.4f;    // ~2.5 s do rozbytí plným laserem
    [SerializeField] private float coolPerSecond = 0.2f;
    [SerializeField] private Color baseColor = Color.gray;
    [SerializeField] private Color hotColor = new Color(1f, 0.3f, 0.1f);

    private Fracture _fracture;
    private Material _material;
    private float _heat;

    private void Awake()
    {
        _fracture = GetComponent<FractureAsteroid>();
        _material = GetComponentInChildren<Renderer>().material;   // .material = vlastní instance, nesdílí se
    }

    public void Mine(float amount)
    {
        ore -= amount;
        _heat = Mathf.Min(_heat + heatPerSecond * Time.deltaTime, 1f);

        if (ore <= 0f)
            _fracture.FractureObject();
    }

    private void Update()
    {
        if (_heat > 0f)
            _heat = Mathf.Max(_heat - coolPerSecond * Time.deltaTime, 0f);

        _material.color = Color.Lerp(baseColor, hotColor, _heat);
    }
}
