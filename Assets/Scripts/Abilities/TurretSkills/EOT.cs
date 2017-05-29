using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EOT : MonoBehaviour {
    public float duration;
    public float tickFrequency;
    public Transform ParticleEffect;
    public Turret attacker;
    protected virtual void Start () {
        Initialize();
        StartCoroutine(WaitAndTick());
	}

    protected virtual IEnumerator WaitAndTick()
    {
        yield return new WaitForSeconds(.2f);
        OnEffectStart();
        for(float counter = 0f; counter <= duration; counter += tickFrequency)
        {
            OnTick(counter);
            yield return new WaitForSeconds(tickFrequency);
        }
        Remove();
    }

    public virtual void Remove() { OnEffectEnd(); Destroy(this); }
    protected virtual void Initialize() { }
    protected virtual void OnEffectStart() { }
    protected virtual void OnEffectEnd() { }
    protected virtual void OnTick(float time) { }
}
