using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private Music[] musics;
    [SerializeField] private SFX[] sfxs;

    // Musics
    private float musicCurTime;
    private string musicCurName;

    private AudioSource curMusicAudioSource;

    private GameObject musicCurObj;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject); // Faz com que o objeto mova entre as scenes
        AudioListener.volume = PlayerPrefs.GetFloat("masterVolume"); // Alterando o masterVolume com base no PlayerPrefs
    }

    private void Update()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("masterVolume"); // Alterando o masterVolume com base no PlayerPrefs

        // Retomando o progresso da música que estava tocando
        if (curMusicAudioSource != null) musicCurTime = curMusicAudioSource.time;
    }

    public void PlaySFX(string name)
    {
        // Procure pelo sfx desejado
        foreach (SFX s in sfxs)
        {
            if (s.Clip.name == name) // Caso achar, instancie um objeto com o componente de audio
            {
                var sfx = new GameObject("SFX " + s.Clip.name);
                var sAudioSource = sfx.AddComponent<AudioSource>();
                sAudioSource.clip = s.Clip;
                sAudioSource.volume = s.Volume;
                sAudioSource.pitch = s.Pitch;
                sAudioSource.Play();
                Destroy(sfx, 5f);
                break;
            }
        }
    }

    public void PlayMusic(string name)
    {
        // Procure pela música desejada
        foreach (Music m  in musics)
        {
            if (m.Clip.name == name) // Caso achar, instancie um objeto com o componente de audio
            {
                var mObj = new GameObject("Music " + m.Clip.name);
                var mAudioSource = mObj.AddComponent<AudioSource>();
                mAudioSource.clip = m.Clip;
                mAudioSource.volume = m.Volume;
                if (musicCurTime != 0 && m.Clip.name == musicCurName)
                {
                    mAudioSource.time = musicCurTime;
                }
                else
                {
                    Destroy(musicCurObj);
                }

                musicCurObj = mObj;
                musicCurName = m.Clip.name;
                mAudioSource.Play();
                mAudioSource.loop = true;

                curMusicAudioSource = mAudioSource;
            }
        }
    }
}