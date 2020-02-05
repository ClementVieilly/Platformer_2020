///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 04/02/2020 11:10
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles {
    abstract public class AInteractiveObstacles : ACollisionableObject
    {
        protected override void EffectOnCollision()
        {
            TriggerInteraction();
        }

        abstract protected void TriggerInteraction();

    }
}