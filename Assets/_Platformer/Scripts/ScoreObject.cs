///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 19/02/2020 10:32
///-----------------------------------------------------------------

using System;

namespace Com.IsartDigital.Platformer
{
	[Serializable]
	public class ScoreObject : IComparable
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

		public int CompareTo(object other)
		{
			if (other == null) return 1;

			ScoreObject otherScoreObject = (ScoreObject)other;
			int totalScore = -completion_time + nb_score + nb_lives;
			int otherTotalScore = -otherScoreObject.completion_time + otherScoreObject.nb_score + otherScoreObject.nb_lives;

			return otherTotalScore.CompareTo(totalScore);
		}
	}
}