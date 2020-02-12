///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:37
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using System;

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
			else if (currentSound.Source.isPlaying) 
			{
				Debug.LogWarning("Sound: " + name + " is already playing!");
				return;
			} 

			currentSound.Source.volume = currentSound.Volume * (1 + UnityEngine.Random.Range(-currentSound.VolumeVariance / 2, currentSound.VolumeVariance / 2));

			currentSound.Source.pitch = currentSound.IsPitchedBetweenValues ?
										UnityEngine.Random.Range(currentSound.MinPitchValue, currentSound.MaxPitchValue) :
										currentSound.Source.pitch = currentSound.Pitch * (1 + UnityEngine.Random.Range(-currentSound.PitchVariance / 2, currentSound.PitchVariance / 2));
			currentSound.Source.Play();
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
	}
}