///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 11/03/2020 14:19
///-----------------------------------------------------------------

using Com.IsartDigital.Common.UnityEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Com.IsartDigital.Common
{
	[RequireComponent(typeof(Collider))]
	public class Trigger3D : MonoBehaviour
	{
		[SerializeField] new Collider collider = null;

		[SerializeField] private Trigger3DUnityEvent _onEnter = null;
		[SerializeField] private Trigger3DUnityEvent _onStay = null;
		[SerializeField] private Trigger3DUnityEvent _onExit = null;

		public event UnityAction<Collider> OnEnter
		{
			add { _onEnter.AddListener(value); }
			remove { _onEnter.RemoveListener(value); }
		}

		public event UnityAction<Collider> OnStay
		{
			add { _onStay.AddListener(value); }
			remove { _onStay.AddListener(value); }
		}

		public event UnityAction<Collider> OnExit
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
				collider = GetComponent<Collider>();

			collider.isTrigger = true;
		}

		private void OnTriggerEnter(Collider other)
		{
			_onEnter?.Invoke(other);
		}

		private void OnTriggerStay(Collider other)
		{
			_onStay?.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			_onExit?.Invoke(other);
		}
	}
}