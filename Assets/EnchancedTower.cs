using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchancedTower : MonoBehaviour {
    private string theTag;
    private void Start()
    {
        theTag = this.tag;
        switch (theTag)
        {
            case "EnchancedFire":
                HighlightEnchancedTowers.Instance.fireElements.Add(this.gameObject);
                break;
            case "EnchancedWater":
                HighlightEnchancedTowers.Instance.waterElements.Add(this.gameObject);
                break;
            case "EnchancedEarth":
                HighlightEnchancedTowers.Instance.earthElements.Add(this.gameObject);
                break;
            case "EnchancedAir":
                HighlightEnchancedTowers.Instance.airElements.Add(this.gameObject);
                break;
            case "EnchancedLife":
                HighlightEnchancedTowers.Instance.lifElements.Add(this.gameObject);
                break;
            case "EnchancedDeath":
                HighlightEnchancedTowers.Instance.deathElements.Add(this.gameObject);
                break;
        }
    }

    public void RemoveFromList()
    {
        switch (theTag)
        {
            case "EnchancedFire":
                HighlightEnchancedTowers.Instance.fireElements.Remove(this.gameObject);
                break;
            case "EnchancedWater":
                HighlightEnchancedTowers.Instance.waterElements.Remove(this.gameObject);
                break;
            case "EnchancedEarth":
                HighlightEnchancedTowers.Instance.earthElements.Remove(this.gameObject);
                break;
            case "EnchancedAir":
                HighlightEnchancedTowers.Instance.airElements.Remove(this.gameObject);
                break;
            case "EnchancedLife":
                HighlightEnchancedTowers.Instance.lifElements.Remove(this.gameObject);
                break;
            case "EnchancedDeath":
                HighlightEnchancedTowers.Instance.deathElements.Remove(this.gameObject);
                break;
        }
    }
}
