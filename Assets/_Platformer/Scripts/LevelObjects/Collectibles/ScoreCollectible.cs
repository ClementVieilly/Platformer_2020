///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 28/01/2020 10:35
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Collectibles
{
	public delegate void ScoreCollectibleEventHandler(int score);

	public class ScoreCollectible : ACollectible
	{
		private static List<ScoreCollectible> _list = new List<ScoreCollectible>();
		public static List<ScoreCollectible> List => _list;

		public event ScoreCollectibleEventHandler OnCollected;

        [SerializeField] private ParticleSystem collectParticleSystem = null;

        [SerializeField] private List<Sprite> allSprites = new List<Sprite>();

		[SerializeField] private int score = 1;

		private void Awake()
		{
			_list.Add(this);
            GetComponentInChildren<SpriteRenderer>().sprite = allSprites[Random.Range(0, allSprites.Count -1)];
		}

		protected override void EffectOfTheCollectible()
		{
			OnCollected?.Invoke(score);
            Instantiate(collectParticleSystem,transform.position, Quaternion.identity);
		}

		public static void ResetAll()
		{
			for (int i = List.Count - 1; i >= 0; i--)
				List[i].gameObject.SetActive(true);
		}

		private void OnDestroy()
		{
			_list.Remove(this);
		}
	}
}