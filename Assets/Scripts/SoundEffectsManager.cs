using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectsManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private AudioSource soundEffectsObject;

    [Header("Player audio clips")]
    [SerializeField] public AudioClip[] playerHurt;
    [SerializeField] public AudioClip[] playerWalk;

    [Header("Enemy audio clips")]
    [SerializeField] public AudioClip[] enemyHit;
    [SerializeField] public AudioClip[] enemyWalk;
    [SerializeField] public AudioClip[] enemyDeath;

    [Header("Tower audio clips")]
    [SerializeField] public AudioClip[] towerShot;
    [SerializeField] public AudioClip[] towerShotImpact;
    [SerializeField] public AudioClip[] towerBuild;

    [Header("Build point and tower")]
    [SerializeField] public AudioClip[] buildPointEntered;
    [SerializeField] public AudioClip[] towerEntered;

    [Header("World space UI audio clips")]
    [SerializeField] public AudioClip towerMenuOpened;
    [SerializeField] public AudioClip towerMenuSwitched;
    [SerializeField] public AudioClip towerToBuildSelected;


    [Header("UI audio clips")]
    [SerializeField] public AudioClip buttonHover;
    [SerializeField] public AudioClip buttonClick;


    public static SoundEffectsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of SoundManager in scene");

            Destroy(gameObject);
            return;
        }
        instance = this;

    }

    public void PlaySoundEffectClip(AudioClip audioClip, Transform spawnTransform, float volume = 1f)
    {
        AudioSource audioSource = Instantiate(soundEffectsObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundEffectClip(AudioClip[] audioClips, Transform spawnTransform, float volume = 1f)
    {
        int randomIdx = Random.Range(0, audioClips.Length);

        PlaySoundEffectClip(audioClips[randomIdx], spawnTransform, volume);
    }

}
