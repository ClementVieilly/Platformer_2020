///-----------------------------------------------------------------
/// Author : Joël VOIGNIER
/// Date : 21/01/2020 10:38
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects
{
	public abstract class ALevelObject : MonoBehaviour
	{
		[SerializeField] protected SoundsSettings sounds = null;
		[HideInInspector]
		public List<Sound> sfxList = new List<Sound>();
		virtual public void Init()
		{
			throw new NotImplementedException();
		}
	}
}