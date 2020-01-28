///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 24/01/2020 10:42
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
    public class KillZone : ACollisionableObject {

        private static List<KillZone> _list = new List<KillZone>();
        public static List<KillZone> List => _list;
        public Action OnCollision;

        private void Awake()
        {
            _list.Add(this);
        }
        protected override void EffectOnCollision()
        {
            base.EffectOnCollision();

            OnCollision?.Invoke();
        }

        private void OnDestroy()
        {
            _list.Remove(this);
        }
    }
}