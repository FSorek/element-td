using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyAgony : ApplyBehaviour
{
    [Range(0, 3f)]
    public float BonusDamageMultiplier;
    public float duration;
    public override void Apply(GameObject target)
    {

        if (!target.GetComponent<Agony>())
            target.AddComponent<Agony>().OnApply(duration, BonusDamageMultiplier);
        else if (target.GetComponent<Agony>().BonusDamageMultiplier <= BonusDamageMultiplier)
        {
            target.GetComponent<Agony>().Remove();
            target.AddComponent<Agony>().OnApply(duration, BonusDamageMultiplier);
        }
    }
}
