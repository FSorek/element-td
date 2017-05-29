using UnityEngine;

public class Stun : EffectOnHit
{
    private GameObject PartEff;
    private EnemyStat enemyStat;
    // Use this for initialization
    public void OnApply(float _duration)
    {
        duration = _duration;
    }
    protected override void Initialize()
    {
        enemyStat = this.gameObject.GetComponent<EnemyStat>();
        ParticleEffect = Resources.Load("Objects/Abilities/StunEffect", typeof(Transform)) as Transform;
    }

    protected override void OnEffectStart()
    {
        if (PartEff == null)
            PartEff = Instantiate(ParticleEffect.gameObject, this.transform.position, Quaternion.identity, this.transform);

            enemyStat.StunPenalty = 1;
            enemyStat.UpdateMovement();

    }

    protected override void OnEffectEnd()
    {
        if (PartEff != null)
            Destroy(PartEff);

        enemyStat.StunPenalty = 0;
        enemyStat.UpdateMovement();
    }

    public override void Remove()
    {
        OnEffectEnd();
        Destroy(this, enemyStat.stunResistanceTime);
    }
}
