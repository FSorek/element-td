using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegatesEventsManager : MonoBehaviour {
    public delegate void WaveEventHandler();
    public delegate void UpgradeEventHandler(UpgradeBase upgrade, int upLevel);


    public static event WaveEventHandler OnWaveEnd;
    public static event UpgradeEventHandler OnUpgradeResearched;




    public static void WaveFinished()
    {
        if (OnWaveEnd != null)
            OnWaveEnd();
    }

    public static void UpgradeResearch(UpgradeBase upgrade, int upLevel)
    {
        if (OnUpgradeResearched != null)
            OnUpgradeResearched(upgrade, upLevel);
    }
}
