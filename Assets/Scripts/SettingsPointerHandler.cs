using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsPointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SoundEffectsManager.instance.PlaySoundEffectClip(SoundEffectsManager.instance.buttonClick, transform);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundEffectsManager.instance.PlaySoundEffectClip(SoundEffectsManager.instance.buttonHover, transform);

    }

}
