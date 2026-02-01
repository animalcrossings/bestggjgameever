using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using System.Collections;



public class AudioManager : MonoBehaviour
{
    private const float _DEFAULT_AUDIO_PITCH = 1.0f;
    public static AudioManager Instance { get; private set; }
    [SerializeField] public AudioClip backgroundMusic;
    [SerializeField] public AudioClip breakBlockSound;
    [SerializeField] public AudioClip clickUIButtonSound;
    [SerializeField] public AudioClip cloneSound;
    [SerializeField] public AudioClip gainMaskSound;
    [SerializeField] public AudioClip keyCollectSound;
    [SerializeField] public AudioClip levelCompleteSound; // TODO:
    [SerializeField] public AudioClip moveSound;
    [SerializeField] public AudioClip pushBlockSound; // TODO:
    [SerializeField] public AudioClip teleportSound; // TODO:
    [SerializeField] public AudioSource soundFXSource;
    [SerializeField] public AudioSource musicSource;

    [Tooltip("Minimum interval between footstep sounds in seconds.")]
    [SerializeField] public float footstepInterval = 0.5f;
    // pitch range for footstep sounds
    [SerializeField] public float minFootstepPitch = 0.6f;
    [SerializeField] public float maxFootstepPitch = 1.4f;

    private float _lastFootstepTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlaySound(AudioClip clip)
    {
        Debug.Log($"[AudioManager] Playing sound: {clip.name}");
        soundFXSource.PlayOneShot(clip);
    }

    public void PlayFootstepSound()
    {
        if (Time.time - _lastFootstepTime >= footstepInterval)
        {
            float pitch = Random.Range(minFootstepPitch, maxFootstepPitch);
            StartCoroutine(PlayAndResetPitch(soundFXSource, moveSound, pitch));
            _lastFootstepTime = Time.time;
        }
    }

    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void StopBackgroundMusic()
    {
        if(musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void PauseBackgroundMusic()
    {
        if(musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void ResumeBackgroundMusic()
    {
        if(!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    private IEnumerator PlayAndResetPitch(AudioSource audioSource, AudioClip clip, float temporaryPitch)
    {
        audioSource.pitch = temporaryPitch;
        audioSource.clip = clip;
        
        audioSource.Play();

        yield return new WaitWhile(() => audioSource.isPlaying);

        audioSource.pitch = _DEFAULT_AUDIO_PITCH;
    }



}