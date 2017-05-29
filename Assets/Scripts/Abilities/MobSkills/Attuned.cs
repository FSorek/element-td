using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attuned : OnSpawn
{
    public float changeFrequency;
    public bool random;

    private EnemyStat enemyStat;
    private int Element = 0;
    private void Start()
    {
        enemyStat = GetComponent<EnemyStat>();
    }

    public override void Activate()
    {

        InvokeRepeating("Attune", 0f, changeFrequency);
    }

    void Attune()
    {
        
        if (random)
            Element = Mathf.FloorToInt(UnityEngine.Random.Range(0, 5.99f));
        Damages clearAttunment = new Damages();
        switch (Element)
        {
            case 0:
                enemyStat.attument = clearAttunment;
                enemyStat.attument.FireDamage = 1;
                break;
            case 1:
                enemyStat.attument = clearAttunment;
                enemyStat.attument.WaterDamage = 1;
                break;
            case 2:
                enemyStat.attument = clearAttunment;
                enemyStat.attument.EarthDamage = 1;
                break;
            case 3:
                enemyStat.attument = clearAttunment;
                enemyStat.attument.AirDamage = 1;
                break;
            case 4:
                enemyStat.attument = clearAttunment;
                enemyStat.attument.DeathDamage = 1;
                break;
            case 5:
                enemyStat.attument = clearAttunment;
                enemyStat.attument.LifeDamage = 1;
                break;
        }
        Element++;
        if (!random && Element >= 5)
            Element = 0;
    }
}
