using UnityEngine;
using System.Collections;

public class GeneralSpecialUpgrade : UpgradeBase
{

    public GeneralSpecialUpgrade()
    {
        uName = "General Special Upgrade";
        uDescription = "Adds a specific upgrade for all Towers";
        uCategory = upgradeCat.upCategory.General;
        initialCost = 5;
        linearCostAdd = 3;
        maxUpgradeLevel = 3;
    }

    public override void Action(GameObject turret, int lvToApply)
    {
        Turret tower = turret.GetComponent<Turret>();
        if (tower.turretType == TurretType.MultipleTarget)
            tower.maxTargets += 1;
        else if (tower.turretType == TurretType.AreaDamage)
            tower.AreaOfEffect += 1;
        else if (tower.turretType == TurretType.ChainAttack)
            tower.bounces += 1;
        else if (tower.turretType == TurretType.SingleTarget)
            tower.percentGlobalDamageBuff += 0.2f;
    }

}
