using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyChill : ApplyBehaviour
{
    [Range(0, 1f)]
    public float slowPercentage;
    public float duration;
    public override void Apply(GameObject target)
    {

        if (!target.GetComponent<Chill>())
            target.AddComponent<Chill>().OnApply(duration, slowPercentage);
        else if (target.GetComponent<Chill>().slowPercentage <= slowPercentage)
        {
            target.GetComponent<Chill>().Remove();
            target.AddComponent<Chill>().OnApply(duration, slowPercentage);
        }
    }
}
