using UnityEngine;
using System.Collections;

public class MergingList : MonoBehaviour {
    public enum enchantedTypes
    {
        NOTHING,
        EnchantedFire,
        EnchantedWater,
        EnchantedEarth,
        EnchantedAir,
        EnchantedLife,
        EnchantedDeath
    }
	[System.Serializable]
	public class Recipe
	{
		public Transform MergesWith;
		public Transform Result;
	}
}
