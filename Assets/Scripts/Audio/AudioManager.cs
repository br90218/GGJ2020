using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// To play sounds from this script, simply use the method FindObjectOfType<AudioManager>().Play("AudioName");
/// Add your sounds to the sounds array from the AudioManager object in the editor.
/// </summary>

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;
    public AudioMixerGroup musicChannel;
    public AudioMixerGroup sfxChannel;

    public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = s.mixerGroup;
		}

        AudioManager.instance.Play("BGM");
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        if (s.waitTime > 0)
        {
            StartCoroutine("Wait", s);
        }

        else
        {
            s.source.Play();
        }
	}

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }
        s.source.Stop();
    }

    public bool isPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return false;
        }

        bool isPlaying = s.source.isPlaying;

        return isPlaying;
    }

    IEnumerator Wait(Sound s)
    {
        yield return new WaitForSeconds(s.waitTime);
        s.source.Play();
    }

    public void Mute(string mixerGroup)
    {
        if(mixerGroup == "Music")
        {
            musicChannel.audioMixer.SetFloat("musicVolume", -80);
        }

        if(mixerGroup == "SFX")
        {
            musicChannel.audioMixer.SetFloat("sfxVolume", -80);
        }
    }

    public void Unmute(string mixerGroup)
    {
        if (mixerGroup == "Music")
        {
            musicChannel.audioMixer.SetFloat("musicVolume", 0);
        }

        if (mixerGroup == "SFX")
        {
            musicChannel.audioMixer.SetFloat("sfxVolume", 0);
        }
    }
}
