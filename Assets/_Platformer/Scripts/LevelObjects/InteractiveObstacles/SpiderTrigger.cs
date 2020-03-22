///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/03/2020 06:04
///-----------------------------------------------------------------

using Com.IsartDigital.InteractiveObstacles;
using Com.IsartDigital.Platformer.InteractiveObstacles;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles {
	public class SpiderTrigger : AInteractiveObstacles
    {

        [SerializeField] private TimedSpider spider = null;
        [SerializeField] private List<Transform> cocoonsList = new List<Transform>();

        private Vector2[] startPos;
        private bool test = false; 
        private void Awake()
        {
            startPos = new Vector2[] { cocoonsList[0].position, cocoonsList[1].position }; 
        }
        protected override void TriggerInteraction()
        {
            test = true; 
            spider.Open();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            test = false; 
            spider.Close();
        }

        private void Update()
        {
            if(!test) return;
            else ShakeCocoons(); 
        }

        private void ShakeCocoons()
        {
            float x; 
            float y; 
            for(int i = cocoonsList.Count - 1; i >= 0; i--)
            {
                 x = Random.Range(-1f, 1f) * 0.06f;
                 y = Random.Range(-1f, 1f) * 0.06f;
                 cocoonsList[i].position = new Vector2(x, y) + startPos[i] ;  
            }
        }
    }
}