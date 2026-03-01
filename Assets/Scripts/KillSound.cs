using UnityEngine;

public class KillSound : MonoBehaviour
{

    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        Destroy(gameObject, source.clip.length);
    }
}
