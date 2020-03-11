///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 11/03/2020 14:19
///-----------------------------------------------------------------

using Com.IsartDigital.Common.UnityEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Com.IsartDigital.Common
{
	[RequireComponent(typeof(Collider2D))]
	public class Trigger2D : MonoBehaviour
	{
		[SerializeField] new Collider2D collider = null;

		[SerializeField] private Trigger2DUnityEvent _onStay = null;
		[SerializeField] private Trigger2DUnityEvent _onEnter = null;
		[SerializeField] private Trigger2DUnityEvent _onExit = null;

		public event UnityAction<Collider2D> OnEnter
		{
			add { _onEnter.AddListener(value); }
			remove { _onEnter.RemoveListener(value); }
		}

		public event UnityAction<Collider2D> OnStay
		{
			add { _onStay.AddListener(value); }
			remove { _onStay.AddListener(value); }
		}

		public event UnityAction<Collider2D> OnExit
		{
			add { _onExit.AddListener(value); }
			remove { _onExit.RemoveListener(value); }
		}

		/// <summary>
		/// Register collider if not registered and force it to be a trigger
		/// </summary>
		private void OnValidate()
		{
			if (!collider)
				collider = GetComponent<Collider2D>();

			collider.isTrigger = true;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			_onEnter?.Invoke(other);
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			_onStay?.Invoke(other);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			_onExit?.Invoke(other);
		}
	}
}