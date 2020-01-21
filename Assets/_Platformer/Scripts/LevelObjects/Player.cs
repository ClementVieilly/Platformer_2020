///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:38
///-----------------------------------------------------------------

using Cinemachine;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects
{
    public class Player : MonoBehaviour
    {
        //Temporary : test for Cinemachine parameters
        [SerializeField] private float speed = 5;
        [SerializeField] private CinemachineVirtualCamera currentCamera;
        [SerializeField] private Transform otherTransform = null;

        private void Start()
        {

        }

        private void Update()
        {
            TestCinemachine();
        }

        //Temporary : movement for testing cinemachine
        private void TestCinemachine()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position -= new Vector3(1, 0, 0) * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0, 1, 0) * speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position -= new Vector3(0, 1, 0) * speed * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentCamera.Follow == this.transform)
                {
                    currentCamera.Follow = otherTransform;
                }
                else
                {
                    currentCamera.Follow = this.transform;
                }
            }
        }
    }
}