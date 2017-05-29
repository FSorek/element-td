using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBerserk : ApplyBehaviour
{
    [Range(0,1f)]
    public float bonusSpeedMultiplier;
    [Range(0,1f)]
    public float applyChance;
    public float duration;
    public override void Apply(GameObject target)
    {
        if (Random.Range(0, 1f) > applyChance)
            return;
        GameObject theTurret = this.gameObject;
        if (!theTurret.GetComponent<Berserk>())
            theTurret.AddComponent<Berserk>().OnApply(bonusSpeedMultiplier, duration, theTurret.GetComponent<Turret>());
    }
}
