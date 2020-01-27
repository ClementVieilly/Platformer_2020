///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 24/01/2020 11:10
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects {
    [RequireComponent(typeof(Collider2D))]

   abstract public class ACollisionableObject : MonoBehaviour {
        protected Collider2D collidedObject;
        protected string playerTag = "Player"; 
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collidedObject = collision;
            EffectOnCollision();
        }

       virtual protected void EffectOnCollision()
        {
            Debug.Log("Je viens de collisioner avec : " + collidedObject.name);
        }
    }
}