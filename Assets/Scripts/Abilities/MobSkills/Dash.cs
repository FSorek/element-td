using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : OnSpawn
{
    public float castDelay;
    public float duration;
    [Range(1,10f)]
    public float speedMultiplier;

    private EnemyStat enemyStat;
    
    private void Start()
    {
        enemyStat = GetComponent<EnemyStat>();
    }
    public override void Activate()
    {
        Invoke("AbilityStart", castDelay);
    }

    private void AbilityStart()
    {
        enemyStat.DashBonus = speedMultiplier;
        enemyStat.UpdateMovement();
        Invoke("AbilityEnd", duration);
    }

    private void AbilityEnd()
    {
        enemyStat.DashBonus = 1;
        enemyStat.UpdateMovement();
    }
}
