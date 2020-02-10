///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:37
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Com.IsartDigital.Platformer.Managers {
	public class SoundManager : MonoBehaviour {

		public static SoundManager instance;

		public AudioMixerGroup mixerGroup;

		public Sound[] sounds;

		void Awake()
		{
			if (instance != null)
			{
				Destroy(gameObject);
			}

			instance = this;
			DontDestroyOnLoad(gameObject);

			for (int i = sounds.Length - 1; i > -1; i--)
			{
				Sound sound = sounds[i];
				sound.Source = gameObject.AddComponent<AudioSource>();
				sound.Source.clip = sounds[i].Clip;
				sound.Source.loop = sounds[i].IsLoop;

				sound.Source.outputAudioMixerGroup = mixerGroup;
			}
		}

		public void Play(string sound)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			currentSound.Source.Play();
		}

		public void Stop(string sound)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			currentSound.Source.Stop();
		}
	}
}