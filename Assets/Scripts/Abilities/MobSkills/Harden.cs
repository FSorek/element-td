using UnityEngine;

public class Harden : OnSpawn
{
    public float castDelay;
    public float duration;
    public float repeatTimer;
    [Range(0, 1f)]
    public float damageReduction;

    private EnemyStat enemyStat;

    public void Initialise(float _castDelay, float _duration, float _repeatTimer, float _damageReduction)
    {
        castDelay = _castDelay;
        duration = _duration;
        repeatTimer = _repeatTimer;
        damageReduction = _damageReduction;
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
        enemyStat.HardenModifier = damageReduction;
        Invoke("AbilityEnd", duration);
    }

    private void AbilityEnd()
    {
        enemyStat.HardenModifier = 0;
    }
}
