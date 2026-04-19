using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] Slider volumeSlider;

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume",  volumeSlider.value);
    }
        void Awake()
    {
        instance = this;
    }

    // Sound effects
    public AudioClip sfx_landing, sfx_jumping, sfx_cherry, sfx_enemyDeath;

    // Music
    public AudioClip BGMusic, HurryUpAndRun, CaveDiving, UnderTheRainbow, HurtAndHeart;

    // Music source (separate from SFX objects)
    public AudioSource musicSource;

    // Sound object
    public GameObject soundObject;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Main Menu" || sceneName == "Character Select")
        {
            Music("HurryUpAndRun");
        }
        else if (sceneName == "GameScene2")
        {
            Music("CaveDiving");
        }
        else if (sceneName == "GameScene3")
        {
            Music("UnderTheRainbow");
        }
        else if (sceneName == "End Credits")
        {
            Music("HurtAndHeart");
        }
        else
        {
            Music("BGMusic");
        }

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        }

        Load();
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
            case "enemy death":
                SoundObjectCreation(sfx_enemyDeath);
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
            case "HurryUpAndRun":
                MusicObjectCreation(HurryUpAndRun);
                break;
            case "CaveDiving":
                MusicObjectCreation(CaveDiving);
                break;
            case "UnderTheRainbow":
                MusicObjectCreation(UnderTheRainbow);
                break;
            case "HurtAndHeart":
                MusicObjectCreation(HurtAndHeart);
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
