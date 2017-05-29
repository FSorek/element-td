using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critical : EffectOnHit
{
    public float damageMultiplier;
    private GameObject PartEff;
    private EnemyStat enemyStat;
    private Damages damage = new Damages();
    // Use this for initialization
    public void OnApply(float _damageMultiplier, Damages _damage, Turret _attacker)
    {
        damageMultiplier = _damageMultiplier;
        damage.AirDamage = (int)(_damage.AirDamage * (damageMultiplier - 1));
        damage.WaterDamage = (int)(_damage.WaterDamage * (damageMultiplier - 1));
        damage.FireDamage = (int)(_damage.FireDamage * (damageMultiplier - 1));
        damage.EarthDamage = (int)(_damage.EarthDamage * (damageMultiplier - 1));
        damage.DeathDamage = (int)(_damage.DeathDamage * (damageMultiplier - 1));
        damage.LifeDamage = (int)(_damage.LifeDamage * (damageMultiplier - 1));
        attacker = _attacker;
    }
    protected override void Initialize()
    {
        enemyStat = this.gameObject.GetComponent<EnemyStat>();
    }

    protected override void OnEffectStart()
    {
        enemyStat.DamageTaken(damage, attacker);
        Remove();
    }
}
