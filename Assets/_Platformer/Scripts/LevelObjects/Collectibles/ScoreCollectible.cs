///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 28/01/2020 10:35
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Platformer.LevelObjects.Collectibles {
    public delegate void ScoreCollectibleEventHandler(float score);

    public class ScoreCollectible : ACollectible {
        private static List<ScoreCollectible> _list = new List<ScoreCollectible>();
        public static List<ScoreCollectible> List => _list;

        public event ScoreCollectibleEventHandler OnCollected;

        [SerializeField] private float score = 10;

        private void Awake()
        {
            _list.Add(this); 
        }
        protected override void EffectOfTheCollectible()
        {
            OnCollected?.Invoke(score); 
        }
    }
}