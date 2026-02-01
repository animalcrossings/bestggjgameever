
using System.Runtime.CompilerServices;
using UnityEngine;


public class RegionSound : MonoBehaviour {
    [Header("Sound Settings")]
    [Tooltip("The audio clip to play when player enters this region")]
    public AudioClip regionSound;

    [Tooltip("Volume of the sound (0.0 to 1.0)")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    [Tooltip("Should the sound loop continuously while in the region?")]
    public bool loop = false;

    [Tooltip("Fade in duration in seconds")]
    public float fadeInDuration = 1.0f;
    
    [Tooltip("Fade out duration in seconds")]
    public float fadeOutDuration = 1.0f;

    [Header("Behavior Settings")]
    [Tooltip("Stop sound when player exits region?")]
    public bool stopOnExit = true;
    
    [Tooltip("Restart sound if player re-enters?")]
    public bool restartOnReenter = true;

    private AudioSource audioSource;
    private bool isPlayerInside = false;
    private float targetVolume = 0f;
    private float currentVolume = 0f;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = regionSound;
        audioSource.loop = loop;
        audioSource.volume = 0f; // Start silent
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound

        targetVolume = volume;

        
    }

    private void Update()
    {

        if(isPlayerInside && currentVolume < targetVolume)
        {
            currentVolume += (targetVolume / fadeInDuration) * Time.deltaTime;
            audioSource.volume = Mathf.Min(currentVolume, targetVolume);
        }
        else if(!isPlayerInside && currentVolume > 0f)
        {
            currentVolume -= (targetVolume / fadeOutDuration) * Time.deltaTime;
            audioSource.volume = Mathf.Max(currentVolume, 0f);

            if(currentVolume <= 0f && stopOnExit)
            {
                audioSource.Stop();
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            
            isPlayerInside = true;
            if(restartOnReenter || !audioSource.isPlaying)
            {
                audioSource.Play();

                if(AudioManager.Instance != null)
                {
                    Debug.Log("[RegionSound] Notifying AudioManager of region sound play.");
                    AudioManager.Instance.PauseBackgroundMusic();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isPlayerInside = false;

            if (stopOnExit && fadeOutDuration <= 0f)
            {
                audioSource.Stop();
                currentVolume = 0f;
            }

            if(AudioManager.Instance != null)
            {
                Debug.Log("[RegionSound] Notifying AudioManager of region sound stop.");
                AudioManager.Instance.ResumeBackgroundMusic();
            }
        }
    }

    public void ForceStopSound()
    {
        audioSource.Stop();
        currentVolume = 0f;
        isPlayerInside = false;
    }

    public void ForcePlaySound()
    {
        isPlayerInside = true;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    


}