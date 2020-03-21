///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 04/02/2020 12:15
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.LevelObjects;
using UnityEngine;

namespace Com.IsartDigital.Platformer.InteractiveObstacles {
    public class InteractiveEnemyAttack : MonoBehaviour
    {
        private Vector3 targetPosition;
        private Vector3 startPosition;
        [SerializeField] private float speed = 1f;
        [SerializeField] private float restingTime = 0.3f;
        [SerializeField] private string playerTag = "Player";

        public void Init(Vector3 startPos ,Vector3 targetPos)
        {
            targetPosition = targetPos;
            startPosition = startPos;
            transform.position = startPosition;
        }

        private void Update()
        {
           if(targetPosition != null) transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if(transform.position == targetPosition) Destroy(gameObject,restingTime); 
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(playerTag))
				collision.GetComponent<Player>().LooseLife();
        }
    }
}