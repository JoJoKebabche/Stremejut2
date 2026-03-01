using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    void Awake()
    {
        instance = this;
    }

    // Sound effects
    public AudioClip sfx_landing, sfx_jumping, sfx_cherry;

    // Music
    public AudioClip BGMusic;

    // Music source (separate from SFX objects)
    public AudioSource musicSource;

    // Sound object
    public GameObject soundObject;

    void Start()
    {
        // Play BG music when the game starts
        Music("BGMusic");
    }

    public void SFX(string sfxName)
    {
        switch (sfxName)
        {
            case "landing":
                SoundObjectCreation(sfx_landing);
                break;
            case "cherry":
                SoundObjectCreation(sfx_cherry);
                break;
            case "jumping":
                SoundObjectCreation(sfx_jumping);
                break;
            default:
                break;
        }
    }

    void SoundObjectCreation(AudioClip clip)
    {
        // Create SoundObject gameobject
        GameObject newObject = Instantiate(soundObject, transform);

        // Assign audioclip to its audiosource
        AudioSource source = newObject.GetComponent<AudioSource>();
        source.clip = clip;

        // Play the audio
        source.Play();
    }

    public void Music(string musicName)
    {
        switch (musicName)
        {
            case "BGMusic":
                MusicObjectCreation(BGMusic);
                break;

            default:
                break;
        }
    }

    void MusicObjectCreation(AudioClip clip)
    {
        // Check if theres an existing music source
        if (musicSource == null)
        {
            Debug.LogWarning("MusicSource is missing!");
            return;
        }

        // Assign audioclip to music source
        musicSource.clip = clip;

        // Make the audio source looping
        musicSource.loop = true;

        // Play the audio
        musicSource.Play();
    }
}
