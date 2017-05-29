using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCreateEntity : ApplyBehaviour {

    public GameObject EntityToSpawn;
    [Range(0, 1)]
    public float applyChance;
    public float radius;
    public float duration;
    public float tickFrequency;
    public override void Apply(GameObject target)
    {
        if (Random.Range(0, 1f) > applyChance)
            return;
        target.AddComponent<CreateEntity>().OnApply(EntityToSpawn, radius, duration, tickFrequency);
    }
}
