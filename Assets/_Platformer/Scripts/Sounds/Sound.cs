///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 07/02/2020 12:53
///-----------------------------------------------------------------

using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;


namespace Com.IsartDigital.Platformer.Sounds {

	[System.Serializable]
	public class Sound
	{
		[SerializeField] private string _name = "defaultName";
		public string Name => _name;

		[SerializeField] private AudioClip _clip = null;
		public AudioClip Clip => _clip;

		[SerializeField] private SoundTypes _type;
		public SoundTypes Type
		{
			get {return _type; }
			set 
			{
				_type = value;
			}
		}
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

		#region Spatialization properties

		[SerializeField] private AudioRolloffMode _rolloffMode;
		public AudioRolloffMode RolloffMode => _rolloffMode;

		[SerializeField] AnimationCurve _volumeSpatialization;
		public AnimationCurve VolumeSpatialization => _volumeSpatialization;

		[SerializeField] private float _minDistance;
		public float minDistance => _minDistance;

		[SerializeField] private float _maxDistance;
		public float maxDistance => _maxDistance;

		#endregion

		#region Fade properties

		[SerializeField] private bool _isFadeIn = false;
		public bool IsFadeIn => _isFadeIn;
		
		[SerializeField] private bool _isFadeOut = false;
		public bool IsFadeOut => _isFadeOut;

		[SerializeField] private AnimationCurve _fadeInCurve;
		public AnimationCurve FadeInCurve => _fadeInCurve;

		[SerializeField] private AnimationCurve _fadeOutCurve;
		public AnimationCurve FadeOutCurve => _fadeOutCurve;

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
			
			Source.rolloffMode = _rolloffMode;
			Source.minDistance = minDistance;
			Source.maxDistance = _maxDistance;
			if (_rolloffMode == AudioRolloffMode.Custom)
			{
				Source.SetCustomCurve(AudioSourceCurveType.CustomRolloff,_volumeSpatialization);
			}

			if (_type == SoundTypes.SFX)
			{
				Source.spatialBlend = 1;
			}
		}

		public void DuplicateValues(Sound originSound)
		{
			_name = originSound.Name;
			_clip = originSound.Clip;
			_type = originSound.Type;

			_volume = originSound.Volume;
			_volumeVariance = originSound.VolumeVariance;

			_pitch = originSound.Pitch;
			_pitchVariance = originSound.PitchVariance;

			_minPitchValue = originSound.MinPitchValue;
			_maxPitchValue = originSound.MaxPitchValue;

			_isLoop = originSound.IsLoop;
			_isPitchedBetweenValues = originSound.IsPitchedBetweenValues;

			_rolloffMode = originSound.RolloffMode;
			_volumeSpatialization = originSound.VolumeSpatialization;
			_minDistance = originSound.minDistance;
			_maxDistance = originSound.maxDistance;

			_isFadeIn = originSound.IsFadeIn;
			_isFadeOut = originSound.IsFadeOut;
			_fadeInCurve = originSound.FadeInCurve;
			_fadeOutCurve = originSound.FadeOutCurve;

			_mixerGroup = originSound._mixerGroup;
			_source = originSound.Source;

		}

#if UNITY_EDITOR
		public bool showInEditor = true;
#endif
	}
	public enum SoundTypes
	{
		MUSIC,
		SFX
	}
}