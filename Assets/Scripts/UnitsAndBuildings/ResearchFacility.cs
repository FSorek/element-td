using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchFacility : MonoBehaviour {
    public GameObject occupiedNode;
    // Use this for initialization
    void Start () {
        DelegatesEventsManager.OnWaveEnd += this.AddResearchPoints;
    }
    void OnDisable()
    {
        DelegatesEventsManager.OnWaveEnd -= this.AddResearchPoints;
    }
    // Update is called once per frame
    public void Unsubscribe()
    {
        DelegatesEventsManager.OnWaveEnd -= this.AddResearchPoints;
    }

    public void AddResearchPoints()
    {
        PlayerManager.ResearchPoints += 1;
    }
}
