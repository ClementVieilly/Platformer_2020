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
		[SerializeField] private string _name;
		public string Name => _name;

		[SerializeField] private AudioClip _clip;
		public AudioClip Clip => _clip;

		[Range(0f, 1f)]
		[SerializeField] private float _volume = .75f;
		public float Volume => _volume;

		[Range(0f, 1f)]
		[SerializeField] private float _volumeVariance = .1f;
		public float VolumeVariance => _volumeVariance;

		[Range(.1f, 3f)]
		[SerializeField] private float _pitch = 1f;
		public float Pitch => _pitch;

		[Range(0f, 1f)]
		[SerializeField] private float _pitchVariance = .1f;
		public float PitchVariance => _pitchVariance;

		[SerializeField] private bool _isLoop = false;
		public bool IsLoop => _isLoop;

		[SerializeField] private AudioMixerGroup _mixerGroup;
		public AudioMixerGroup MixerGroup => _mixerGroup;

		private AudioSource _source;
		public AudioSource Source { get => _source; set { _source = value; } }
	}
}