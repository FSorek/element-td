using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBleed : ApplyBehaviour {

    public int damage;
    public float duration;
    public float tickFrequency;
    public override void Apply(GameObject target)
    {
        if (!target.GetComponent<Bleed>())
            target.AddComponent<Bleed>().OnApply(duration, tickFrequency, damage, this.GetComponent<Turret>());
        else if (target.GetComponent<Bleed>().damagePerSecond <= damage)
        {
            target.GetComponent<Bleed>().Remove();
            target.AddComponent<Bleed>().OnApply(duration, tickFrequency, damage, this.GetComponent<Turret>());
        }
    }
}
