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
    [SerializeField] private Vector2 windowedResolution;

    private void Start()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        if (PlayerPrefs.GetInt("IsFullScreen") == 1)
        {
            fullscreenToggle.isOn = true;
        }
        else
        {
            fullscreenToggle.isOn = false;
        }
    }

    public void SetSoundEffectsVolume(float level)
    {
        PlayerPrefs.SetFloat("SfxVolume", level);
        audioMixer.SetFloat("soundEffectsVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        PlayerPrefs.SetFloat("MusicVolume", level);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }
    
    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        if (isFullScreen)
        {
            Resolution currRes = Screen.currentResolution;
            Screen.SetResolution(currRes.width, currRes.height, true);
            PlayerPrefs.SetInt("IsFullScreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("IsFullScreen", 0);
            Screen.SetResolution((int)windowedResolution.x, (int)windowedResolution.y, false);
        }
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
