using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject objectToHide;
    [SerializeField] private ButtonTextVisual backButton;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Toggle fullscreenToggle;

    private static float SoundEffectsVolume = 0.7f;
    private static float MusicVolume = 0.7f;
    private static bool IsFullscreen = true;

    private void Start()
    {
        Debug.Log("Start settings");
        sfxSlider.value = SoundEffectsVolume;
        musicSlider.value = MusicVolume;
        fullscreenToggle.isOn = IsFullscreen;
    }

    private void SetMaterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSoundEffectsVolume(float level)
    {
        Debug.Log(level);
        SoundEffectsVolume = level;
        audioMixer.SetFloat("soundEffectsVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        MusicVolume = level;
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }

    //SoundEffectsManager.instance.PlaySoundEffectClip(SoundEffectsManager.instance.buttonHover, transform);
    //SoundEffectsManager.instance.PlaySoundEffectClip(SoundEffectsManager.instance.buttonClick, transform);
    
    public void SetFullscreen(bool isFullScreen)
    {
        IsFullscreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    public void ToggleSettingsVisible(bool isVisible)
    {
        if(!isVisible)
        {
            backButton.OnPointerExit();
        }

        objectToHide.SetActive(!isVisible);
        settingsUI.SetActive(isVisible);
    }
}
