using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEntity : EffectOnHit
{
    private GameObject EntityToSpawn;
    private GameObject PartEff;
    private float radius;
    private float tickFrequency;
    // Use this for initialization
    public void OnApply(GameObject _entityToSpawn, float _radius, float _duration, float _tickFrequency)
    {
        EntityToSpawn = _entityToSpawn;
        radius = _radius;
        duration = _duration;
        tickFrequency = _tickFrequency;
    }

    protected override void OnEffectStart()
    {
        GameObject spawn = Instantiate(EntityToSpawn, transform.position, Quaternion.identity);
        spawn.GetComponent<AoeEntity>().Apply(radius, duration, tickFrequency);
        Remove();
    }
}
    