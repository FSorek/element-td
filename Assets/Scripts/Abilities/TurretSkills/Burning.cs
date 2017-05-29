using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning : EOT
{
    public Damages damagePerSecond;
    private GameObject PartEff;
    private EnemyStat enemyStat;
    // Use this for initialization
    public void OnApply(float _duration, float _tickFreq, Damages _DamagePerSec, Turret _attacker)
    {
        attacker = _attacker;
        duration = _duration;
        tickFrequency = _tickFreq;
        damagePerSecond = _DamagePerSec;
    }
    protected override void Initialize()
    {
        enemyStat = this.gameObject.GetComponent<EnemyStat>();
        ParticleEffect = Resources.Load("Objects/Abilities/BurnEffect", typeof(Transform)) as Transform;
    }

    protected override void OnTick(float time)
    {
        enemyStat.DamageTaken(damagePerSecond, attacker);
    }

    protected override void OnEffectStart()
    {
        if (PartEff == null)
            PartEff = Instantiate(ParticleEffect.gameObject, this.transform.position, Quaternion.identity, this.transform);

    }

    protected override void OnEffectEnd()
    {
        if (PartEff != null)
            Destroy(PartEff);
    }
}
