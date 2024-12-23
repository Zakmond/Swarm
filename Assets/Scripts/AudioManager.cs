
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public float sfxVolume = 1f;
    public float musicVolume = 1f;
    public float basicMusicVolume = 0.3f;

    private AudioSource music;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            music = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSfx(float volume)
    {
        sfxVolume = volume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        music.volume = volume * basicMusicVolume;
    }

}