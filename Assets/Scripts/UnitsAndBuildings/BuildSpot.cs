using UnityEngine;
using System.Collections;

public class BuildSpot : MonoBehaviour {
	public bool occupied = false;
	// Use this for initialization
	public void setOccupied()
	{
		if (occupied)
			return;
		occupied = true;
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
