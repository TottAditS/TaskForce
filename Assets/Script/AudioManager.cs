using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] MusicSound, SFXSound;
    public AudioSource MSource, SSource;
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (MSource == null) MSource = gameObject.AddComponent<AudioSource>();
            if (SSource == null) SSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool IsPlaying(string name)
    {
        //Sound s = Array.Find(musicsounds, x => x.sname == name) ?? Array.Find(sfxsounds, x => x.sname == name);
        //return (Musicsource.clip == s.clip && Musicsource.isPlaying) || (SFXsource.clip == s.clip && SFXsource.isPlaying);

        Sound s = Array.Find(MusicSound, x => x.soundname == name) ?? Array.Find(SFXSound, x => x.soundname == name);

        if (s == null)
        {
            Debug.LogWarning($"Sound with name {name} not found.");
            return false;
        }

        bool isMusicPlaying = MSource.clip == s.clip && MSource.isPlaying;
        bool isSFXPlaying = SSource.clip == s.clip && SSource.isPlaying;

        return isMusicPlaying || isSFXPlaying;
    }

    public void playM(string name)
    {
        Sound s = Array.Find(MusicSound, x => x.soundname == name);
        //Debug.Log("bunyi suara musik");
        if (s != null)
        {
            MSource.clip = s.clip;
            //Debug.Log(s.sname);
            MSource.Play();
        }
    }

    public void playS(string name)
    {

        //Debug.Log("bunyi suara sfx");
        Sound s = Array.Find(SFXSound, x => x.soundname == name);

        if (s != null)
        {
            SSource.clip = s.clip;
            //Debug.Log(s.sname);
            SSource.PlayOneShot(s.clip);
            //Debug.Log("Playing" + s.sname);
        }
    }
    public void StopM()
    {
        MSource.Stop();
    }
    public void StopS()
    {
        SSource.Stop();
    }
    public void SetMVol(float vol)
    {
        MSource.volume = vol;

        PlayerPrefs.SetFloat("MusicVolumes", vol);
        PlayerPrefs.Save();
    }
    public void SetSVol(float vol)
    {
        SSource.volume = vol;

        PlayerPrefs.SetFloat("SFXVolumes", vol);
        PlayerPrefs.Save();
    }
}
