///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 04/02/2020 12:15
///-----------------------------------------------------------------

using Com.IsartDigital.Platformer.Managers;
using Com.IsartDigital.Platformer.Screens;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.InteractiveObstacles {
    public class InteractiveEnemyAttack : MonoBehaviour
    {
        private Vector3 targetPosition;
        private Vector3 startPosition;
        [SerializeField] private float speed = 1f;
        [SerializeField] private string playerTag = "Player";


        public void Init(Vector3 startPos ,Vector3 targetPos)
        {
            targetPosition = targetPos;
            startPosition = startPos;
            transform.position = startPosition;
            Destroy(gameObject, 2);
        }

        private void Update()
        {
           if(targetPosition != null) transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(playerTag))
            {
                if (collision.GetComponent<Player>().LooseLife())
                {
                    collision.GetComponent<Player>().setPosition(CheckpointManager.Instance.LastCheckpointPos);
                    Hud.Instance.Life = collision.GetComponent<Player>().Life;
                }
            }
        }


    }
}