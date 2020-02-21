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
		[SerializeField] private Level currentLevel;
		private List<Checkpoints> checkpointList = new List<Checkpoints>();
		private Vector2 _lastCheckpointPos;
		public Vector2 LastCheckpointPos { get => _lastCheckpointPos; set => _lastCheckpointPos = value; }
		private Vector2 _lastSuperCheckpointPos;

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
            {
                checkpointList[i].OnCollision += SetCheckpoint; 
            }
		}

        private void SetCheckpoint(Checkpoints triggredCheckpoint)
        { 
            _lastCheckpointPos = triggredCheckpoint.transform.position;
			if (triggredCheckpoint.IsFinalCheckPoint)
                OnFinalCheckPointTriggered?.Invoke();
        }

		public void ResetColliders()
		{
			for (int i = checkpointList.Count - 1; i >= 0; i--)
				checkpointList[i].ResetCollider();
		}

		private void OnDestroy()
		{
			if (this == _instance) _instance = null;
		}
	}
}