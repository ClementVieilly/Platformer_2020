///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 04/02/2020 11:37
///-----------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace Com.IsartDigital.Platformer.InteractiveObstacles {
    public class InteractiveEnemy : AInteractiveObstacles
    {
        private bool isTriggered = false;
        [SerializeField] private float detectionDuration = 2f;
        private float elapsedTime;
        [SerializeField] private GameObject interactiveEnemyAttackPrefab = null;
        private InteractiveEnemyAttack interactiveEnemyAttack = null;

        protected override void TriggerInteraction()
        {
            isTriggered = true;
            StartCoroutine(DetectionCoroutine());
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            isTriggered = false;
        }

        private IEnumerator DetectionCoroutine()
        {

            while (isTriggered && elapsedTime < detectionDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (elapsedTime >= detectionDuration)
            {
                Attack(collidedObject.transform.position);
            }

            elapsedTime = 0;
        }

        private void Attack(Vector3 targetPos)
        {
            interactiveEnemyAttack = Instantiate(interactiveEnemyAttackPrefab).GetComponent<InteractiveEnemyAttack>();
            interactiveEnemyAttack.Init(transform.position, targetPos);
            interactiveEnemyAttack.gameObject.SetActive(true);
        }


    }
}