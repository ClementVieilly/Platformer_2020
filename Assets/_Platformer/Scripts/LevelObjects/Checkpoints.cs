///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 24/01/2020 11:35
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
    public delegate void CheckpointEvent();
	public class Checkpoints : ACollisionableObject {

        public event CheckpointEvent OnCollision;

        [SerializeField] private bool _isSuperCheckpoint = false;
        public bool IsSuperCheckpoint => _isSuperCheckpoint;

        protected override void EffectOnCollision()
        {
            base.EffectOnCollision();

            if (OnCollision != null) OnCollision();

            if(collidedObject.CompareTag(playerTag))
            {
                collidedObject.GetComponent<Player>().LastCheckpointPos = transform.position;
                transform.GetComponent<BoxCollider2D>().enabled = false; 
            }
        }
    }
}