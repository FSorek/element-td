using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agony : EffectOnHit
{
    public float BonusDamageMultiplier;
    private GameObject PartEff;
    private EnemyStat enemyStat;
    // Use this for initialization
    public void OnApply(float _duration, float _BonusDamageMultiplier)
    {
        duration = _duration;
        BonusDamageMultiplier = _BonusDamageMultiplier;
    }
    protected override void Initialize()
    {
        enemyStat = this.gameObject.GetComponent<EnemyStat>();
        ParticleEffect = Resources.Load("Objects/Abilities/AgonyEffect", typeof(Transform)) as Transform;
    }

    protected override void OnEffectStart()
    {
        if (PartEff == null)
            PartEff = Instantiate(ParticleEffect.gameObject, this.transform.position, Quaternion.identity, this.transform);
        if (enemyStat.AgonyModifier <= BonusDamageMultiplier)
        {
            enemyStat.AgonyModifier = BonusDamageMultiplier;
        }
    }

    protected override void OnEffectEnd()
    {
        if (PartEff != null)
            Destroy(PartEff);

        enemyStat.AgonyModifier = 0;
    }
}
