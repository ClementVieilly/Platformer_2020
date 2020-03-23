///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 22/03/2020 23:28
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer
{
	[RequireComponent(typeof(Image))]
	public class LoadingWheel : MonoBehaviour
	{
		private Image wheel = null;

		[SerializeField] private float duration = 1f;
		private float elapsedTime = 0f;

		[SerializeField] private AnimationCurve animationCurve = null;

		private void Start()
		{
			Init();
		}

		public void Init()
		{
			wheel = GetComponent<Image>();
			wheel.type = Image.Type.Filled;
			wheel.fillMethod = Image.FillMethod.Radial360;
			wheel.fillClockwise = true;
			wheel.fillAmount = 1f;

			elapsedTime = 0f;
		}

		private void Update()
		{
			elapsedTime += Time.deltaTime;

			if (wheel.fillClockwise)
				wheel.fillAmount = Mathf.Lerp(1f, 0f, animationCurve.Evaluate(Mathf.Clamp01(elapsedTime / duration)));
			else
				wheel.fillAmount = Mathf.Lerp(0f, 1f, animationCurve.Evaluate(Mathf.Clamp01(elapsedTime / duration)));

			if (elapsedTime >= duration)
			{
				elapsedTime = 0f;
				wheel.fillClockwise = !wheel.fillClockwise;
			}
		}
	}
}