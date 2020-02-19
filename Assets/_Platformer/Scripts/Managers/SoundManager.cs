///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:37
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Audio;

namespace Com.IsartDigital.Platformer.Managers
{
	public class SoundManager : MonoBehaviour {

		private static SoundManager _instance;
		public static SoundManager Instance => _instance;

		public AudioMixerGroup mixerGroup;
		
		public Sound[] sounds;

		private List<Sound> soundsList = new List<Sound>();

		private List<Sound> playedSounds = new List<Sound>();

		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
				return;
			}
			else _instance = this;

			DontDestroyOnLoad(this.gameObject);

			for (int i = sounds.Length - 1; i > -1; i--)
			{
				Sound sound = sounds[i];
				sound.Source = gameObject.AddComponent<AudioSource>();
				sound.Source.clip = sounds[i].Clip;
				sound.Source.loop = sounds[i].IsLoop;

				sound.Source.outputAudioMixerGroup = mixerGroup;
			}
		}

		public Sound Load(string sound,GameObject currentGameObject,AudioSource audioSource = null)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);

			Sound loadedSound = new Sound();
			soundsList.Add(loadedSound);

			//Teste si le son est présent dans le soundmanager
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return null;
			}

			//Teste si un audiosource est present sur le gameobject appelant la methode
			if (audioSource == null)
			{
				audioSource =  currentGameObject.GetComponent<AudioSource>() == null ?
					currentGameObject.AddComponent<AudioSource>() : currentGameObject.GetComponent<AudioSource>();
			}
			
			loadedSound.Source = audioSource;
			loadedSound.Source.clip = currentSound.Clip;
			loadedSound.Source.loop = currentSound.IsLoop;

			loadedSound.Source.outputAudioMixerGroup = currentSound.MixerGroup;
			soundsList.Add(loadedSound);

			return loadedSound;
		}

		public void Play(string sound)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);

			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			else if (currentSound.Source.isPlaying) 
			{
				//Debug.LogWarning("Sound: " + name + " is already playing!");
				return;
			}

			currentSound.Source.volume = currentSound.Volume * (1 + UnityEngine.Random.Range(-currentSound.VolumeVariance / 2, currentSound.VolumeVariance / 2));

			currentSound.Source.pitch = currentSound.IsPitchedBetweenValues ?
										UnityEngine.Random.Range(currentSound.MinPitchValue, currentSound.MaxPitchValue) :
										currentSound.Source.pitch = currentSound.Pitch * (1 + UnityEngine.Random.Range(-currentSound.PitchVariance / 2, currentSound.PitchVariance / 2));
			currentSound.Source.Play();
		}

		/// <summary>
		/// DO NOT USE
		/// </summary>
		/// <param name="sound"></param>
		public void Play(Sound sound)
		{
			AudioSource source = sound.Source;

			if (source.isPlaying) return;

			source.volume = sound.Volume * (1 + UnityEngine.Random.Range(-sound.VolumeVariance / 2, sound.VolumeVariance / 2));

			source.pitch = sound.IsPitchedBetweenValues ?
							UnityEngine.Random.Range(sound.MinPitchValue, sound.MaxPitchValue) :
							source.pitch = sound.Pitch * (1 + UnityEngine.Random.Range(-sound.PitchVariance / 2, sound.PitchVariance / 2));
			source.Play();
		}

		public void Stop(string sound)
		{
			Sound currentSound = System.Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			currentSound.Source.Stop();
		}

		/// <summary>
		/// DO NOT USE
		/// </summary>
		/// <param name="sound"></param>
		public void Stop(Sound sound)
		{
			sound.Source.Stop();
		}

		public void Pause(string sound)
		{
			Sound currentSound = System.Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			currentSound.Source.Pause();
		}

		/// <summary>
		/// DO NOT USE
		/// </summary>
		/// <param name="sound"></param>
		public void Pause(Sound sound)
		{
			sound.Source.Pause();
		}

		public void PauseAll()
		{
			playedSounds.RemoveRange(0, playedSounds.Count);
			for (int i = sounds.Length - 1; i >= 0; i--)
			{
				Sound testedSound = sounds[i];
				if (testedSound.Source.isPlaying)
				{
					playedSounds.Add(testedSound);
					testedSound.Source.Pause();
				}
			}
		}

		public void ResumeAll()
		{
			for (int i = playedSounds.Count - 1; i >= 0; i--)
			{
				playedSounds[i].Source.UnPause();
				playedSounds.Remove(playedSounds[i]);
			}
		}

#if UNITY_EDITOR
		#region EditorMethods
		public void AddSound()
		{
			ArrayUtility.Add<Sound>(ref sounds, new Sound());
		}

		public void RemoveSound(string sound)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);

			ArrayUtility.Remove<Sound>(ref sounds, currentSound);
		}

		public void RemoveLastSound()
		{
			ArrayUtility.RemoveAt<Sound>(ref sounds, sounds.Length-1);
		}
		#endregion
#endif
	}
}