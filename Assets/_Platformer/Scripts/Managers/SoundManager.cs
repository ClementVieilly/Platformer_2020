///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:37
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using Com.IsartDigital.Platformer.Sounds;
using System;
using System.Collections;
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
				soundsList.Add(sounds[i]);
				sound.SetNewSource(gameObject.AddComponent<AudioSource>());
				sound.Source.outputAudioMixerGroup = mixerGroup;
			}
		}

		public void Play(string sound)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);

			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + sound + " not found!");
				return;
			}
			if (currentSound.Source.isPlaying) 
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



		private IEnumerator FadeIn(Sound sound)
		{
			//float elapsedTime = 0f;
			//float ratio = sound.FadeInCurve.Evaluate(0);

			//while()

			yield return null;
		}


		//private IEnumerator FadeIn(Sound sound)
		//{
		//	float elapsed = 0f;
		//	float lRatio = fadeInCurve.Evaluate(0);
		//	AnimationCurve lCurve = fadeIn ? fadeInCurve : fadeOutCurve;
		//	Debug.Log(sound.isPaused ? "resume" : "play");
		//	sound.source.volume = 0f;
		//	while (lRatio != fadeInCurve.Evaluate(cooldownTimeForFades / cooldownTimeForFades))
		//	{
		//		elapsed += Time.deltaTime;
		//		lRatio = fadeInCurve.Evaluate(elapsed / cooldownTimeForFades);
		//		sound.source.volume = sound.volume * lRatio;
		//		Debug.Log("source volume : " + sound.source.volume + "ratio volume : " + lRatio);
		//		yield return null;
		//	}
		//	elapsed -= cooldownTimeForFades;
		//	Debug.Log("on termine fadeIn");
		//}

		/// <summary>
		/// Coroutine needed for fades, fadeIn false => fadeOut
		/// </summary>
		/// <param name="sound"></param>
		/// <param name="fadeCurve"></param>
		/// <returns></returns>
		//private IEnumerator Fade(Sound sound, AnimationCurve fadeCurve, float targetedVolume = 1)
		//{
		//	float elapsed = 0f;
		//	float lRatio = 1f - targetedVolume;
		//	float volumeInitial = 1 - targetedVolume;
		//	AnimationCurve lCurve = fadeCurve;
		//	//AnimationCurve lCurve = fadeIn ? fadeInCurve : fadeOutCurve;
		//	sound.source.volume = lCurve.Evaluate(0);
		//	while (lRatio != lCurve.Evaluate(1))
		//	{
		//		elapsed += Time.deltaTime;
		//		lRatio = lCurve.Evaluate(elapsed / cooldownTimeForFades);
		//		sound.source.volume = 1f * lRatio;
		//		//Debug.Log("source volume : " + sound.source.volume);
		//		yield return null;
		//	}
		//	elapsed -= cooldownTimeForFades;
		//	Debug.Log("on termine la fade std");
		//}

		/// <summary>
		/// Coroutine needed for fadeIn
		/// </summary>
		/// <param name="sound"></param>
		/// <returns></returns>
		//private IEnumerator FadeIn(Sound sound)
		//{
		//	float elapsed = 0f;
		//	float lRatio = fadeInCurve.Evaluate(0);
		//	AnimationCurve lCurve = fadeIn ? fadeInCurve : fadeOutCurve;
		//	Debug.Log(sound.isPaused ? "resume" : "play");
		//	sound.source.volume = 0f;
		//	while (lRatio != fadeInCurve.Evaluate(cooldownTimeForFades / cooldownTimeForFades))
		//	{
		//		elapsed += Time.deltaTime;
		//		lRatio = fadeInCurve.Evaluate(elapsed / cooldownTimeForFades);
		//		sound.source.volume = sound.volume * lRatio;
		//		Debug.Log("source volume : " + sound.source.volume + "ratio volume : " + lRatio);
		//		yield return null;
		//	}
		//	elapsed -= cooldownTimeForFades;
		//	Debug.Log("on termine fadeIn");
		//}

		/// <summary>
		/// Coroutine needed for fades, fadeIn false => fadeOut
		/// </summary>
		/// <param name="sound"></param>
		/// <param name="fadeCurve"></param>
		/// <returns></returns>
		//private IEnumerator FadeOut(Sound sound)
		//{
		//	float elapsed = 0f;
		//	float lRatio = fadeOutCurve.Evaluate(0);
		//	//AnimationCurve lCurve = fadeIn ? fadeInCurve : fadeOutCurve;
		//	sound.source.volume = 1f;
		//	while (lRatio != fadeOutCurve.Evaluate(1))
		//	{
		//		elapsed += Time.deltaTime;
		//		lRatio = fadeOutCurve.Evaluate(elapsed / cooldownTimeForFades);
		//		sound.source.volume = 1f * lRatio;
		//		//Debug.Log("source volume : " + sound.source.volume);
		//		yield return null;
		//	}
		//	elapsed -= cooldownTimeForFades;
		//	Debug.Log("on termine fadeOut");
		//	sound.source.Pause();
		//}



		public void Play(string sound, ALevelObject emitter)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);

			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + sound + " not found!");
				return;
			}

			if (currentSound.Type == SoundTypes.SFX)
			{
				Sound emitSound = emitter.sfxList.Find(x => x.Name == sound);

				if (emitSound == null)
				{
					emitter.sfxList.Add(new Sound());
					emitSound = emitter.sfxList[emitter.sfxList.Count - 1];
					emitSound.DuplicateValues(currentSound);

					currentSound = emitSound;
					AudioSource source = emitter.gameObject.AddComponent<AudioSource>();
					currentSound.SetNewSource(source);

					soundsList.Add(currentSound);
				}
				else
				{
					currentSound = emitSound;
				}
			}

			if (currentSound.Source.isPlaying)
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

		public void PlayRandom(string[] randomSounds)
		{
			float random = UnityEngine.Random.Range(0, randomSounds.Length - 1);
			int randomIndex = Mathf.CeilToInt(random);

			Play(randomSounds[randomIndex]);
		}

		public void PlayRandom(string[] randomSounds, ALevelObject emitter)
		{
			float random = UnityEngine.Random.Range(0, randomSounds.Length - 1);
			int randomIndex = Mathf.CeilToInt(random);

			Play(randomSounds[randomIndex], emitter);
		}

		public void Stop(string sound)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}

			if (currentSound.Source)
				currentSound.Source.Stop();
		}

		public void Stop(string sound, ALevelObject emitter)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}

			if (currentSound.Type == SoundTypes.SFX)
			{
				Sound emitSound = emitter.sfxList.Find(x => x.Name == sound);

				if (emitSound == null)
					return;
				else
					currentSound = emitSound;
			}

			if (currentSound.Source)
				currentSound.Source.Stop();
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