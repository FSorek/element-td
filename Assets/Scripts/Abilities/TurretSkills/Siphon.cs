using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siphon : EffectOnHit
{
    public float bonusDamageFromHealth;
    public int damageCap;
    private GameObject PartEff;
    private EnemyStat enemyStat;
    private Damages damage = new Damages();
    // Use this for initialization
    public void OnApply(float _bonusDmgFromHealth, int _dmgCap, Turret _attacker)
    {
        bonusDamageFromHealth = _bonusDmgFromHealth;
        damageCap = _dmgCap;
        attacker = _attacker;
    }
    protected override void Initialize()
    {
        enemyStat = this.gameObject.GetComponent<EnemyStat>();
        if (enemyStat.currHP * bonusDamageFromHealth < damageCap)
            damage.DeathDamage = (int)(enemyStat.currHP * bonusDamageFromHealth);
        else
            damage.DeathDamage = damageCap;
    }

    protected override void OnEffectStart()
    {
        enemyStat.DamageTaken(damage, attacker);
        Remove();
    }

    protected override void OnEffectEnd()
    {
    }
}
