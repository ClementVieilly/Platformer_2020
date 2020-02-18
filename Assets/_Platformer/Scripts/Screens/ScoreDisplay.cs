///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 18/02/2020 17:22
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Platformer.Screens
{
	public class ScoreDisplay : MonoBehaviour
	{
		[SerializeField] private Text username = null;
		[SerializeField] private Text time = null;
		[SerializeField] private Text score = null;
		[SerializeField] private Text lives = null;

		public string Username { set { username.text = value; } }
		public string Time { set { time.text = value; } }
		public string Score { set { score.text = value; } }
		public string Lives { set { lives.text = value; } }
	}
}