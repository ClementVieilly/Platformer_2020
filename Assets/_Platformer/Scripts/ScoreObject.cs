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

		public int TotalScore { get => -completion_time + nb_score + nb_lives; }

		public int CompareTo(object other)
		{
			if (other == null) return 1;

			ScoreObject otherScoreObject = (ScoreObject)other;
			return otherScoreObject.TotalScore.CompareTo(TotalScore);
		}

		public static bool operator <(ScoreObject a, ScoreObject b)
		{
			return a.TotalScore < b.TotalScore;
		}

		public static bool operator >(ScoreObject a, ScoreObject b)
		{
			return a.TotalScore > b.TotalScore;
		}

		public static ScoreObject Worst { get => new ScoreObject(int.MaxValue, int.MinValue, int.MinValue); }
	}
}