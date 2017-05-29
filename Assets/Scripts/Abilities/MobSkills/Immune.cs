using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immune : OnSpawn {
    public float cleanseCycleTime;
    public override void Activate()
    {
        InvokeRepeating("Cleanse", cleanseCycleTime, cleanseCycleTime);
    }

    private void Cleanse()
    {
        if (GetComponent<Chill>())
        {
            GetComponent<Chill>().Remove();
        }
        if (GetComponent<Stun>())
        {
            GetComponent<Stun>().Remove();
        }
        if (GetComponent<Burning>())
        {
            GetComponent<Burning>().Remove();
        }
        if (GetComponent<Poison>())
        {
            GetComponent<Poison>().Remove();
        }
        if (GetComponent<Agony>())
        {
            GetComponent<Agony>().Remove();
        }
        if (GetComponent<Bleed>())
        {
            GetComponent<Bleed>().Remove();
        }
        if (GetComponent<Slow>())
        {
            GetComponent<Slow>().Remove();
        }
    }
}
