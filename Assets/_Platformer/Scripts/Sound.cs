///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/02/2020 12:53
///-----------------------------------------------------------------

using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;


namespace Com.IsartDigital.Platformer {

	[System.Serializable]
	public class Sound
	{
		[SerializeField] private string _name = null;
		public string Name => _name;

		[SerializeField] private AudioClip _clip = null;
		public AudioClip Clip => _clip;

		public SoundTypes type;
		[Space]
		[Space]

        #region Volume properties
        [Range(0f, 1f)]
		[SerializeField] private float _volume = .75f;
		public float Volume => _volume;

		[Range(0f, 1f)]
		[SerializeField] private float _volumeVariance = .1f;
		public float VolumeVariance => _volumeVariance;
        #endregion

		[Space]
		[Space]

        #region Pitch properties
        [Range(.1f, 3f)]
		[SerializeField] private float _pitch = 1f;
		public float Pitch => _pitch;

		[Range(0f, 1f)]
		[SerializeField] private float _pitchVariance = .1f;
		public float PitchVariance => _pitchVariance;

		[Space]
		[SerializeField] private bool _isPitchedBetweenValues = false;
		public bool IsPitchedBetweenValues => _isPitchedBetweenValues;

		[SerializeField] private float _minPitchValue = 0f;
		public float MinPitchValue => _minPitchValue;

		[SerializeField] private float _maxPitchValue = 0f;
		public float MaxPitchValue => _maxPitchValue;
        #endregion

		[Space]
		[Space]

        #region Loop properties
        [SerializeField] private bool _isLoop = false;
		public bool IsLoop => _isLoop;
        #endregion

        [SerializeField] private AudioMixerGroup _mixerGroup = null;
		public AudioMixerGroup MixerGroup => _mixerGroup;

		private AudioSource _source = null;
		public AudioSource Source { get => _source; set { _source = value; } }

		public void SetNewSource(AudioSource newSource)
		{
			Source = newSource;

			Source.clip = Clip;
			Source.volume = Volume;
			Source.pitch = Pitch;
			Source.loop = IsLoop;
			Source.outputAudioMixerGroup = MixerGroup;
		}
	}
	public enum SoundTypes
	{
		MUSIC,
		SFX
	}
}