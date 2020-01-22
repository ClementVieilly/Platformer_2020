///-----------------------------------------------------------------
/// Author : Maximilien SADI KORICHENE
/// Date : 21/01/2020 12:07
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Platformer.Controllers
{
	public class TouchController : AController
	{
		private float _horizontalAxis = 0f;
		private bool _jump = false;

		public override float HorizontalAxis { get => _horizontalAxis; }
		public override bool Jump { get => _jump; }

		private Camera mainCamera = null;

		private Vector2 startPosition = Vector2.zero;
		private Vector2 position = Vector2.zero;
		private Vector2 direction = Vector2.zero;

		/// <summary>
		/// Initialize controller
		/// </summary>
		public override void Init()
		{
			mainCamera = Camera.main;
		}

		/// <summary>
		/// Update controller inputs
		/// </summary>
		public override void Update()
		{
			if (Input.touchCount == 0) return;

			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				direction = position = touch.position;
				startPosition = mainCamera.ScreenToViewportPoint(position);
				Debug.Log("//////////////////////// start position is : " + startPosition);
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				direction = touch.position - position;
				position = touch.position;

				_horizontalAxis = direction.x < 0f ? -1f : 1f;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				_horizontalAxis = 0f;
			}
		}
	}
}