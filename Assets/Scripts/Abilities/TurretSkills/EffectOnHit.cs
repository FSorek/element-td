using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnHit : MonoBehaviour
{
    public float duration;
    public Transform ParticleEffect;
    public Turret attacker;
    private void Start()
    {
        Initialize();
        Apply();
    }

    private void Apply()
    {
        OnEffectStart();
        CancelInvoke("Remove");
        Invoke("Remove", duration);
    }

    public virtual void Remove() { OnEffectEnd(); Destroy(this); }
    protected virtual void Initialize() { }
    protected virtual void OnEffectStart() { }
    protected virtual void OnEffectEnd() { }
}
