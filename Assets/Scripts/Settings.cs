using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    // All volume slider should be from 0.0001 to 1

    private void SetMaterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20);
    }

    private void SetSoundEffectsVolume(float level)
    {
        audioMixer.SetFloat("soundEffectsVolume", Mathf.Log10(level) * 20);
    }

    private void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20);
    }


    /* Key rebinding
    private void Update()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(keyCode))
            {
                Debug.Log(keyCode);
            }
        }
    }
    */

}
