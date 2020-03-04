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
		/// Struct to register touch info between frames
		/// </summary>
		private class TouchInfo
		{
			public int touchIndex;

			public Vector2 position = Vector2.zero;
			public Vector2 direction = Vector2.zero;

			public Side side = Side.ABORTED;

			public TouchInfo(int index)
			{
				touchIndex = index;
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
			get { if (_mainCamera == null)
					{
					_mainCamera = Camera.main;
					}
				return _mainCamera;
			}
			set
			{
				_mainCamera = value;
			}
		}

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
				if (touches.Count != 0) touches.Clear();

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
			Touch touch;
			TouchInfo touchInfo;
			for (int i = Input.touchCount - 1; i >= 0; i--)
			{
				touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began)
				{
					touchInfo = new TouchInfo(i);
					Debug.Log(MainCamera);
					touchInfo.direction = touchInfo.position = touch.position;
					touchInfo.side = MainCamera.ScreenToViewportPoint(touch.position).x <= 0.5f ? Side.LEFT : Side.RIGHT;

					// Check if this side touch is already registered, if not adds it to touches List
					if (!CheckSideIsAlreadyRegistered(touchInfo.side))
					{
						// If touch is right side set _jump to true
						if (touchInfo.side == Side.RIGHT) _jump = true;
						touches.Add(touchInfo);
					}
				}
			}
		}

		/// <summary>
		/// Returns true if a touch same side than "side" is already registered
		/// </summary>
		/// <returns></returns>
		private bool CheckSideIsAlreadyRegistered(Side side)
		{
			for (int i = touches.Count - 1; i >= 0; i--)
			{
				if (touches[i].side == side)
					return true;
				else
					continue;
			}

			return false;
		}

		/// <summary>
		/// Removes the TouchInfo from touches List at index position and rectifies Touch indexes of others TouchInfo
		/// </summary>
		private void RemoveTouchInfoAt(int index)
		{
			TouchInfo touchInfo;
			for (int i = touches.Count - 1; i >= 0; i--)
			{
				touchInfo = touches[i];
				if (i > index) touchInfo.touchIndex--;
			}

			touches.RemoveAt(index);
		}

		/// <summary>
		/// Update touches already contained in touches List<TouchInfo>
		/// </summary>
		private void UpdateTouches()
		{
			TouchInfo touchInfo;
			for (int i = touches.Count - 1; i >= 0; i--)
			{
				touchInfo = touches[i];

				if (touchInfo.side == Side.LEFT)
					UpdateLeftTouch(i);
				else if (touchInfo.side == Side.RIGHT)
					UpdateRightTouch(i);
				else
				{
					if (Input.GetTouch(touchInfo.touchIndex).phase == TouchPhase.Ended)
						RemoveTouchInfoAt(i);
				}
			}
		}

		/// <summary>
		/// Update left side touch corresponding to the horizontal axis
		/// </summary>
		/// <param name="index">index in the touches List</param>
		private void UpdateLeftTouch(int index)
		{
			TouchInfo touchInfo = touches[index];
			Touch touch = Input.GetTouch(touchInfo.touchIndex);

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
				RemoveTouchInfoAt(index);
			}
		}

		/// <summary>
		/// Update right side touch corresponding to the jump axis
		/// </summary>
		/// <param name="index">index in the touches List</param>
		private void UpdateRightTouch(int index)
		{
			TouchInfo touchInfo = touches[index];
			Touch touch = Input.GetTouch(touchInfo.touchIndex);

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
				RemoveTouchInfoAt(index);
			}
		}
	}
}
 