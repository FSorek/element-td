using UnityEngine;
using System.Collections;

public class GeneralFireRateUpgrade : UpgradeBase
{

    public GeneralFireRateUpgrade()
    {
        uName = "General Fire Rate Upgrade";
        uDescription = "Upgrades Fire Rate for all Towers";
        uCategory = upgradeCat.upCategory.General;
        initialCost = 5;
        linearCostAdd = 3;
        maxUpgradeLevel = 3;
    }

    public override void Action(GameObject turret, int lvToApply)
    {
        switch (lvToApply)
        {
            case 1:
                turret.GetComponent<Turret>().percFireRate += (1-0.15f);
                break;
            case 2:
                turret.GetComponent<Turret>().percFireRate += (1 - 0.15f);
                break;
            case 3:
                turret.GetComponent<Turret>().percFireRate += (1 - 0.20f);
                break;
        }

    }

}
