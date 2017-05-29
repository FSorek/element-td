using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodlust : MonoBehaviour {

    private EnemyStat enemyStat;
    [Range(0,10)][Tooltip("Za kazdy 1% HP jednosta dostaje 1% wiecej movement speeda * BonusSpeedPerPercentHealth")]
    public float BonusSpeedPerPercentHealth;

    private void Start()
    {
        enemyStat = GetComponent<EnemyStat>();
        InvokeRepeating("UpdateSpeed", 1f, .25f);
    }
    private void Update()
    {
        
    }

    void UpdateSpeed()
    {
        enemyStat.BloodLustBonus = enemyStat.baseMovementSpeed * (1 - ((float)enemyStat.currHP / (float)enemyStat.MaxHealth)) * (BonusSpeedPerPercentHealth);
        if (enemyStat.BloodLustBonus != 0)
            enemyStat.UpdateMovement();
    }
}
