using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevTools : MonoBehaviour {

    //public GameObject DevTools_Panel;

    //DEVTOOLS DETAILS TAB
    public GameObject DevTools_Details;
    //DPS
    public Text DPS_SHIELDS;
    public Text DPS_AIR;
    public Text DPS_HEAVY;
    public Text DPS_LIGHT;
    public Text DPS_UNARMORED;
    public Text DPS_ELEMENTAL;
    public Text DPS_FLAT;

    //TOTAL DAMAGE
    public Text TOTAL_SENT;
    public Text TOTAL_DEALT;

    int waveskipNum;

    private void Start()
    {
        InvokeRepeating("CalculateDPS", 1f, .5f);
        waveskipNum = 0;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                PlayerManager.Gold += 1000;
            else
                PlayerManager.Gold += 100;
        }

        if (Input.GetKeyDown(KeyCode.P))
            WaveSpawner.Instance.DEV_skipTo(++waveskipNum);
    }

    void CalculateDPS()
    {
        if (DevTools_Details.activeSelf)
        {
            if (MainManagerScript.Instance.selectedTower != null)
            {
                Turret selectedTurret = MainManagerScript.Instance.selectedTowerTurret;
                DPS_SHIELDS.text = (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 0.5f) + (selectedTurret.damages.AirDamage * 0.5f) + (selectedTurret.damages.EarthDamage * 0.5f) + (selectedTurret.damages.WaterDamage * 2) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage * .5f))).ToString() + " | "
                    + (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 0.5f) + (selectedTurret.damages.AirDamage * 0.5f) + (selectedTurret.damages.EarthDamage * 0.5f) + (selectedTurret.damages.WaterDamage * 2) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage * .5f)) / selectedTurret.damages.fireRate).ToString();

                DPS_AIR.text = (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 0.5f) + (selectedTurret.damages.AirDamage * 2f) + (selectedTurret.damages.EarthDamage * 0.5f) + (selectedTurret.damages.WaterDamage * 0.5f) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage * .75f))).ToString() + " | "
                    + (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 0.5f) + (selectedTurret.damages.AirDamage * 2f) + (selectedTurret.damages.EarthDamage * 0.5f) + (selectedTurret.damages.WaterDamage * 0.5f) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage * .75f)) / selectedTurret.damages.fireRate).ToString();

                DPS_HEAVY.text = (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 0.5f) + (selectedTurret.damages.AirDamage * 0.5f) + (selectedTurret.damages.EarthDamage * 1.5f) + (selectedTurret.damages.WaterDamage * 0.75f) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage * .5f))).ToString() + " | "
                    + (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 0.5f) + (selectedTurret.damages.AirDamage * 0.5f) + (selectedTurret.damages.EarthDamage * 1.5f) + (selectedTurret.damages.WaterDamage * 0.75f) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage * .5f)) / selectedTurret.damages.fireRate).ToString();

                DPS_LIGHT.text = (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 1.25f) + (selectedTurret.damages.AirDamage * 0.75f) + (selectedTurret.damages.EarthDamage * 0.75f) + (selectedTurret.damages.WaterDamage * 0.75f) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage * .75f))).ToString() + " | "
                    + (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 1.25f) + (selectedTurret.damages.AirDamage * 0.75f) + (selectedTurret.damages.EarthDamage * 0.75f) + (selectedTurret.damages.WaterDamage * 0.75f) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage * .75f)) / selectedTurret.damages.fireRate).ToString();


                DPS_UNARMORED.text = (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 1.25f) + (selectedTurret.damages.AirDamage * .5f) + (selectedTurret.damages.EarthDamage * .75f) + (selectedTurret.damages.WaterDamage * 0.5f) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage * 1.5f))).ToString() + " | "
                    + (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 1.25f) + (selectedTurret.damages.AirDamage * .5f) + (selectedTurret.damages.EarthDamage * .75f) + (selectedTurret.damages.WaterDamage * 0.5f) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage * 1.5f)) / selectedTurret.damages.fireRate).ToString();


                DPS_ELEMENTAL.text = (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 0.5f) + (selectedTurret.damages.AirDamage * 0.5f) + (selectedTurret.damages.EarthDamage * 0.5f) + (selectedTurret.damages.WaterDamage * 0.5f) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage))).ToString() + " | "
                    + (Mathf.RoundToInt((selectedTurret.damages.FireDamage * 0.5f) + (selectedTurret.damages.AirDamage * 0.5f) + (selectedTurret.damages.EarthDamage * 0.5f) + (selectedTurret.damages.WaterDamage * 0.5f) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage)) / selectedTurret.damages.fireRate).ToString();


                DPS_FLAT.text = (Mathf.RoundToInt((selectedTurret.damages.FireDamage) + (selectedTurret.damages.AirDamage) + (selectedTurret.damages.EarthDamage) + (selectedTurret.damages.WaterDamage) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage))).ToString() + " | "
                    + (Mathf.RoundToInt((selectedTurret.damages.FireDamage) + (selectedTurret.damages.AirDamage) + (selectedTurret.damages.EarthDamage) + (selectedTurret.damages.WaterDamage) + (selectedTurret.damages.DeathDamage) + (selectedTurret.damages.LifeDamage)) / selectedTurret.damages.fireRate).ToString();



                TOTAL_SENT.text = selectedTurret.totalDamageSent.ToString();
                TOTAL_DEALT.text = "<color=lightblue>" + selectedTurret.totalShieldDamageDealt.ToString() + "</color> | " + selectedTurret.totalDamageDealt.ToString();

            }

        }
    }

}
