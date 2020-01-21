///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 15/01/2020 16:05
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Common.ScriptableObjects
{
	[CreateAssetMenu(menuName = "Common/Animator Parameter")]
	public class AnimatorParameter : ScriptableObject
	{
		[SerializeField] private string parameterName = "defaultParam";

		private int _parameterID;
		public int ParameterID => _parameterID;
		public static implicit operator int(AnimatorParameter animatorParameter) => animatorParameter.ParameterID;

		private void OnEnable()
		{
			_parameterID = Animator.StringToHash(parameterName);
		}
	}
}