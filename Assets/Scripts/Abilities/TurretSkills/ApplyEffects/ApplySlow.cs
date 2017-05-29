using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplySlow : ApplyBehaviour
{
    [Range(0, 1f)]
    public float slowPercentage;
    public float duration;
    public float slowReductionTime;
    [Range(0,1f)]
    public float slowReductionAmount;
    public override void Apply(GameObject target)
    {

        if (!target.GetComponent<Slow>())
            target.AddComponent<Slow>().OnApply(duration, slowPercentage, slowReductionTime, slowReductionAmount);
        else if (target.GetComponent<Slow>().slowPercentage <= slowPercentage)
        {
            target.GetComponent<Slow>().Remove();
            target.AddComponent<Slow>().OnApply(duration, slowPercentage, slowReductionTime, slowReductionAmount);
        }
    }
}
