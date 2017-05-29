using UnityEngine;
using System.Collections;

public class FireDamageUpgrade : UpgradeBase {

    public FireDamageUpgrade()
	{
		uName = "Fire Damage Upgrade";
		uDescription = "Upgrades the Fire Damage value";
		uCategory = upgradeCat.upCategory.Fire;
	}

	public override void Action (GameObject turret, int lvToApply)
	{
		if (turret.GetComponent<Turret> ().damages.FireDamage <= 0)
			return;
		switch (lvToApply) {
		case 1:
			turret.GetComponent<Turret> ().percentDamageBuff_Fire += 0.2f;
			break;
		case 2:
			turret.GetComponent<Turret> ().percentDamageBuff_Fire += 0.25f;
			break;
		case 3:
			turret.GetComponent<Turret> ().percentDamageBuff_Fire += 0.55f;
			break;
		}

	}

}
