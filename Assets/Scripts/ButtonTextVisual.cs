using TMPro;
using UnityEngine;

public class ButtonTextVisual : MonoBehaviour
{
    [SerializeField] private TMP_ColorGradient hoverColor;
    [SerializeField] private TMP_ColorGradient selectedColor;
    [SerializeField] private TMP_ColorGradient normalColor;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private float scaleMultiplier = 1.2f;

    private Vector3 startScale;

    private void Start()
    {
        startScale = buttonText.transform.localScale;
    }

    public void OnPointerEnter()
    {
        SoundEffectsManager.instance.PlaySoundEffectClip(SoundEffectsManager.instance.buttonHover, transform);

        buttonText.colorGradientPreset = hoverColor;
        buttonText.transform.localScale *= scaleMultiplier;
    }

    public void OnPointerExit()
    {
        SetNormalColor();
        buttonText.transform.localScale = startScale;

    }

    public void OnPointerDown()
    {
        SoundEffectsManager.instance.PlaySoundEffectClip(SoundEffectsManager.instance.buttonClick, transform);
        buttonText.colorGradientPreset = selectedColor;
    }

    public void SetNormalColor()
    {
        buttonText.colorGradientPreset = normalColor;
    }

}
