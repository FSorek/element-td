using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPoison : ApplyBehaviour {
    public int damagePerSecond;
    [Range(0, 1)]
    public float RegenerationReduction;
    public float duration;
    public float tickFrequency;
    public override void Apply(GameObject target)
    {

        if (!target.GetComponent<Poison>())
            target.AddComponent<Poison>().OnApply(duration, tickFrequency, RegenerationReduction, damagePerSecond, this.GetComponent<Turret>());
        else
        {
            if (target.GetComponent<Poison>().Damage <= damagePerSecond)
            {
                target.GetComponent<Poison>().Remove();
                target.AddComponent<Poison>().OnApply(duration, tickFrequency, RegenerationReduction, damagePerSecond, this.GetComponent<Turret>());
            }
        }
    }
}
