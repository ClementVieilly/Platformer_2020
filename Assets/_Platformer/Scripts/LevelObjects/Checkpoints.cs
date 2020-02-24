///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 24/01/2020 11:35
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
	public class Checkpoints : ACollisionableObject {

        public Action<Checkpoints> OnCollision;

        [SerializeField] private bool _isFinalCheckPoint = false;
        public bool IsFinalCheckPoint => _isFinalCheckPoint;

        protected override void EffectOnCollision()
        {
            OnCollision?.Invoke(this);
            GetComponent<Collider2D>().enabled = false; 
            GetComponentInChildren<Renderer>().enabled = false; 
        }

        public void ResetCollider()
        {
            GetComponent<Collider2D>().enabled = true;
            GetComponentInChildren<Renderer>().enabled = true;
        }
    }
}