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

		[SerializeField] private AudioMixerGroup mainMixerGroupLvl1;
		[SerializeField] private AudioMixerGroup mainMixerGroupLvl2;
		[SerializeField] private AudioMixerGroup pauseMixerGroup;
		
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
			}
		}

		/// <summary>
		/// Play a sound whose name is the parameter sound 
		/// </summary>
		/// <param name="sound">name of the sound you want to play</param>
		/// <param name="isForcePlay">want to restart the sound at the beginning if is already playing</param>
		public void Play(string sound,bool isForcePlay = false)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + sound + " not found!");
				return;
			}
			if (currentSound.Source == null)
			{
				currentSound.SetNewSource(gameObject.AddComponent<AudioSource>());
				if (currentSound.MixerGroupLvl1 == null) currentSound.Source.outputAudioMixerGroup = mainMixerGroupLvl1;
			}
			if (currentSound.Source.isPlaying && isForcePlay) 
			{
				//Debug.LogWarning("Sound: " + sound + " is already playing!");
				return;
			}

			if (currentSound.IsFadeIn) FadeIn(currentSound);
			else currentSound.Source.volume = currentSound.Volume * (1 + UnityEngine.Random.Range(-currentSound.VolumeVariance / 2, currentSound.VolumeVariance / 2));

			currentSound.Source.pitch = currentSound.IsPitchedBetweenValues ?
										UnityEngine.Random.Range(currentSound.MinPitchValue, currentSound.MaxPitchValue) :
										currentSound.Source.pitch = currentSound.Pitch * (1 + UnityEngine.Random.Range(-currentSound.PitchVariance / 2, currentSound.PitchVariance / 2));

			if (currentSound.IsStartAtRandomTime) currentSound.Source.time = UnityEngine.Random.Range(0, currentSound.Source.clip.length);
			currentSound.Source.Play();
		}

		/// <summary>
		/// Play a sound whose name is the parameter sound on a specific gameObject
		/// </summary>
		/// <param name="sound">name of the sound you want to play</param>
		/// <param name="emitter">object which call the Play method</param>
		/// <param name="isForcePlay">want to restart the sound at the beginning if is already playing</param>
		public void Play(string sound, ALevelObject emitter , bool isForcePlay = false)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);

			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + sound + " not found!");
				return;
			}

			if (currentSound.Type == SoundTypes.SFX_ClassicPause || currentSound.Type == SoundTypes.SFX_MixerPause)
			{
				Sound emitSound = emitter.sfxList.Find(x => x.Name == sound);

				if (emitSound != null)
				{
					currentSound = emitSound;
				}
				else
				{
					emitter.sfxList.Add(new Sound());
					emitSound = emitter.sfxList[emitter.sfxList.Count - 1];
					emitSound.DuplicateValues(currentSound);

					currentSound = emitSound;
					AudioSource source = emitter.gameObject.AddComponent<AudioSource>();
					currentSound.SetNewSource(source);

					soundsList.Add(currentSound);
				}

			}

			if (currentSound.Source.isPlaying && !isForcePlay)
			{
				//Debug.LogWarning("Sound: " + sound + " is already playing!");
				return;
			}
			currentSound.Source.volume = currentSound.Volume * (1 + UnityEngine.Random.Range(-currentSound.VolumeVariance / 2, currentSound.VolumeVariance / 2));

			currentSound.Source.pitch = currentSound.IsPitchedBetweenValues ?
										UnityEngine.Random.Range(currentSound.MinPitchValue, currentSound.MaxPitchValue) :
										currentSound.Source.pitch = currentSound.Pitch * (1 + UnityEngine.Random.Range(-currentSound.PitchVariance / 2, currentSound.PitchVariance / 2));

			if (currentSound.IsStartAtRandomTime) currentSound.Source.time = UnityEngine.Random.Range(0, currentSound.Source.clip.length);
			currentSound.Source.Play();
		}

		/// <summary>
		/// Play a sound randomly chosen on a list of sound's names
		/// </summary>
		/// <param name="randomSounds">list of sound's name in which you want to choose randomly a sound</param>
		public void PlayRandom(string[] randomSounds)
		{
			float random = UnityEngine.Random.Range(0, randomSounds.Length - 1);
			int randomIndex = Mathf.CeilToInt(random);

			Play(randomSounds[randomIndex]);
		}

		/// <summary>
		/// Play a sound randomly chosen on a list of sound's names on a specific gameObject
		/// </summary>
		/// <param name="randomSounds">list of sound's name in which you want to choose randomly a sound</param>
		/// <param name="emitter">object which call the PlayRandom method</param>
		public void PlayRandom(string[] randomSounds, ALevelObject emitter)
		{
			float random = UnityEngine.Random.Range(0, randomSounds.Length - 1);
			int randomIndex = Mathf.CeilToInt(random);

			Play(randomSounds[randomIndex], emitter);
		}

		/// <summary>
		/// Stop a sound whose name is the parameter sound
		/// </summary>
		/// <param name="sound">name of the sound you want to play</param>
		public void Stop(string sound)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + sound + " not found!");
				return;
			}

			if (currentSound.Source)
			{
				if (!currentSound.IsFadeOut) currentSound.Source.Stop();
				else FadeOut(currentSound, currentSound.Source.Stop);
			}
		}

		/// <summary>
		/// Stop a sound whose name is the parameter sound on a specific gameObject
		/// </summary>
		/// <param name="sound">name of the sound you want to play</param>
		/// <param name="emitter">object which call the Play method</param>
		public void Stop(string sound, ALevelObject emitter)
		{
			Sound currentSound = Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + sound + " not found!");
				return;
			}

			if (currentSound.Type == SoundTypes.SFX_ClassicPause || currentSound.Type == SoundTypes.SFX_MixerPause)
			{
				Sound emitSound = emitter.sfxList.Find(x => x.Name == sound);

				if (emitSound == null) return;
				else currentSound = emitSound;
			}

			if (currentSound.Source)
			{
				if (!currentSound.IsFadeOut) currentSound.Source.Stop();
				else FadeOut(currentSound, currentSound.Source.Stop);
			}
		}

		/// <summary>
		/// Pause a sound whose name is the parameter sound
		/// </summary>
		/// <param name="sound">name of the sound you want to pause</param>
		public void Pause(string sound)
		{
			Sound currentSound = System.Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + sound + " not found!");
				return;
			}
			Pause(currentSound);
		}

		/// <summary>
		/// Pause a sound from Sound object
		/// </summary>
		/// <param name="sound">Sound object you want to pause</param>
		private void Pause (Sound sound)
		{
			if (!sound.IsFadeOut) sound.Source.Pause();
			else FadeOut(sound, sound.Source.Pause);
		}

		public void PauseByMixer(string sound)
		{
			Sound currentSound = System.Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			PauseByMixer(currentSound);
		}

		private void PauseByMixer(Sound sound)
		{
			if (sound.PauseMixerGroup != null) sound.Source.outputAudioMixerGroup = sound.PauseMixerGroup;
			else sound.Source.outputAudioMixerGroup = pauseMixerGroup;
		}

		public void PauseAll()
		{
			playedSounds.RemoveRange(0, playedSounds.Count);

			for (int i = sounds.Length - 1; i >= 0; i--)
			{
				Sound sound = sounds[i];
				
				if (sound.Source != null && sound.Source.isPlaying)
				{
					if (sound.Type == SoundTypes.SFX_ClassicPause) Pause(sound);
					else if (sound.Type == SoundTypes.SFX_MixerPause || sound.Type == SoundTypes.MUSIC) PauseByMixer(sound);

					if (sound.Type != SoundTypes.UI) playedSounds.Add(sound);
				}
			}
		}

		//public void PauseAllByMixerGroup()
		//{
		//	playedSounds.RemoveRange(0, playedSounds.Count);
		//	for (int i = sounds.Length - 1; i >= 0; i--)
		//	{
		//		Sound testedSound = sounds[i];
		//		if (testedSound.Source.isPlaying)
		//		{
		//			playedSounds.Add(testedSound);
		//			testedSound.Source.outputAudioMixerGroup = pauseMixerGroup;
		//		}
		//	}
		//}

		public void Resume(string sound)
		{
			Sound currentSound = System.Array.Find(sounds, searchedSound => searchedSound.Name == sound);
			if (currentSound == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			Pause(currentSound);
		}

		private void Resume(Sound sound)
		{
			if (!sound.IsFadeIn) sound.Source.UnPause();
			else FadeIn(sound, sound.Source.UnPause);
		}

		private void ResumeByMixer(Sound sound, int lvlNumber = 1)
		{
			if (lvlNumber == 1)
			{
				if (sound.MixerGroupLvl1 != null) sound.Source.outputAudioMixerGroup = sound.MixerGroupLvl1;
				else sound.Source.outputAudioMixerGroup = mainMixerGroupLvl1;
			}
			else if (lvlNumber == 2)
			{
				if (sound.MixerGroupLvl2 != null) sound.Source.outputAudioMixerGroup = sound.MixerGroupLvl2;
				else sound.Source.outputAudioMixerGroup = mainMixerGroupLvl2;
			}
		}

		public void ResumeAll(int lvlNumber = 1)
		{
			Sound sound;

			for (int i = playedSounds.Count - 1; i >= 0; i--)
			{
				sound = playedSounds[i];

				if (sound.Type == SoundTypes.SFX_ClassicPause) Resume(sound);
				else if (sound.Type == SoundTypes.SFX_MixerPause || sound.Type == SoundTypes.MUSIC) ResumeByMixer(sound,lvlNumber);

				if (sound.Type != SoundTypes.UI) playedSounds.Remove(sound);
			}
		}

		//public void ResumeAllByMixerGroup()
		//{
		//	playedSounds.RemoveRange(0, playedSounds.Count);
		//	for (int i = sounds.Length - 1; i >= 0; i--)
		//	{
		//		Sound testedSound = sounds[i];
		//		if (testedSound.Source.isPlaying)
		//		{
		//			playedSounds.Add(testedSound);
		//			if (testedSound.MixerGroupLvl1 == null) testedSound.Source.outputAudioMixerGroup = mainMixerGroupLvl1;
		//			else testedSound.Source.outputAudioMixerGroup = testedSound.MixerGroupLvl1;
		//		}
		//	}
		//}

		private void FadeIn(Sound sound, Action action = null)
		{
			StartCoroutine(Fade(sound, sound.FadeInCurve, action,true));
		}

		private void FadeOut(Sound sound, Action action = null)
		{
			StartCoroutine(Fade(sound, sound.FadeOutCurve, action));
		}

		private IEnumerator Fade(Sound sound, AnimationCurve curve, Action action = null, bool isActionOnStart = false)
		{
			float elapsedTime = 0f;
			float ratio = curve.Evaluate(0);

			if (isActionOnStart) action();

			while (ratio <= curve.Evaluate(1))
			{
				elapsedTime += Time.deltaTime;
				ratio = curve.Evaluate(elapsedTime / sound.FadeInDuration);
				sound.Source.volume = sound.Volume * ratio;
				yield return null;
			}
			elapsedTime = 0f;

			if (!isActionOnStart) action();
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