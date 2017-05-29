using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : EOT {
    public int Damage;
    private float RegenReduction;
    private GameObject PartEff;
    private EnemyStat enemyStat;
	// Use this for initialization
    public void OnApply(float _duration, float _tickFreq, float _regenReduction, int _Damage, Turret _attacker)
    {
        attacker = _attacker;
        duration = _duration;
        tickFrequency = _tickFreq;
        Damage = _Damage;
        RegenReduction = _regenReduction;
    }
	protected override void Initialize () {
        enemyStat = this.gameObject.GetComponent<EnemyStat>();
        ParticleEffect = Resources.Load("Objects/Abilities/PoisonedEffect", typeof(Transform)) as Transform;
	}

    protected override void OnTick(float time)
    {
        enemyStat.MagicDamageTaken(Damage, attacker);
        enemyStat.PoisonRegenModifier = RegenReduction;
    }

    protected override void OnEffectStart()
    {
        if (PartEff == null)
            PartEff = Instantiate(ParticleEffect.gameObject, this.transform.position, Quaternion.identity, this.transform);
        //enemyStat.getEffectsUI().
    }

    protected override void OnEffectEnd()
    {
        if (PartEff != null)
            Destroy(PartEff);
    }
}
