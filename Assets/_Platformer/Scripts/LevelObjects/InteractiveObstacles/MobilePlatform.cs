///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 27/01/2020 16:48
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles {
	public class MobilePlatform : MonoBehaviour {

        [SerializeField] private Transform[] allPoints;
        private Vector2 startPos;
        private float elapsedTime;
        private uint index = 1;
        [SerializeField]private float duration;
        [SerializeField]private string playerTag = "Player";

        [SerializeField] private bool _isStarted = false;

        public bool IsStarted
        {
            get
            {
                return _isStarted;
            }
            set
            {
                _isStarted = value;
            }
        }

        private void Start()
        {
            startPos = allPoints[0].position;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("sur une plateforme mobile");
            if (collision.CompareTag(playerTag))
            {
                collision.transform.SetParent(transform);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(playerTag))
            {
                collision.transform.SetParent(null);
            }

        }



        private void Update()
        {
            if (IsStarted)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector2.Lerp(index > 0 ? allPoints[index - 1].position : allPoints[allPoints.Length - 1].position, allPoints[index].position, elapsedTime / duration);
                if (elapsedTime >= duration)
                {
                    if (index >= allPoints.Length - 1) index = 0;
                    else index++;
                    elapsedTime = 0;
                }
            }
        }

    }
}