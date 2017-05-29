using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk : EffectOnHit
{

    public float speedMultiplier;
    private GameObject PartEff;
    private Turret turret;
    // Use this for initialization
    public void OnApply(float _speedMultiplier, float _duration, Turret _turret)
    {
        duration = _duration;
        turret = _turret;
        speedMultiplier = _speedMultiplier;
    }
    protected override void Initialize()
    {
    }

    protected override void OnEffectStart()
    {
        turret.berserkSpeedBonus = speedMultiplier;
        turret.CalculateFinalDamageValues();
    }

    protected override void OnEffectEnd()
    {
        turret.berserkSpeedBonus = 0;
        turret.CalculateFinalDamageValues();
    }
}
