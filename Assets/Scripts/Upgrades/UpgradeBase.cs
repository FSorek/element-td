using UnityEngine;
using System.Collections;
public class upgradeCat
{
    public enum upCategory
    {
        General,
        Fire,
        Air,
        Water,
        Earth
    }
}
public abstract class UpgradeBase  {

	public string uName;
	public string uDescription;
    public int uCurrentUpgradeLevel = 0;
    public int initialCost = 5;
    public int linearCostAdd = 3;
    public int maxUpgradeLevel = 3;

	public upgradeCat.upCategory uCategory;

    public abstract void Action(GameObject turret, int lvToApply);
	

}
