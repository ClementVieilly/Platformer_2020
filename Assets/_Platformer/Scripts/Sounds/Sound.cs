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

		[SerializeField] private float _minDistance = 0;
		public float minDistance => _minDistance;

		[SerializeField] private float _maxDistance = 500;
		public float maxDistance => _maxDistance;

		#endregion

		#region Fade properties

		[SerializeField] private bool _isFadeIn = false;
		public bool IsFadeIn => _isFadeIn;
		
		[SerializeField] private bool _isFadeOut = false;
		public bool IsFadeOut => _isFadeOut;

		[SerializeField] private float _fadeInDuration = 1;
		public float FadeInDuration => _fadeInDuration;

		[SerializeField] private AnimationCurve _fadeInCurve;
		public AnimationCurve FadeInCurve => _fadeInCurve;

		[SerializeField] private AnimationCurve _fadeOutCurve;
		public AnimationCurve FadeOutCurve => _fadeOutCurve;

		[SerializeField] private float _fadeOutDuration = 1;
		public float FadeOutDuration => _fadeOutDuration;

		#endregion

		[SerializeField] private bool _isStartAtRandomTime = false;
		public bool IsStartAtRandomTime => _isStartAtRandomTime;

		//Temp : need to make arrays and choose by level
		[SerializeField] private AudioMixerGroup _mixerGroupLvl1 = null;
		public AudioMixerGroup MixerGroupLvl1 => _mixerGroupLvl1;
		
		[SerializeField] private AudioMixerGroup _mixerGroupLvl2 = null;
		public AudioMixerGroup MixerGroupLvl2 => _mixerGroupLvl2;

		[SerializeField] private AudioMixerGroup _pauseMixerGroup = null;
		public AudioMixerGroup PauseMixerGroup => _pauseMixerGroup;

		[SerializeField] private AudioMixerGroup _transitionMixerGroup = null;
		public AudioMixerGroup TransitionMixerGroup => _transitionMixerGroup;

		public AudioMixerGroup CurrentMixerGroup = null;

		private AudioSource _source = null;
		public AudioSource Source { get => _source; set { _source = value; } }

		public void SetNewSource(AudioSource newSource)
		{
			Source = newSource;
			Source.clip = Clip;
			Source.volume = Volume;
			Source.pitch = Pitch;
			Source.loop = IsLoop;
			CurrentMixerGroup = MixerGroupLvl1;
			Source.outputAudioMixerGroup = CurrentMixerGroup;
			
			Source.rolloffMode = _rolloffMode;
			Source.minDistance = minDistance;
			Source.maxDistance = _maxDistance;
			if (_rolloffMode == AudioRolloffMode.Custom)
			{
				Source.SetCustomCurve(AudioSourceCurveType.CustomRolloff,_volumeSpatialization);
			}

			if (_type == SoundTypes.SFX_ClassicPause || _type == SoundTypes.SFX_MixerPause)
			{
				Source.spatialBlend = 1;
			}
		}

		public void SetMixer1(AudioMixerGroup newMixer)
		{
			SetMixer(_mixerGroupLvl1, newMixer);
		}

		public void SetMixer2(AudioMixerGroup newMixer)
		{
			SetMixer(_mixerGroupLvl2, newMixer);
		}

		public void SetMixerPause(AudioMixerGroup newMixer)
		{
			SetMixer(_pauseMixerGroup, newMixer);
		}

		public void SetMixer(AudioMixerGroup emptyMixer,AudioMixerGroup newMixer)
		{
			emptyMixer = newMixer;
		}
		//public void SetMode(SoundMode mode)
		//{
		//	if (mode == SoundMode.Normal) Source.outputAudioMixerGroup = MixerGroupLvl1;
		//	else if (mode == SoundMode.Pause) Source.outputAudioMixerGroup = PauseMixerGroup;
		//	else if (mode == SoundMode.Transition) Source.outputAudioMixerGroup = TransitionMixerGroup;
		//}

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

			_mixerGroupLvl1 = originSound.MixerGroupLvl1;
			_pauseMixerGroup = originSound.PauseMixerGroup;
			_transitionMixerGroup = originSound.TransitionMixerGroup;

			_source = originSound.Source;
		}

#if UNITY_EDITOR
		public bool showInEditor = true;
#endif
	}
	public enum SoundTypes
	{
		MUSIC,
		SFX_ClassicPause,
		SFX_MixerPause,
		UI
	}	

	//public enum SoundMode
	//{
	//	Normal,
	//	Pause,
	//	Transition
	//}
}