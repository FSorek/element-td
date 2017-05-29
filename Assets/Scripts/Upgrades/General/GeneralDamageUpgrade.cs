using UnityEngine;
using System.Collections;

public class GeneralDamageUpgrade : UpgradeBase
{

   public GeneralDamageUpgrade()
    {
        uName = "General Damage Upgrade";
        uDescription = "Upgrades All Damage values";
        uCategory = upgradeCat.upCategory.General;
    }

    public override void Action(GameObject turret, int lvToApply)
    {
        switch (lvToApply)
        {
            case 1:
                turret.GetComponent<Turret>().percentDamageBuff_Fire += 0.2f;
                turret.GetComponent<Turret>().percentDamageBuff_Air += 0.2f;
                turret.GetComponent<Turret>().percentDamageBuff_Water += 0.2f;
                turret.GetComponent<Turret>().percentDamageBuff_Earth += 0.2f;
                break;
            case 2:
                turret.GetComponent<Turret>().percentDamageBuff_Fire += 0.25f;
                turret.GetComponent<Turret>().percentDamageBuff_Air += 0.25f;
                turret.GetComponent<Turret>().percentDamageBuff_Water += 0.25f;
                turret.GetComponent<Turret>().percentDamageBuff_Earth += 0.25f;
                break;
            case 3:
                turret.GetComponent<Turret>().percentDamageBuff_Fire += 0.3f;
                turret.GetComponent<Turret>().percentDamageBuff_Air += 0.3f;
                turret.GetComponent<Turret>().percentDamageBuff_Water += 0.3f;
                turret.GetComponent<Turret>().percentDamageBuff_Earth += 0.3f;
                break;
        }

    }

}
