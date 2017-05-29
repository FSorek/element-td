using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDeepWounds : ApplyBehaviour
{

    public Damages BonusDmgPerHit;
    public override void Apply(GameObject target)
    {
        target.AddComponent<DeepWounds>().OnApply(BonusDmgPerHit,this.GetComponent<Turret>());
    }
}
