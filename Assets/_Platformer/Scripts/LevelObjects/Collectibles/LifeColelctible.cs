///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 27/01/2020 12:24
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Collectibles {
	public class LifeColelctible : ACollectible {

		[SerializeField] private int WinLife = 1;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			Debug.Log("life +" + WinLife);
			
		}

	}
}