///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 27/01/2020 16:48
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Platforms.PlatformsMovements {
	public class MoveBetweenPoints : MonoBehaviour {

        [SerializeField] private Transform[] allPoints;
        private Vector2 startPos;
        private float elapsedTime;
        private uint index = 1;
        [SerializeField]private float duration;

        private void Start()
        {
            startPos = allPoints[0].position;
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector2.Lerp(index > 0 ? allPoints[index - 1].position : allPoints[allPoints.Length-1].position, allPoints[index].position, elapsedTime / duration);
            if(elapsedTime >= duration)
            {
                if (index >= allPoints.Length - 1) index = 0;
                else index++;
                elapsedTime = 0;
            }
        }

    }
}