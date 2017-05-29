using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightEnchancedTowers : MonoBehaviour {

    public GameObject highlightEffect;


    [HideInInspector]
    public List<GameObject> fireElements = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> waterElements = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> airElements = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> earthElements = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> lifElements = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> deathElements = new List<GameObject>();

    public List<GameObject>[] elements = new List<GameObject>[6];
    public static HighlightEnchancedTowers Instance;
    
    private void Start()
    {
        elements[0] = fireElements;
        elements[1] = waterElements;
        elements[2] = airElements;
        elements[3] = earthElements;
        elements[4] = lifElements;
        elements[5] = deathElements;
        Instance = this;
    }

    private int getType(string type)
    {
        int num=0;
        switch (type)
        {
            case "FIRE":
                num = 0;
                break;
            case "WATER":
                num = 1;
                break;
            case "AIR":
                num = 2;
                break;
            case "EARTH":
                num = 3;
                break;
            case "LIFE":
                num = 4;
                break;
            case "DEATH":
                num = 5;
                break;
        }
        return num;
    }


    public void Hilight(string type, GameObject Exception)
    {
        int num = getType(type);

        foreach (GameObject tower in elements[num])
        {
            if (tower.gameObject == Exception.gameObject)
                continue;
            GameObject o;
            if (tower.transform.FindChild("Highlight"))
            {
                o = tower.transform.FindChild("Highlight").gameObject;
                o.SetActive(true);
            }
            else
            {
                GameObject h = Instantiate(highlightEffect, tower.transform.position, Quaternion.identity, tower.transform);
                h.name = highlightEffect.name;
            }
        }
    }
    public void Unhilight()
    {
        for(int i=0; i<6;i++)
        foreach (GameObject tower in elements[i])
        {
            GameObject o;
            if (tower.transform.FindChild("Highlight"))
            {
                o = tower.transform.FindChild("Highlight").gameObject;
                o.SetActive(false);
            }
        }
    }

}
