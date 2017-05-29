using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unstable : OnSpawn
{
    public float weaknessDuration;
    [Range(0,1f)]
    public float weaknessDamageIncrease;

    [Space(10)]

    public float hardenDuration;
    [Range(0, 1f)]
    public float hardenDamageReduction;

    public override void Activate()
    {
        gameObject.AddComponent<Weakness>().Initialise(hardenDuration, weaknessDuration, hardenDuration + weaknessDuration, weaknessDamageIncrease);
        gameObject.AddComponent<Harden>().Initialise(0f, hardenDuration, hardenDuration + weaknessDuration, hardenDamageReduction);
    }
}
