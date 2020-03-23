///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 23/03/2020 12:50
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Sounds {
	public class SoundEmission : ALevelObject
	{
		[SerializeField] ParticleType emitParticle;

		private string soundToPlay = null;

	    void Start()
	    {
			switch (emitParticle)
			{
				case ParticleType.NONE:
					soundToPlay = null;
					break;
				case ParticleType.Sparks:
					soundToPlay = sounds.Env_electric_sparks;
					break;
				case ParticleType.ElectricityArc:
					soundToPlay = sounds.Env_electric_arc;
					break;
				default:
					break;
			}
		}
	
	    void Update()
	    {
	        
	    }
	}

	public enum ParticleType
	{
		NONE,
		Sparks,
		ElectricityArc,
	}
}