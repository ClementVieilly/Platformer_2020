///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 27/01/2020 16:35
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
	public class Level : MonoBehaviour {

		[SerializeField] private List<Checkpoints> checkpoints = new List<Checkpoints>();

		private void Start () {
			InitCheckpoints();
		}
		
		private void InitCheckpoints()
		{
			Debug.Log("Start Checkpoints");
			Debug.Log(checkpoints.Count);
		}
	}
}