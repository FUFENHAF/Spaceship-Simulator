using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image hpFill;
    [SerializeField] private TMP_Text leftMotorIndicator;
    [SerializeField] private TMP_Text rightMotorIndicator;
    [SerializeField] private TMP_Text cargoText;

    [Header("Colors")]
    [SerializeField] private Color motorOnColor = Color.white;
    [SerializeField] private Color motorOffColor = Color.gray;

    [Header("MotorIndices")]
    [SerializeField] private int motorIndexLeft = 0;
    [SerializeField] private int motorIndexRight = 1;

    public void SetHullIntegrity(float current, float max)
    {
        hpFill.fillAmount = Mathf.Clamp01(current / max);
    }

    public void SetMotorState(int motorIndex, bool state)
    {
        if (motorIndex == motorIndexLeft)
        {
            leftMotorIndicator.color  = state ? motorOnColor : motorOffColor;
        }
        else if (motorIndex == motorIndexRight)
        {
            rightMotorIndicator.color = state ? motorOnColor : motorOffColor;
        }
    }

    public void SetCargo(float current, float max)
    {
        cargoText.text = $"Cargo: {current:0} / {max:0}";
    }
}
