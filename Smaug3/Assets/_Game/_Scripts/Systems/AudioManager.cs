using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private Music[] musics;
    [SerializeField] private SFX[] sfxs;

    [SerializeField, Range(0f, 1f)] private float masterVolume;

    // Musics
    private GameObject lastMusic;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AudioListener.volume = masterVolume;
    }

    public void PlaySFX(string name)
    {
        foreach (SFX s in sfxs )
        {
            if (s.Clip.name == name)
            {
                var sfx = new GameObject("SFX " + s.Clip.name);
                var sAudioSource = sfx.AddComponent<AudioSource>();
                sAudioSource.clip = s.Clip;
                sAudioSource.volume = s.Volume;
                sAudioSource.Play();
                Destroy(sfx, 5f);
                break;
            }
        }    
    }

    public void PlayMusic(string name)
    {
        if (lastMusic != null) Destroy(lastMusic); // TODO: Efeito de FadeOut
        
        foreach (Music m in musics)
        {
            if (m.Clip.name == name)
            {
                var music = new GameObject("Music " + m.Clip.name);
                var mAudioSource = music.AddComponent<AudioSource>();
                mAudioSource.clip = m.Clip;
                mAudioSource.volume = m.Volume;
                mAudioSource.Play();
                mAudioSource.loop = true;

                lastMusic = music;
                break;
            }
        }
    }
}
