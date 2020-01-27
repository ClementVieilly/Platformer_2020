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
        private static List<Checkpoints> _list = new List<Checkpoints>();
        public static List<Checkpoints> List => _list;

        private void Awake()
        {
            _list.Add(this);
        }

        protected override void EffectOnCollision()
        {
            base.EffectOnCollision();

            //OnCollision();

            if(collidedObject.CompareTag(playerTag))
            {
                collidedObject.GetComponent<Player>().LastCheckpointPos = transform.position;
                transform.GetComponent<BoxCollider2D>().enabled = false; 
            }
        }

        private void OnDestroy()
        {
            //_list.Remove(this);
        }
    }
}