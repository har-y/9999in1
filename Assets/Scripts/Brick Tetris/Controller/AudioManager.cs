using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float fxVolume = 1f;

    public bool musicEnabled = true;
    public bool fxEnabled = true;

    public AudioClip clearRowSound;
    public AudioClip moveSound;
    public AudioClip dropSound;
    public AudioClip gameOverSound;

    public AudioClip backgroundMusic;
    //private AudioClip _randomMusicClip;
    //public AudioClip[] musicClips;

    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        //_randomMusicClip = GetRandomClip(musicClips);
        //PlayBackgroundMusic(_randomMusicClip);
        PlayBackgroundMusic(backgroundMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!musicEnabled || !musicClip || !musicSource)
        {
            return;
        }

        musicSource.Stop();
        musicSource.clip = musicClip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void UpdateMusic()
    {
        if (musicSource.isPlaying != musicEnabled)
        {
            if (musicEnabled)
            {
                //PlayBackgroundMusic(_randomMusicClip);
                PlayBackgroundMusic(backgroundMusic);
            }
            else
            {
                musicSource.Stop();
            }
        }
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        UpdateMusic();
    }

    public AudioClip GetRandomClip(AudioClip[] clips)
    {
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        return randomClip;
    }
}
