using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : EOT
{
    public int damagePerSecond;
    private GameObject PartEff;
    private EnemyStat enemyStat;
    // Use this for initialization
    public void OnApply(float _duration, float _tickFreq, int _DamagePerSec, Turret _attacker)
    {
        attacker = _attacker;
        duration = _duration;
        tickFrequency = _tickFreq;
        damagePerSecond = _DamagePerSec;
    }
    protected override void Initialize()
    {
        enemyStat = this.gameObject.GetComponent<EnemyStat>();
        ParticleEffect = Resources.Load("Objects/Abilities/BleedEffect", typeof(Transform)) as Transform;
    }

    protected override void OnTick(float time)
    {
        enemyStat.DirectMagicDamageTaken(damagePerSecond, attacker);
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
