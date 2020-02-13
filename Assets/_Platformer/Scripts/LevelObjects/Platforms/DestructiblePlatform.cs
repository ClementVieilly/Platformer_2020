///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:40
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Platforms {
	public class DestructiblePlatform : Platform {

        private static List<DestructiblePlatform> _list = new List<DestructiblePlatform>();
        public static List<DestructiblePlatform> List => _list;

        private bool startCount = false;
        [SerializeField] private float duration;
        [SerializeField] private GameObject triggeredCollider;
        private float elapsedTime;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            startCount = true;
            Debug.Log("je suis dessus"); 
        }

        private void Update()
        {
            if (startCount)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime > duration) triggeredCollider.SetActive(false);
            }
        }

        public static void ResetAll()
        {
            for (int i = List.Count - 1; i >= 0; i--)
            {
                List[i].triggeredCollider.SetActive(true);
            }
        }

    }
}