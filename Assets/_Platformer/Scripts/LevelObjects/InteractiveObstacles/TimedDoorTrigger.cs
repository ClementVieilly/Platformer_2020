///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 04/02/2020 14:45
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles
{
    public class TimedDoorTrigger : AInteractiveObstacles
    {
        [SerializeField] private GameObject doorGameObject = null;
        private TimedDoor timedDoor = null;

        private void Awake()
        {
            timedDoor = doorGameObject.GetComponent<TimedDoor>();
        }

        protected override void TriggerInteraction()
        {
            timedDoor.Open();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            timedDoor.Close();
        }
    }
}