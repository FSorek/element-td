using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overflow : MonoBehaviour {

    private EnemyStat enemyStat;
    [Range(0, 1f)]
    public float hpThreshold;
    private float calculatedHpThreshold;
    private float calculatedShieldRecovery;
    private void Start()
    {
        enemyStat = GetComponent<EnemyStat>();
        calculatedHpThreshold = enemyStat.MaxHealth * hpThreshold;
        calculatedShieldRecovery = enemyStat.MaxHealth - calculatedHpThreshold;
    }

    private void Update()
    {
        if (enemyStat.currHP <= calculatedHpThreshold)
        {
            enemyStat.MaxShields += (int)calculatedShieldRecovery;
            enemyStat.currSP = (int)calculatedShieldRecovery;
            Destroy(this);
        }
    }

}
