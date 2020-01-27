///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 27/01/2020 15:32
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers {
	public class CheckpointManager : MonoBehaviour {

		private List<Checkpoints> checkpointList = new List<Checkpoints>();

		private Vector2 _lastCheckpointPos;
		public Vector2 LastCheckpointPos { get => _lastCheckpointPos; set => _lastCheckpointPos = value; }

		private Vector2 _lastSuperCheckpointPos;
		public Vector2 LastSuperCheckpointPos { get => _lastSuperCheckpointPos; set => _lastSuperCheckpointPos = value; }

		private void Start()
		{
			
		}

		private void setCheckpoint(Checkpoints triggredCheckpoint)
		{
			_lastCheckpointPos = triggredCheckpoint.transform.position;
			if (triggredCheckpoint.IsSuperCheckpoint) _lastSuperCheckpointPos = triggredCheckpoint.transform.position;
		}
	}
}