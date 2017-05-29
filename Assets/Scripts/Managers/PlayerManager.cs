using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
	public static int Gold;
	public static int ResearchPoints;

	[Header("UI Setup")]
	public Text goldAmountUI;
    public Text ResearchPointsAmountUI;


    public static PlayerManager Instance;
	// Use this for initialization
	void Awake(){
		Instance = this;
	}
	void Start () {
		Gold = 300;
        ResearchPoints = 150;
	}
	
	// Update is called once per frame
	void Update () {
        updateGold();
        updateRP();
    }

    void updateGold()
    {
        if (goldAmountUI.text == Gold.ToString())
            return;
        goldAmountUI.text = Gold.ToString();
    }

    void updateRP()
    {
        if (ResearchPointsAmountUI.text == ResearchPoints.ToString())
            return;
        ResearchPointsAmountUI.text = ResearchPoints.ToString();
    }
}
