using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {
    private EnemyStat enemyStat;
    [Range(0,1f)]
    public float hpThreshold;
    [Range(0, 1f)]
    public float shieldRecovery;
    private float calculatedHpThreshold;
    private float calculatedShieldRecovery;
    private void Start()
    {
        enemyStat = GetComponent<EnemyStat>();
        calculatedHpThreshold = enemyStat.MaxHealth * hpThreshold;
        calculatedShieldRecovery = enemyStat.MaxShields * shieldRecovery;
    }

    private void Update()
    {
        if(enemyStat.currHP <= calculatedHpThreshold)
        {
            enemyStat.currSP = (int)calculatedShieldRecovery;
            Destroy(this);
        }
    }
}
