using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBurning : ApplyBehaviour
{
    public int damage;
    private Damages damagePerSecond = new Damages();
    public float duration;
    public float tickFrequency;
    public override void Apply(GameObject target)
    {
        damagePerSecond.FireDamage = damage;
        if (!target.GetComponent<Burning>())
            target.AddComponent<Burning>().OnApply(duration, tickFrequency, damagePerSecond, this.gameObject.GetComponent<Turret>());
        else if (target.GetComponent<Burning>().damagePerSecond.FireDamage <= damagePerSecond.FireDamage)
        {
            target.GetComponent<Burning>().Remove();
            target.AddComponent<Burning>().OnApply(duration, tickFrequency, damagePerSecond, this.gameObject.GetComponent<Turret>());
        }
    }
}
