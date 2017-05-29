using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyStun : ApplyBehaviour
{
    [Range(0,1f)]
    public float applyChance;
    public float duration;
    public override void Apply(GameObject target)
    {
        if (Random.Range(0, 1f) > applyChance)
            return;
        if (!target.GetComponent<Stun>())
            target.AddComponent<Stun>().OnApply(duration);
    }
}
