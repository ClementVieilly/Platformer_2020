///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 27/01/2020 16:35
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
	public class Level : MonoBehaviour {

		[SerializeField] private List<Checkpoint> checkpoints = new List<Checkpoint>();
        [SerializeField] private Vector2 _startPos = Vector2.zero;
		public List<Checkpoint> CheckpointsList => checkpoints;
		public Vector2 StartPos => _startPos;

	}
}