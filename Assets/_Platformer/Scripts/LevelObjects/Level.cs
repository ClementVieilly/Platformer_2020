///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 27/01/2020 16:35
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
	public class Level : MonoBehaviour {

		[SerializeField] private List<Checkpoints> checkpoints = new List<Checkpoints>();
		public List<Checkpoints> CheckpointsList => checkpoints;

	}
}