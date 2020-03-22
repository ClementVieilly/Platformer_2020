///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/03/2020 06:04
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.InteractiveObstacles;
using UnityEngine;

namespace Com.IsartDigital.InteractiveObstacles {
	public class SpiderTrigger : AInteractiveObstacles
    {

        [SerializeField] private TimedSpider spider = null;


        protected override void TriggerInteraction()
        {
            spider.Open();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            spider.Close();
        }
    }
}