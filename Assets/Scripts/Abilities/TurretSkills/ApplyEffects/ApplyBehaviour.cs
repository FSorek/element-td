using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBehaviour : MonoBehaviour {
    public Sprite Image;
    public string abilityName;
    public string toolTip;


    public virtual void Apply(GameObject target) { }
    public void RemoveAbility(GameObject obj, ApplyBehaviour toRemove)
    {
        ApplyBehaviour[] Abilities = obj.GetComponents<ApplyBehaviour>();
        foreach (ApplyBehaviour ability in Abilities)
        {
            if (ability == toRemove)
            {
                DestroyImmediate(ability);
            }
        }
    }
}
