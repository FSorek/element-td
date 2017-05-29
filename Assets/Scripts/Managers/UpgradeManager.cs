using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UpgradeManager : MonoBehaviour {

	public List<UpgradeBase> upgrades = new List<UpgradeBase>();
	public static UpgradeManager Instance;
    public Dictionary<string,UpgradeBase> allUpgrades = new Dictionary<string, UpgradeBase>();
	private UpgradeBase FireDamageUp = new FireDamageUpgrade ();
    private UpgradeBase GeneralDamageUp = new GeneralDamageUpgrade();
    private UpgradeBase GeneralRangeUp = new GeneralFireRateUpgrade();
    private UpgradeBase GeneralFireRateUp = new GeneralFireRateUpgrade();
    private UpgradeBase GeneralSpecialUp = new GeneralSpecialUpgrade();

    void Start()
	{
		Instance = this;
        //--------------------GENERAL UPGRADES
        allUpgrades.Add("GeneralDamageUp", GeneralDamageUp);
        allUpgrades.Add("GeneralRangeUp", GeneralRangeUp);
        allUpgrades.Add("GeneralFireRateUp", GeneralFireRateUp);
        allUpgrades.Add("GeneralSpecialUp", GeneralSpecialUp);

        //--------------------FIRE UPGRADES
        allUpgrades.Add("FireDamageUp", FireDamageUp);

        //--------------------WATER UPGRADES


        //--------------------AIR UPGRADES


        //--------------------EARTH UPGRADES


    }


    public void General_UpgradeDamage(string upgradeName)
	{
        UpgradeBase upgrade = allUpgrades[upgradeName];
		int UpgradeLv = upgrade.uCurrentUpgradeLevel;
		int TotalCost = upgrade.initialCost + upgrade.linearCostAdd * UpgradeLv;

		if (PlayerManager.ResearchPoints < TotalCost || UpgradeLv >= upgrade.maxUpgradeLevel)
			return;
		PlayerManager.ResearchPoints -= TotalCost;
		UpgradeLv++;
        upgrade.uCurrentUpgradeLevel++;
		if (!upgrades.Contains (upgrade)) {
			upgrades.Add (upgrade);
		}
        DelegatesEventsManager.UpgradeResearch(upgrade, UpgradeLv);
    }
}
