///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 19/02/2020 10:32
///-----------------------------------------------------------------

using System;

namespace Com.IsartDigital.Platformer
{
	[Serializable]
	public class ScoreObject
	{
		public string username = "default";
		public int completion_time = 0;
		public int nb_score = 0;
		public int nb_lives = 0;

		public ScoreObject(int completionTime, int nbScore, int nbLives)
		{
			completion_time = completionTime;
			nb_score = nbScore;
			nb_lives = nbLives;
		}
	}
}