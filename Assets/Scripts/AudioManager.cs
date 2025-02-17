using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicAudioSource;
    public AudioSource soundsAudioSource;

    public AudioClip codeFail;
    public AudioClip codeSuccess;
    public AudioClip digitSuccess;
    public AudioClip noteOpen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
