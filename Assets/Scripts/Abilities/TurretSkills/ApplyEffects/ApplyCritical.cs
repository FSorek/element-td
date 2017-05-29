using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCritical : ApplyBehaviour
{
    
    public float damageMultiplier;
    [Range(0, 1)]
    public float applyChance;
    public override void Apply(GameObject target)
    {
        if (Random.Range(0, 1f) > applyChance)
            return;
        target.AddComponent<Critical>().OnApply(damageMultiplier, this.GetComponent<Turret>().finalDamageValues, this.gameObject.GetComponent<Turret>());
    }
}
