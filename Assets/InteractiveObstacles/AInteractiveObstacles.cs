///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 04/02/2020 11:10
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;

namespace Com.IsartDigital.InteractiveObstacles
{
    abstract public class AInteractiveObstacles : ACollisionableObject
    {
        protected override void EffectOnCollision()
        {
            TriggerInteraction();
        }

        abstract protected void TriggerInteraction();

    }
}