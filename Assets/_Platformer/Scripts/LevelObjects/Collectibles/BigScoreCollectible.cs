///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 03/03/2020 15:39
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Collectibles
{
    public delegate void BigScoreCollectibleEventHandler(int slotNumber);

	public class BigScoreCollectible : ACollectible
	{
		private static List<BigScoreCollectible> _list = new List<BigScoreCollectible>();
		public static List<BigScoreCollectible> List => _list;

		[SerializeField] private int slotNumber = 0;
        [SerializeField] private ParticleSystem collectParticleSystem = null;

        [SerializeField] private int currentLvlNumber = 1;
        private string soundToPlay = null;

        public event BigScoreCollectibleEventHandler OnCollected;

		private void Awake()
		{
			_list.Add(this);
            SetCurrentLevel(currentLvlNumber);

        }

		protected override void EffectOfTheCollectible()
		{
			OnCollected?.Invoke(slotNumber);
            Instantiate(collectParticleSystem, transform.position, Quaternion.identity);
            SoundManager.Instance.Play(soundToPlay);
        }

        private void SetCurrentLevel(int lvlNumber)
        {
            currentLvlNumber = lvlNumber;
            if (currentLvlNumber == 1) soundToPlay = sounds.Collectible_BigScore_Lvl1;
            if (currentLvlNumber == 2) soundToPlay = sounds.Collectible_BigScore_Lvl2;
        }

        public static void ResetAll()
        {
            for(int i = List.Count - 1; i >= 0; i--)
                List[i].ResetObject();
        }


        private void OnDestroy()
        {
            _list.Remove(this); 
        }
    }
}