using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeEntity : EOT {
    public LayerMask enemyLayer;
    private float Radius;
    private ApplyBehaviour[] _ApplyEffects;
    public void Apply(float _radius, float _duration, float tickFreq)
    {
        Radius = _radius;
        duration = _duration;
        tickFrequency = tickFreq;

        Initialize();
        StartCoroutine(WaitAndTick());
    }
    protected override void Start()
    {
        
    }
    protected override void Initialize()
    {
        _ApplyEffects = this.GetComponents<ApplyBehaviour>();
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem particle in particles)
        {
            ParticleSystem.ShapeModule shape = particle.shape;
            shape.radius = Radius;
        }
    }
    protected override void OnEffectStart()
    {

    }
    protected override void OnTick(float time)
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, Radius, enemyLayer);
        foreach (Collider enemy in enemies)
        {
            if (_ApplyEffects.Length > 0)
            {
                foreach (ApplyBehaviour effect in _ApplyEffects)
                {
                    effect.Apply(enemy.gameObject);
                }
            }
        }
    }

    public override void Remove()
    {
        Destroy(this.gameObject);
    }
}
