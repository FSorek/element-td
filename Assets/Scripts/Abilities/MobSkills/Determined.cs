using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Determined : OnSpawn
{
    public float duration;
    public override void Activate()
    {
        GetComponent<EnemyStat>().Determined = true;
        Invoke("OnEnd", duration);
    }

    private void OnEnd()
    {
        GetComponent<EnemyStat>().Determined = false;
        Destroy(this);
    }
}