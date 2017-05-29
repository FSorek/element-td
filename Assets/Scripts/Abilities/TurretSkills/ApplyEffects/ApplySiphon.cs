using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplySiphon : ApplyBehaviour
{
    [Range(0, 1f)]
    public float bonusDamageFromHealth;
    public int damageCap;
    public override void Apply(GameObject target)
    {
            target.AddComponent<Siphon>().OnApply(bonusDamageFromHealth, damageCap, this.gameObject.GetComponent<Turret>());
    }
}
