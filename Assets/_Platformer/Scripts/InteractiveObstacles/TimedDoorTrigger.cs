///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 04/02/2020 14:45
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.InteractiveObstacles {
    public class TimedDoorTrigger : AInteractiveObstacles
    {
        [SerializeField] private GameObject doorGameObject = null;
        private TimedDoor timedDoor = null;

		[SerializeField] SpriteRenderer gfx = null;
		[SerializeField] Sprite spriteIsOn = null;
		[SerializeField] Sprite spriteIsOff = null;

        private void Awake()
        {
            timedDoor = doorGameObject.GetComponent<TimedDoor>();
        }

        protected override void TriggerInteraction()
        {
            timedDoor.Open();
			gfx.sprite = spriteIsOn;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            timedDoor.Close();
			gfx.sprite = spriteIsOff;
		}
	}
}