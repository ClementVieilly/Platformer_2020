///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 03/03/2020 15:39
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Collectibles
{
    public delegate void BigScoreCollectibleEventHandler(uint slotNumber);

	public class BigScoreCollectible : ACollectible
	{
		private static List<BigScoreCollectible> _list = new List<BigScoreCollectible>();
		public static List<BigScoreCollectible> List => _list;

		[SerializeField] private uint slotNumber = 0;
        [SerializeField] private ParticleSystem collectParticleSystem;

        public event BigScoreCollectibleEventHandler OnCollected;

		private void Awake()
		{
			_list.Add(this);
		}

		protected override void EffectOfTheCollectible()
		{
			OnCollected?.Invoke(slotNumber);
            Instantiate(collectParticleSystem, transform.position, Quaternion.identity);
        }

        public static void ResetAll()
		{
			for (int i = List.Count - 1; i >= 0; i--)
				List[i].gameObject.SetActive(true);
		}
	}
}