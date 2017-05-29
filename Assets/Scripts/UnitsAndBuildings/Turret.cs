using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Turret : MonoBehaviour, IUpgradeable {

   
    [Header("Visuals")]
	public Sprite Image;
    public int ProjectileID;
	public GameObject ImpactEffect;
	public bool looksAtTarget = false;
    public Transform ShootPoint;

	[Header("On Hit Effect Properties")]
    public List<ApplyBehaviour> ApplyEffects = new List<ApplyBehaviour>();
    [HideInInspector]
    public Damages DeepWoundsBonus = new Damages();
    [HideInInspector]
    public float berserkSpeedBonus;

    [Header("Attributes")]
    public bool EnableTargetFocus;
    [Range(1,6)]
	public int Tier;
	public float projectileSpeed = 5f;
	public float range = 15f;
	public TurretType turretType;
	public AllowedTargets allowedTargets;
	public Damages damages;

    [HideInInspector]
    public int totalDamageDealt = 0;
    [HideInInspector]
    public int totalDamageSent = 0;

    [HideInInspector]
    public int totalShieldDamageDealt = 0;

    [Space][Space][Space]
    public List<MergingList.Recipe> Merging;
    public Transform upgradesTo;


    [HideInInspector]    public Damages finalDamageValues;
	[HideInInspector]    public float percentDamageBuff_Fire = 0;
    [HideInInspector]    public float percentDamageBuff_Air = 0;
    [HideInInspector]    public float percentDamageBuff_Water = 0;
    [HideInInspector]    public float percentDamageBuff_Earth = 0;
    [HideInInspector]    public float percentDamageBuff_Death = 0;
    [HideInInspector]    public float percentDamageBuff_Life = 0;
    [HideInInspector]    public float percentGlobalDamageBuff = 1;
    [HideInInspector]    public float percFireRate = 1f;

    [Header("Area of Effect Attributes")]
	public bool guidedMissle;
	public int AreaOfEffect;
	[Tooltip("Dmg - (Dmg * damageFallOff * (Dist/AreaOfEffect)) // Damage reduction at the end of range")][Range(0,1f)]
	public float damageFallOff; 

	[Header("Chain Attack Attributes")]
	//public bool isChainAttack;
	public float chainRadius;
	public int bounces;

	[Header("Multiple Target Attributes")]
	public int maxTargets;

    [Header("Line Attributes")]
    public float DamageTimer;
    public float DestroyProjectileDelay;
    public int LineDamageRadius;


    [Header("Unity Setup Fields")]
	public Transform target;
	public string enemyTag = "Enemy";
    public GameObject occupiedNode = null;

	private int rotationSpeed = 10;
	private float timeToShoot;
	private List<GameObject> targets = null;
	private Vector3 enemyDistance;
	private int appliedUpgradeLv;


    void Awake()
    {
        if (Image == null)
        {
            Image = Resources.Load("Images/Default/NoImage", typeof(Sprite)) as Sprite;
        }
    }
    void Start () {
        if (ShootPoint == null)
            ShootPoint = this.transform;
        ApplyUpgradesOnStart ();
        DelegatesEventsManager.OnUpgradeResearched += this.ApplyUpgrade;


        InvokeRepeating ("UpdateTarget", 0f,0.5f);
        //InvokeRepeating ("CalculateFinalDamageValues", 0f,1f);
        CalculateFinalDamageValues();
        ApplyEffects.AddRange(GetComponents<ApplyBehaviour>());
    }
    
    public void Unsubscribe()
    {
        DelegatesEventsManager.OnUpgradeResearched -= this.ApplyUpgrade;
    }

	int sortByDistance(GameObject a, GameObject b)
	{
		float dstToA = Vector3.Distance (transform.position, a.transform.position);
		float dstToB = Vector3.Distance (transform.position, b.transform.position);
		return dstToA.CompareTo (dstToB);
	}

	void UpdateTarget()
	{
        if(turretType == TurretType.MultipleTarget)
		    MultiTargetScan ();
        else
		    SingleTargetScan ();
	}

	void MultiTargetScan()
	{
		if (turretType == TurretType.MultipleTarget) {

			GameObject[] multipleEnemies = GameObject.FindGameObjectsWithTag (enemyTag);
			List<GameObject> list = new List<GameObject> (multipleEnemies);
            targets = new List<GameObject> ();

			list.Sort (sortByDistance);
			for (int i = 0; i < Mathf.Min (maxTargets, list.Count); i++) {
				if (allowedTargets == AllowedTargets.Ground && list [i].GetComponent<EnemyStat> ().unitType != EnemyStat.UnitType.Ground)
					continue;
				if (allowedTargets == AllowedTargets.Air && list [i].GetComponent<EnemyStat> ().unitType != EnemyStat.UnitType.Flying)
                    continue;
				enemyDistance = list [i].transform.position - transform.position;
				if (Mathf.Abs(enemyDistance.x) <= range && Mathf.Abs(enemyDistance.z) <= range) {
					if (!targets.Contains (list [i]))
						targets.Add (list [i]);

				}
			}
		}
	}

	void SingleTargetScan()
	{
		if (turretType != TurretType.MultipleTarget) {
            if (target != null && EnableTargetFocus)
                if (Vector3.Distance(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z)) <= range)
                    return;
			GameObject[] enemies = GameObject.FindGameObjectsWithTag (enemyTag);
			float shortestDist = Mathf.Infinity;
			GameObject nearestEnemy = null;

			foreach (GameObject enemy in enemies) {
				if (allowedTargets == AllowedTargets.Ground && enemy.GetComponent<EnemyStat> ().unitType != EnemyStat.UnitType.Ground)
					continue;
				if (allowedTargets == AllowedTargets.Air && enemy.GetComponent<EnemyStat> ().unitType != EnemyStat.UnitType.Flying)
					continue;
				float distToEnemy = Vector3.Distance (transform.position, new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z));
				if (distToEnemy < shortestDist) {
					shortestDist = distToEnemy;
					nearestEnemy = enemy;
				}
			}

			if (nearestEnemy != null && shortestDist <= range) {
				target = nearestEnemy.transform;
                DeepWoundsBonus = new Damages();
			}
			if (target != null)
			if (Vector3.Distance (transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z)) > range)
				target = null;	
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (timeToShoot > 0)
			timeToShoot -= Time.deltaTime;
		if (target == null && turretType != TurretType.MultipleTarget)
			return;
		if (targets == null && turretType == TurretType.MultipleTarget)
			return;
		if (targets != null && turretType == TurretType.MultipleTarget)
			if (targets.Count == 0)
			return;
		
		if(looksAtTarget && turretType != TurretType.MultipleTarget)
			LookAtTarget ();
		Shoot ();
	}

	void LookAtTarget()
	{
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation (dir);
		Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
		transform.rotation = Quaternion.Euler(0f,rotation.y,0f);
	}






	public void ApplyUpgrade(UpgradeBase up, int lvToApply)
	{
		up.Action (this.gameObject, lvToApply);
        CalculateFinalDamageValues();
    }

	public void ApplyUpgradesOnStart()
	{
		foreach (UpgradeBase up in UpgradeManager.Instance.upgrades) {
			for (int i = 1; i <= up.uCurrentUpgradeLevel; i++)
			{
				up.Action (this.gameObject, i);
			}
		}
	}

    void Shoot()
    {
        if (timeToShoot > 0)
            return;
        SimpleProjectile Projectile = ProjectilePool.Instance.Activate(ProjectileID, ShootPoint.position, transform.rotation);
        if (ImpactEffect == null)
        {
            if (turretType != TurretType.MultipleTarget)
                Projectile.Initialize(this, ApplyEffects.ToArray(), turretType, target, projectileSpeed, finalDamageValues, AreaOfEffect, damageFallOff, guidedMissle, chainRadius, bounces, null,LineDamageRadius, DestroyProjectileDelay, DamageTimer);
            else
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].gameObject == null)
                        continue;
                    var multiProjectile = ProjectilePool.Instance.Activate(ProjectileID, ShootPoint.position, transform.rotation);
                    multiProjectile.Initialize(this, ApplyEffects.ToArray(), TurretType.SingleTarget, targets[i].transform, projectileSpeed, finalDamageValues, AreaOfEffect, damageFallOff, guidedMissle, chainRadius, bounces, null, LineDamageRadius, DestroyProjectileDelay, DamageTimer);
                    multiProjectile.gameObject.SetActive(true);
                }
        }

        if (ImpactEffect != null)
        {
            if (turretType != TurretType.MultipleTarget)
                Projectile.Initialize(this, ApplyEffects.ToArray(), turretType, target, projectileSpeed, finalDamageValues, AreaOfEffect, damageFallOff, guidedMissle, chainRadius, bounces, ImpactEffect, LineDamageRadius, DestroyProjectileDelay, DamageTimer);
            else
                for (int i = 0; i < targets.Count; i++)
                {
                    
                    if (targets[i].gameObject == null)
                        continue;
                    var multiProjectile = ProjectilePool.Instance.Activate(ProjectileID, ShootPoint.position, transform.rotation);
                    multiProjectile.Initialize(this, ApplyEffects.ToArray(), TurretType.SingleTarget, targets[i].transform, projectileSpeed, finalDamageValues, AreaOfEffect, damageFallOff, guidedMissle, chainRadius, bounces, ImpactEffect, LineDamageRadius, DestroyProjectileDelay, DamageTimer);
                    multiProjectile.gameObject.SetActive(true);
                }
        }
        Projectile.gameObject.SetActive(true);
        timeToShoot = finalDamageValues.fireRate;
    }


	public void CalculateFinalDamageValues()
	{
		finalDamageValues.FireDamage = 0;
		finalDamageValues.AirDamage = 0;
		finalDamageValues.WaterDamage = 0;
		finalDamageValues.EarthDamage = 0;
        finalDamageValues.DeathDamage = 0;
        finalDamageValues.LifeDamage = 0;
        finalDamageValues.fireRate = 0;
        finalDamageValues.fireRate = damages.fireRate * percFireRate * (1-berserkSpeedBonus);
        finalDamageValues.FireDamage = Mathf.RoundToInt(damages.FireDamage * (percentDamageBuff_Fire + percentGlobalDamageBuff));
		finalDamageValues.AirDamage = Mathf.RoundToInt(damages.AirDamage * (percentDamageBuff_Air + percentGlobalDamageBuff));
		finalDamageValues.WaterDamage = Mathf.RoundToInt(damages.WaterDamage * (percentDamageBuff_Water + percentGlobalDamageBuff));
		finalDamageValues.EarthDamage = Mathf.RoundToInt(damages.EarthDamage * (percentDamageBuff_Earth + percentGlobalDamageBuff));
        finalDamageValues.DeathDamage = Mathf.RoundToInt(damages.DeathDamage * (percentDamageBuff_Death + percentGlobalDamageBuff));
        finalDamageValues.LifeDamage = Mathf.RoundToInt(damages.LifeDamage * (percentDamageBuff_Life + percentGlobalDamageBuff));
    }

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range);
	}
}
