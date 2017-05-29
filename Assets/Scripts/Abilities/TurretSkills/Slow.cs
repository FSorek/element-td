using UnityEngine;

public class Slow : EffectOnHit
{
    public float slowPercentage;
    private float slowReductionTime;
    private float slowReductionAmount;
    private GameObject PartEff;
    private EnemyStat enemyStat;
    // Use this for initialization
    public void OnApply(float _duration, float _slowPercentage, float _slowReductionTime, float _slowReductionAmount)
    {
        duration = _duration;
        slowPercentage = _slowPercentage;
        slowReductionTime = _slowReductionTime;
        slowReductionAmount = _slowReductionAmount;
    }
    protected override void Initialize()
    {
        enemyStat = this.gameObject.GetComponent<EnemyStat>();
        ParticleEffect = Resources.Load("Objects/Abilities/SlowEffect", typeof(Transform)) as Transform;
    }

    protected override void OnEffectStart()
    {
        if (PartEff == null)
            PartEff = Instantiate(ParticleEffect.gameObject, this.transform.position, Quaternion.identity, this.transform);
        if (enemyStat.SlowMovementPenalty <= slowPercentage)
        {
            enemyStat.SlowMovementPenalty = slowPercentage;
            enemyStat.UpdateMovement();
        }
        InvokeRepeating("SlowReduction", slowReductionTime, slowReductionTime);
    }

    void SlowReduction()
    {
        
        enemyStat.SlowMovementPenalty *= slowReductionAmount;
        enemyStat.UpdateMovement();
        
    }

    protected override void OnEffectEnd()
    {
        if (PartEff != null)
            Destroy(PartEff);

        enemyStat.SlowMovementPenalty = 0;
        enemyStat.UpdateMovement();
        
    }
}
