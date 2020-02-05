using Com.IsartDigital.Platformer.LevelObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinFlag : ACollisionableObject
{
    public Action OnCollision;

    protected override void EffectOnCollision()
    {
        OnCollision?.Invoke();
    }
}
