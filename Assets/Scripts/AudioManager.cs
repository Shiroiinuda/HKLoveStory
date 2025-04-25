using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoSingleton<AudioManager>
{
    public float musicVolume;
    public float sfxVolume;
    public float cSoundVolume;


    public AudioMixer mixer;
    public AudioSource SFXSource;
    public AudioSource buttonSFXSource;
    public AudioSource bgmSource;
    public AudioSource CSoundSource;


    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";
    public const string MIXER_CSOUND = "CharacterSoundVolume";
    

    private void Start()
    {
        //Music//
        if (!PlayerPrefs.HasKey(MIXER_MUSIC))
        {
            PlayerPrefs.SetFloat(MIXER_MUSIC, 0.5f);
            LoadMusic();
        }
        else
        {
            LoadMusic();
        }


        //SFX//
        if (!PlayerPrefs.HasKey(MIXER_SFX))
        {
            PlayerPrefs.SetFloat(MIXER_SFX, 1f);
            LoadSFX();
        }
        else
        {
            LoadSFX();
        }

        //Character Sound//
        if (!PlayerPrefs.HasKey(MIXER_CSOUND))
        {
            PlayerPrefs.SetFloat(MIXER_CSOUND, 1f);
            LoadCSound();
        }
        else
        {
            LoadCSound();
        }

        //Vibration//
        if (!PlayerPrefs.HasKey("canVibrate"))
        {
            PlayerPrefs.SetInt("canVibrate", 1);
        }
    }



    public void LoadMusic()
    {
        musicVolume = PlayerPrefs.GetFloat(MIXER_MUSIC);
        
        if (musicVolume == 0.0001 || musicVolume == 0)
        {
            mixer.SetFloat(MIXER_MUSIC, -80);
        }
        else
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);

    }

    public void LoadSFX()
    {
        sfxVolume = PlayerPrefs.GetFloat(MIXER_SFX);
        
        if (sfxVolume == 0.0001 || sfxVolume == 0)
        {
            mixer.SetFloat(MIXER_SFX, -80);
        }
        else
            mixer.SetFloat(MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
        
    }

    public void LoadCSound()
    {
        cSoundVolume = PlayerPrefs.GetFloat(MIXER_CSOUND);
        if (cSoundVolume > 1)
        {
            cSoundVolume = 1;
        }
        if (cSoundVolume == 0.0001 || cSoundVolume == 0)
        {
            mixer.SetFloat(MIXER_CSOUND, -80);
        }
        else
            mixer.SetFloat(MIXER_CSOUND, Mathf.Log10(cSoundVolume) * 20);

    }

    public void PlayMusic(AudioClip musicName)
    {
        if (musicName != null)
        {
            if (bgmSource.isPlaying)
            {
                if (bgmSource.clip == musicName)
                    return;
                bgmSource.Stop();
            }
            bgmSource.clip = musicName;
            bgmSource.Play();
        }
    }

    public void PlaySFX(AudioClip sfxName)
    {
        if (sfxName != null)
        {
            if (SFXSource.isPlaying)
            {
                if(sfxName != SFXSource.clip)
                {
                    SFXSource.Stop();
                    SFXSource.PlayOneShot(sfxName);
                }
            }
            else
                SFXSource.PlayOneShot(sfxName);

        }
    }
    public void PlayCSound(AudioClip npcVoice)
    {
        if (npcVoice != null)
        {
            if (CSoundSource.isPlaying)
            {
                if (npcVoice != CSoundSource.clip)
                {
                    CSoundSource.Stop();
                    CSoundSource.PlayOneShot(npcVoice);
                }
            }
            else
                CSoundSource.PlayOneShot(npcVoice);
        }
    }


    public void PlayButtonSFX(AudioClip sfxName)
    {
        if (sfxName != null)
        {
            if (buttonSFXSource.isPlaying)
                buttonSFXSource.Stop();
            buttonSFXSource.PlayOneShot(sfxName);
        }
    }

    public void StopCSound()
    {
        CSoundSource.Stop();
    }

    public void StopSfx()
    {
        SFXSource.Stop();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    public void adsStopBGM()
    {
        bgmSource.Stop();
    }


    public void ResetAudioAndPlay()
    {
        //AudioSettings.Reset(AudioSettings.GetConfiguration());
        AudioListener.volume = 1;

        if (bgmSource != null)
        {
            bgmSource.volume = 1;
            if (bgmSource.clip != null)
                bgmSource.Play();
        }
    }
}
