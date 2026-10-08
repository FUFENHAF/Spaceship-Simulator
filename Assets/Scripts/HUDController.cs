using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
    [SerializeField] private Image hpFill;
    [SerializeField] private TMP_Text leftMotorIndicator;
    [SerializeField] private TMP_Text rightMotorIndicator;

    [SerializeField] private Color motorOnColor = Color.white;
    [SerializeField] private Color motorOffColor = Color.gray;

    public void SetHullIntegrity(float current, float max)
    {
        hpFill.fillAmount = Mathf.Clamp01(current / max);
    }

    public void SetMotorState(bool leftOn, bool rightOn)
    {
        leftMotorIndicator.color  = leftOn  ? motorOnColor : motorOffColor;
        rightMotorIndicator.color = rightOn ? motorOnColor : motorOffColor;
    }
}
