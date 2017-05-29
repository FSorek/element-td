using UnityEngine;
using System.Collections;
public class SimpleProjectile : MonoBehaviour {

    private ApplyBehaviour[] _ApplyEffects;

    private Turret _attacker;

    private Transform _target;
	private float _speed;
	private GameObject _impactEffect;
	private int _AoeRadius;
	private float _AoeDamageFalloff;
	private Vector3 targetPos;
	private bool _isGuided;

	private Damages _damageTypes;

	private float _chainRadius;
	private int _bounces;

	private LayerMask enemyMask;


	private Transform previousTarget = null;
	private Transform prevPreviousTarget = null;
	private TurretType _projectileType;
    
	private Transform particles;

    private float Line_damageTickTimer;
    private float Line_damageTickCountdown;
    private float Line_DestroyDelay;
    private int Line_radius;

    void Start()
	{
		if (transform.childCount > 0)
            if(transform.GetChild(0).GetComponent<ParticleSystem>())
			    particles = transform.GetChild (0);
	}

    public void Initialize(Turret attacker, ApplyBehaviour[] effects, TurretType projectileType, Transform target, float speed, Damages damageTypes, int AoeRadius, float AoeDamageFalloff, bool isGuided, float chainRadius, int bounces, GameObject impactEffect, int LineRadius, float LineDestroyDelay, float LineTickTime)
    {
        _attacker = attacker;
        _ApplyEffects = effects;
        _projectileType = projectileType;
        _target = target;
        _speed = speed;
        _damageTypes = damageTypes;
        _impactEffect = impactEffect;
        _AoeRadius = AoeRadius;
        _AoeDamageFalloff = AoeDamageFalloff;
        targetPos = target.position;
        _isGuided = isGuided;
        _chainRadius = chainRadius;
        _bounces = bounces;
        Line_damageTickTimer = LineTickTime;
        Line_DestroyDelay = LineDestroyDelay;
        Line_radius = LineRadius;
    }

	void Update () {
		if (_target == null && _projectileType != TurretType.AreaDamage && _projectileType != TurretType.ChainAttack && _projectileType != TurretType.Line) {
			DestroyProjectile ();
			return;
		}
		switch (_projectileType) {
		case TurretType.SingleTarget:
			singleTargetMissle ();
			break;
		case TurretType.AreaDamage:
			aoeGuided ();
			break;
		case TurretType.ChainAttack:
			chainMissle ();
			break;
        case TurretType.Line:
            LineAttack();
            break;
        }
	}


    void LineAttack()
    {
        Vector3 dir = targetPos - transform.position;
        float distanceThisFrame = _speed * Time.deltaTime;
        if (dir.magnitude <= distanceThisFrame)
        {
            if (!IsInvoking("DestroyProjectile"))
                Invoke("DestroyProjectile", Line_DestroyDelay);
        }
        Line_damageTickCountdown -= Time.deltaTime;
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_target.transform.position);
        if (Line_damageTickCountdown > 0)
            return;
        
        Collider[] enemies = Physics.OverlapCapsule(new Vector3(transform.position.x, transform.position.y-10, transform.position.z), new Vector3(transform.position.x, transform.position.y+10, transform.position.z), Line_radius, 1 << 8);
        foreach (Collider enemy in enemies)
        {
            enemy.GetComponent<EnemyStat>().DamageTaken(_damageTypes, _attacker);
        }
        Line_damageTickCountdown = Line_damageTickTimer;
    }

	void singleTargetMissle()
	{
		Vector3 dir = _target.position - transform.position;
		float distanceThisFrame = _speed * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame) {

			_target.GetComponent<EnemyStat> ().DamageTaken (_damageTypes, _attacker);
			if (_ApplyEffects.Length > 0) {
				foreach(ApplyBehaviour effect in _ApplyEffects)
                {
                    effect.Apply(_target.gameObject);
                }
			}
			DestroyProjectile ();
			return;
		}
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_target.transform.position);
    }


	void chainMissle()
	{
		if (_target != null)
			targetPos = _target.position;

		Vector3 dir = targetPos - transform.position;
		float distanceThisFrame = _speed * Time.deltaTime;


		if (dir.magnitude <= distanceThisFrame) {
			if (_target != null) {
				if(previousTarget != null)
					prevPreviousTarget = previousTarget;
				previousTarget = _target;
				_target.GetComponent<EnemyStat> ().DamageTaken (_damageTypes, _attacker);
                if (_ApplyEffects.Length > 0)
                {
                    foreach (ApplyBehaviour effect in _ApplyEffects)
                    {
                        effect.Apply(_target.gameObject);
                    }
                }
				_target = null;
			}

			HitEffect ();
			if (_bounces <= 0) {
				DestroyProjectile ();
			}

			if (_target == null) {
				
				_target = chainSearch (previousTarget, prevPreviousTarget);
			}
			if (_target == null) {
				DestroyProjectile ();
				return;
			}
			_bounces--;
			if (_target != null)
				targetPos = _target.position;
			dir = targetPos - transform.position;
			distanceThisFrame = _speed * Time.deltaTime;
		}

		transform.Translate (dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt (targetPos);

	}

	Transform chainSearch(Transform prevTarget, Transform ppTarget)
	{
		Collider[] enemies = Physics.OverlapSphere (transform.position, _chainRadius, 1<<8);
		float shortestDist = Mathf.Infinity;
		GameObject nextTarget = null;
		if (enemies.Length <= 1) {
			return null;
		}

		foreach (Collider enemy in enemies) {
			float distToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
			if (distToEnemy < shortestDist && enemy.transform != prevTarget && enemy.transform != ppTarget) {
				shortestDist = distToEnemy;
				nextTarget = enemy.gameObject;

			}
		}
		if (nextTarget == null)
			return null;
		return nextTarget.transform;
	}


	void aoeGuided()
	{
		if (_target != null && _isGuided)
			targetPos = _target.position;
		
		Vector3 dir = targetPos - transform.position;
		float distanceThisFrame = _speed * Time.deltaTime;
		if (dir.magnitude <= distanceThisFrame) {
			Collider[] enemies = Physics.OverlapSphere(transform.position, _AoeRadius, 1 << 8);
			foreach (Collider enemy in enemies) {
				    float distToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
					Damages _newDmg = new Damages();
                    _newDmg = CalculateNewDamage (_newDmg,distToEnemy);
					enemy.GetComponent<EnemyStat> ().DamageTaken (_newDmg, _attacker);

                    if (_ApplyEffects.Length > 0)
                    {
                        foreach (ApplyBehaviour effect in _ApplyEffects)
                        {
                            effect.Apply(enemy.gameObject);
                        }
                    }
			}
            DestroyProjectile ();
			return;
		}
		transform.Translate (dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt (targetPos);
	}

	Damages CalculateNewDamage(Damages _calculatedDmg, float distToEnemy)
	{
		_calculatedDmg.WaterDamage = _damageTypes.WaterDamage;
		_calculatedDmg.FireDamage = _damageTypes.FireDamage;
		_calculatedDmg.AirDamage = _damageTypes.AirDamage;
		_calculatedDmg.EarthDamage = _damageTypes.EarthDamage;
        _calculatedDmg.DeathDamage = _damageTypes.DeathDamage;
        _calculatedDmg.LifeDamage = _damageTypes.LifeDamage;
        _calculatedDmg.FireDamage = Mathf.RoundToInt (_damageTypes.FireDamage - ((float)_damageTypes.FireDamage * _AoeDamageFalloff * (distToEnemy / _AoeRadius)));
        _calculatedDmg.AirDamage = Mathf.RoundToInt (_damageTypes.AirDamage - ((float)_damageTypes.AirDamage * _AoeDamageFalloff * (distToEnemy / _AoeRadius)));
        _calculatedDmg.WaterDamage = Mathf.RoundToInt (_damageTypes.WaterDamage - ((float)_damageTypes.WaterDamage * _AoeDamageFalloff * (distToEnemy / _AoeRadius)));
        _calculatedDmg.EarthDamage = Mathf.RoundToInt (_damageTypes.EarthDamage - ((float)_damageTypes.EarthDamage * _AoeDamageFalloff * (distToEnemy / _AoeRadius)));
        _calculatedDmg.DeathDamage = Mathf.RoundToInt((float)_damageTypes.DeathDamage - ((float)_damageTypes.DeathDamage * _AoeDamageFalloff * (distToEnemy / _AoeRadius)));
        _calculatedDmg.LifeDamage = Mathf.RoundToInt(_damageTypes.LifeDamage - ((float)_damageTypes.LifeDamage * _AoeDamageFalloff * (distToEnemy / _AoeRadius)));
        return _calculatedDmg;
    }


	void DestroyProjectile()
	{
		HitEffect ();
         if (particles != null) {
            particles.GetComponent<ParticleSystem>().Stop();
         }
        gameObject.GetComponent<SimpleProjectile>().enabled = false;
        Invoke("PoolProjectile", 2f);
	}


    void PoolProjectile()
    {
        gameObject.SetActive(false);
        if (particles != null)
        {
            particles.GetComponent<ParticleSystem>().Play();
        }
        gameObject.GetComponent<SimpleProjectile>().enabled = true;
    }

	void HitEffect ()
	{
		if (_speed >= 500 && _target != null) {
			GameObject ImpactEffect = (GameObject)Instantiate (_impactEffect, _target.position, Quaternion.identity);
			Destroy (ImpactEffect, 2f);
			return;
		} else
		if (_impactEffect != null) {
			GameObject ImpactEffect = (GameObject)Instantiate (_impactEffect, transform.position, Quaternion.identity);
			Destroy (ImpactEffect, 2f);
		}
	}
}
