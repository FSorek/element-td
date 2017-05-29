using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : OnSpawn {
    public float castDelay;
    public float duration;
    public float repeatTimer;
    [Range(0, 1f)]
    public float damageIncrease;

    private EnemyStat enemyStat;

    public void Initialise(float _castDelay, float _duration, float _repeatTimer, float _damageIncrease)
    {
        castDelay = _castDelay;
        duration = _duration;
        repeatTimer = _repeatTimer;
        damageIncrease = _damageIncrease;
    }

    private void Start()
    {
        enemyStat = GetComponent<EnemyStat>();

    }
    public override void Activate()
    {
        InvokeRepeating("AbilityStart", castDelay, repeatTimer);
    }

    private void AbilityStart()
    {
        enemyStat.HardenModifier = damageIncrease + 1;
        Invoke("AbilityEnd", duration);
    }

    private void AbilityEnd()
    {
        enemyStat.HardenModifier = 0;
    }
}
