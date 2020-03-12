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

		[SerializeField] private int score = 1;

		private void Awake()
		{
			_list.Add(this);
		}

		protected override void EffectOfTheCollectible()
		{
			OnCollected?.Invoke(score);
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