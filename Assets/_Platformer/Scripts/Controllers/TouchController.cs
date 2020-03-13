///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 12:07
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.Controllers
{
	public class TouchController : AController
	{
		/// <summary>
		/// Side on the viewport of the touch
		/// </summary>
		private enum Side
		{
			/// <summary>
			/// Touch right side was aborted
			/// </summary>
			ABORTED,
			LEFT,
			RIGHT
		}

		/// <summary>
		/// Class to register touch info between frames
		/// </summary>
		private class TouchInfo
		{
			public int fingerId;

			public Vector2 position = Vector2.zero;
			public Vector2 direction = Vector2.zero;

			public Side side = Side.ABORTED;

			public TouchInfo(int index)
			{
				fingerId = index;
			}
		}

		private float _horizontalAxis = 0f;
		private bool _jump = false;

		public override float HorizontalAxis { get => _horizontalAxis; }
		public override bool Jump { get => _jump; }

		private const float HORIZONTAL_THRESHOLD = 10f;

		private Camera _mainCamera = null;
		private Camera MainCamera
		{
			get
			{
				if (_mainCamera == null)
					_mainCamera = Camera.main;

				return _mainCamera;
			}
			set
			{
				_mainCamera = value;
			}
		}

		private const int NOT_REGISTERED_INDEX = -1;
		private int leftTouchIndex = NOT_REGISTERED_INDEX;
		private int rightTouchIndex = NOT_REGISTERED_INDEX;

		private List<TouchInfo> touches = new List<TouchInfo>();

		/// <summary>
		/// Initialize controller
		/// </summary>
		public override void Init()
		{
			MainCamera = Camera.main;
		}

		/// <summary>
		/// Update controller inputs
		/// </summary>
		public override void Update()
		{
			if (Input.touchCount == 0)
			{
				_horizontalAxis = 0f;
				_jump = false;

				return;
			}

			RegisterTouches();

			UpdateTouches();
		}

		/// <summary>
		/// Register new touches in touches List<TouchInfo>
		/// </summary>
		private void RegisterTouches()
		{
			TouchInfo touchInfo;
			foreach (Touch touch in Input.touches)
			{
				if (touch.phase == TouchPhase.Began)
				{
					touchInfo = new TouchInfo(touch.fingerId);
					touchInfo.direction = touchInfo.position = touch.position;
					touchInfo.side = MainCamera.ScreenToViewportPoint(touch.position).x <= 0.5f ? Side.LEFT : Side.RIGHT;

					// Check if this side touch is already registered, if not set the corresponded side index
					if (touchInfo.side == Side.RIGHT && rightTouchIndex == NOT_REGISTERED_INDEX)
					{
						rightTouchIndex = touchInfo.fingerId;
						_jump = true;
					}
					else if (touchInfo.side == Side.LEFT && leftTouchIndex == NOT_REGISTERED_INDEX)
						leftTouchIndex = touchInfo.fingerId;

					AddInTouches(touchInfo);
				}
			}
		}

		private void AddInTouches(TouchInfo touchInfo)
		{
			int diff = touchInfo.fingerId - touches.Count;

			if (diff >= 0)
			{
				for (int i = diff; i >= 0; i--)
					touches.Add(null);
			}

			touches[touchInfo.fingerId] = touchInfo;
		}

		/// <summary>
		/// Update touches
		/// </summary>
		private void UpdateTouches()
		{
			foreach (Touch touch in Input.touches)
			{
				if (leftTouchIndex != NOT_REGISTERED_INDEX && leftTouchIndex == touch.fingerId) UpdateLeftTouch(touch);
				else if (rightTouchIndex != NOT_REGISTERED_INDEX && rightTouchIndex == touch.fingerId) UpdateRightTouch(touch);
			}
		}

		/// <summary>
		/// Update left side touch corresponding to the horizontal axis
		/// </summary>
		private void UpdateLeftTouch(Touch touch)
		{
			TouchInfo touchInfo = touches[touch.fingerId];

			if (touch.phase == TouchPhase.Moved)
			{
				touchInfo.direction = touch.position - touchInfo.position;
				touchInfo.position = touch.position;

				if (Mathf.Abs(touchInfo.direction.x) < HORIZONTAL_THRESHOLD)
					return;

				_horizontalAxis = touchInfo.direction.x < 0f ? -1f : 1f;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				_horizontalAxis = 0f;
				leftTouchIndex = NOT_REGISTERED_INDEX;
				touches[touch.fingerId] = null;
			}
		}

		/// <summary>
		/// Update right side touch corresponding to the jump axis
		/// </summary>
		private void UpdateRightTouch(Touch touch)
		{
			TouchInfo touchInfo = touches[touch.fingerId];

			if (touch.phase == TouchPhase.Moved)
			{
				// If the touch leaves right side of the screen it's considered as aborted
				// so a new touch can be registered
				if (MainCamera.ScreenToViewportPoint(touch.position).x <= 0.5f)
				{
					_jump = false;
					touchInfo.side = Side.ABORTED;
				}
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				_jump = false;
				rightTouchIndex = NOT_REGISTERED_INDEX;
				touches[touch.fingerId] = null;
			}
		}
	}
}
