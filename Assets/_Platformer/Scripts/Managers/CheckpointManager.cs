///-----------------------------------------------------------------
/// Author : Alexandre RAUMEL
/// Date : 27/01/2020 15:32
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Managers {
    public delegate void CheckpointManagerEventHandler(); 
	public class CheckpointManager : MonoBehaviour {

		//Singleton
		private static CheckpointManager _instance;
		public static CheckpointManager Instance => _instance;

		//Checkpoints positions
		[SerializeField] private Level currentLevel = null;
		private List<Checkpoint> checkpointList = new List<Checkpoint>();
		private Vector2 _lastCheckpointPos = Vector2.zero;
		public Vector2 LastCheckpointPos { get => _lastCheckpointPos; set => _lastCheckpointPos = value; }
		private Vector2 _lastSuperCheckpointPos = Vector2.zero;

        //event de Victoire 
        public static event CheckpointManagerEventHandler OnFinalCheckPointTriggered;
		private void Start()
		{
			if (_instance)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this;
			checkpointList = currentLevel.CheckpointsList;
			
            for(int i = checkpointList.Count - 1; i >= 0; i--)
                checkpointList[i].OnCollision += SetCheckpoint; 
		}

        private void SetCheckpoint(Checkpoint triggredCheckpoint)
        { 
            _lastCheckpointPos = triggredCheckpoint.transform.position;
			if (triggredCheckpoint.IsFinalCheckPoint)
                OnFinalCheckPointTriggered?.Invoke();
        }

		public void Reset()
		{
			for (int i = checkpointList.Count - 1; i >= 0; i--)
				checkpointList[i].Reset();
		}

		private void OnDestroy()
		{
			if (this == _instance) _instance = null;
		}
	}
}