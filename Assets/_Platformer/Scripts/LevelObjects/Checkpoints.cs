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

        [SerializeField] private bool _isSuperCheckpoint = false;
        public bool IsSuperCheckpoint => _isSuperCheckpoint;

        protected override void EffectOnCollision()
        {
            OnCollision?.Invoke(this);
            gameObject.SetActive(false);
        }

        public void ResetCollider()
        {
            gameObject.SetActive(true);
        }
    }
}