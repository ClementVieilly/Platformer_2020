using System.Collections.Generic;
using UnityEngine;

public class AnimationToolkit : MonoBehaviour
{
	private Dictionary<string, ParticleSystem> _particleDictionary = new Dictionary<string, ParticleSystem>();

    private void Start()
    {
		ParticleSystem[] foundParticles = GetComponentsInChildren<ParticleSystem>(true);

		for (int i = 0; i < foundParticles.Length; i++)
		{
			if(!_particleDictionary.ContainsKey(foundParticles[i].name))
				_particleDictionary.Add(foundParticles[i].name, foundParticles[i]);
		}
    }

	public void PlayParticle(string particleName)
	{
		if (_particleDictionary.ContainsKey(particleName))
			_particleDictionary[particleName].Play();
	}
}