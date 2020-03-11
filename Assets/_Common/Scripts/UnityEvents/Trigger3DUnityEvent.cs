///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 11/03/2020 14:13
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Com.IsartDigital.Common.UnityEvents
{
	[Serializable] public class Trigger3DUnityEvent : UnityEvent<Collider> { }
}