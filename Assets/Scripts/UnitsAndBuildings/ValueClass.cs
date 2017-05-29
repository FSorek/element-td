using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllowedTargets
{
    Ground, Air, Both
}
[System.Serializable]
public class Damages
{
    public int FireDamage;
    public int WaterDamage;
    public int AirDamage;
    public int EarthDamage;
    public int DeathDamage;
    public int LifeDamage;
    public float fireRate;
    
}
public enum TurretType
{
    SingleTarget,
    AreaDamage,
    ChainAttack,
    MultipleTarget,
    Line
}

public class ValueClass{}
