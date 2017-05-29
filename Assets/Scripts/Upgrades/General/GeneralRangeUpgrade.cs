using UnityEngine;
using System.Collections;

public class GeneralRangeUpgrade : UpgradeBase
{

    public GeneralRangeUpgrade()
    {
        uName = "General Range Upgrade";
        uDescription = "Upgrades Range for all Towers";
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
                turret.GetComponent<Turret>().range += 1;
                break;
            case 2:
                turret.GetComponent<Turret>().range += 1;
                break;
            case 3:
                turret.GetComponent<Turret>().range += 1;
                break;
        }

    }

}
