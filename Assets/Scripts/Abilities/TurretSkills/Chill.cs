using UnityEngine;

public class Chill : EffectOnHit
{
    [Range(0,1f)]
    public float slowPercentage;
    private GameObject PartEff;
    private EnemyStat enemyStat;
    // Use this for initialization
    public void OnApply(float _duration, float _slowPercentage)
    {
        duration = _duration;
        slowPercentage = _slowPercentage;
    }
    protected override void Initialize()
    {
        enemyStat = this.gameObject.GetComponent<EnemyStat>();
        ParticleEffect = Resources.Load("Objects/Abilities/ChillEffect", typeof(Transform)) as Transform;
    }

    protected override void OnEffectStart()
    {
        if(PartEff==null)
            PartEff = Instantiate(ParticleEffect.gameObject, this.transform.position, Quaternion.identity, this.transform);
        if (enemyStat.ChillMovementPenalty<=slowPercentage)
        {
            enemyStat.ChillMovementPenalty = slowPercentage;
            enemyStat.UpdateMovement();
        }
    }

    protected override void OnEffectEnd()
    {
        if (PartEff != null)
            Destroy(PartEff);

            enemyStat.ChillMovementPenalty -= slowPercentage;
            enemyStat.UpdateMovement();

    }
}
