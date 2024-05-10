using TMPro;
using UnityEngine;

public class ButtonTextVisual : MonoBehaviour
{
    [SerializeField] private TMP_ColorGradient hoverColor;
    [SerializeField] private TMP_ColorGradient selectedColor;
    [SerializeField] private TMP_ColorGradient normalColor;
    [SerializeField] private TextMeshProUGUI buttonText;


    public void OnPointerEnter()
    {
        buttonText.colorGradientPreset = hoverColor;
    }

    public void OnPointerExit()
    {
        SetNormalColor();
    }

    public void OnPointerDown()
    {
        buttonText.colorGradientPreset = selectedColor;
    }

    public void SetNormalColor()
    {
        buttonText.colorGradientPreset = normalColor;
    }

}
