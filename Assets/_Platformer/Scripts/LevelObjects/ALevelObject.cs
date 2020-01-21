///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:38
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects
{
	public abstract class ALevelObject : MonoBehaviour
	{
		virtual public void Init()
		{
			throw new NotImplementedException();
		}
	}
}