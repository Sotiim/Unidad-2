using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource; // Cache del AudioSource

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Cachear el AudioSource una sola vez (optimización)
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogError("ERROR: MusicManager no tiene un AudioSource adjunto.");
            }
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }

    // Acceso seguro al AudioSource
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    // --- MÉTODOS ÚTILES ---

    public void PlayMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
            audioSource.Play();
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
            audioSource.volume = volume;
    }
}
