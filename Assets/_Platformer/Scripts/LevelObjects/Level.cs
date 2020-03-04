///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 27/01/2020 16:35
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
	public class Level : MonoBehaviour {

		[SerializeField] private List<Checkpoints> checkpoints = new List<Checkpoints>();
        [SerializeField] private Vector2 _startPos; 
		public List<Checkpoints> CheckpointsList => checkpoints;
		public Vector2 StartPos => _startPos;

	}
}