using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : OnDeath
{
    public int AmountOfSplit;
    public Transform Unit;
    public override void Activate()
    {
        for(int i=0; i<AmountOfSplit; i++)
        {
            GameObject splitUnit = Instantiate(Unit.gameObject, transform.position, transform.rotation);
            splitUnit.GetComponent<EnemyMovement>().setNextClosestWaypoint(GetComponent<EnemyMovement>().waypointIndex);
            WinLoseManager.Instance.AddEnemyCount();
        }
    }
}
