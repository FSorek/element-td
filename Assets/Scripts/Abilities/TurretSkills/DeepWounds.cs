using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepWounds : EffectOnHit
{

    public Damages DamageBonusPerHit;
    private GameObject PartEff;
    private EnemyStat enemyStat;
    private Turret turret;
    // Use this for initialization
    public void OnApply(Damages _DamageBonusPerHit, Turret _turret)
    {
        DamageBonusPerHit = _DamageBonusPerHit;
        turret = _turret;
    }
    protected override void Initialize()
    {
        enemyStat = this.gameObject.GetComponent<EnemyStat>();
        turret.DeepWoundsBonus.AirDamage += DamageBonusPerHit.AirDamage;
        turret.DeepWoundsBonus.EarthDamage += DamageBonusPerHit.EarthDamage;
        turret.DeepWoundsBonus.FireDamage += DamageBonusPerHit.FireDamage;
        turret.DeepWoundsBonus.WaterDamage += DamageBonusPerHit.WaterDamage;
        turret.DeepWoundsBonus.DeathDamage += DamageBonusPerHit.DeathDamage;
        turret.DeepWoundsBonus.LifeDamage += DamageBonusPerHit.LifeDamage;
        
    }

    protected override void OnEffectStart()
    {
        enemyStat.DamageTaken(turret.DeepWoundsBonus, turret);
        Remove();
    }

    protected override void OnEffectEnd()
    {
    }

    public override void Remove()
    {
        OnEffectEnd();
        base.Remove();
    }
}
