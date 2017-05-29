using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
public class EnemyStat : MonoBehaviour, ITakeDamage<Damages, Turret> {
	private GameObject gameManager;
    [SerializeField]
    private Transform canvasPrefab;
    private GameObject canvas;
	[Header("Enemy Attributes")]
	public int MaxHealth = 100;
	public int MaxShields = 0;
	public float baseMovementSpeed;
	public int GoldRewardForKill = 1;

    private OnDeath[] OnDeathAbilities;
    private OnSpawn[] OnSpawnAbilities;
    

	public enum UnitType
	{
		Ground,
		Flying
	}
	public UnitType unitType;

	public enum ArmorType
	{
		LightArmor,
		HeavyArmor,
        Unarmored,
        Elemental,
        Flying,
        Boss
	}
	public ArmorType armorType;

	[Header("Health Regeneration")]
	public bool isRegeneratingHealth = false;
	public int HealthRegenAmount = 0;
	public int HealthRegenRate = 0;


	[Header("No touchy touchy!")]
	[SerializeField]
	private int _hp;
	[SerializeField]
	private int _sp;

    [HideInInspector]
	public int currHP;
    [HideInInspector]
    public int currSP;
	//[HideInInspector]
	public float currMovementSpeed;
	private float _regRate;
	private RectTransform healthBar;
	private RectTransform shieldBar;
    private Image hpImg;
	private Text goldUI;


	[Header("Effect Stuff")]
	public float stunResistanceTime = 4f;
	private float _stunnedTimer;

	public bool isKnockbackResistant;
	public float knockbackResistanceTime = 7f;
	private float _knockbackTimer;

    [HideInInspector]
    public float SlowMovementPenalty;
    [HideInInspector]
    public float ChillMovementPenalty;
    [HideInInspector]
    public float AgonyModifier;
    [HideInInspector]
    public float PoisonRegenModifier;
    [HideInInspector]
    public float StunPenalty;
    [HideInInspector]
    public float BloodLustBonus;
    [HideInInspector]
    public bool Determined;
    [HideInInspector]
    public float DashBonus;
    [HideInInspector]
    public float HardenModifier;
    [HideInInspector]
    public Damages attument = new Damages();
    public void DamageTaken(Damages _damageTypes, Turret attacker)
	{
        if (Determined)
            return;

        Damages damageTypes = new Damages();
        int CalculatedDamage = 0;
        attacker.totalDamageSent += Mathf.RoundToInt((_damageTypes.FireDamage) + (_damageTypes.AirDamage) + (_damageTypes.EarthDamage) + (_damageTypes.WaterDamage) + (_damageTypes.DeathDamage) + (_damageTypes.LifeDamage));

        damageTypes.FireDamage = Mathf.RoundToInt(_damageTypes.FireDamage * (1 + AgonyModifier) * (1 - HardenModifier)) * (1 - attument.FireDamage);
        damageTypes.WaterDamage = Mathf.RoundToInt(_damageTypes.WaterDamage * (1 + AgonyModifier) * (1 - HardenModifier)) * (1 - attument.WaterDamage);
        damageTypes.EarthDamage = Mathf.RoundToInt(_damageTypes.EarthDamage * (1 + AgonyModifier) * (1 - HardenModifier)) * (1 - attument.EarthDamage);
        damageTypes.AirDamage = Mathf.RoundToInt(_damageTypes.AirDamage * (1 + AgonyModifier) * (1 - HardenModifier)) * (1 - attument.AirDamage);
        damageTypes.DeathDamage = Mathf.RoundToInt(_damageTypes.DeathDamage * (1 + AgonyModifier) * (1 - HardenModifier)) * (1 - attument.DeathDamage);
        damageTypes.LifeDamage = Mathf.RoundToInt(_damageTypes.LifeDamage * (1 + AgonyModifier) * (1 - HardenModifier)) * (1 - attument.LifeDamage);


        if (_sp > 0) {
			CalculatedDamage = Mathf.RoundToInt ((damageTypes.FireDamage * 0.5f) + (damageTypes.AirDamage * 0.5f) + (damageTypes.EarthDamage * 0.5f) + (damageTypes.WaterDamage * 2) + (damageTypes.DeathDamage) + (damageTypes.LifeDamage * .5f));
            _sp -= CalculatedDamage;
            attacker.totalShieldDamageDealt += CalculatedDamage;
        }
		else {
			if(armorType == ArmorType.LightArmor)
                CalculatedDamage = Mathf.RoundToInt((damageTypes.FireDamage * 1.25f) + (damageTypes.AirDamage * 0.75f) + (damageTypes.EarthDamage * 0.75f) + (damageTypes.WaterDamage * 0.75f) + (damageTypes.DeathDamage) + (damageTypes.LifeDamage * .75f));
			else if(armorType == ArmorType.HeavyArmor)
                CalculatedDamage = Mathf.RoundToInt((damageTypes.FireDamage * 0.5f) + (damageTypes.AirDamage * 0.5f) + (damageTypes.EarthDamage * 1.5f) + (damageTypes.WaterDamage * 0.75f) + (damageTypes.DeathDamage) + (damageTypes.LifeDamage * .5f));
            else if (armorType == ArmorType.Unarmored)
                CalculatedDamage = Mathf.RoundToInt((damageTypes.FireDamage * 1.25f) + (damageTypes.AirDamage * .5f) + (damageTypes.EarthDamage * .75f) + (damageTypes.WaterDamage * 0.5f) + (damageTypes.DeathDamage) + (damageTypes.LifeDamage * 1.5f));
            else if (armorType == ArmorType.Elemental)
                CalculatedDamage = Mathf.RoundToInt((damageTypes.FireDamage * 0.5f) + (damageTypes.AirDamage * 0.5f) + (damageTypes.EarthDamage * 0.5f) + (damageTypes.WaterDamage * 0.5f) + (damageTypes.DeathDamage) + (damageTypes.LifeDamage));
            else if (armorType == ArmorType.Flying)
                CalculatedDamage = Mathf.RoundToInt((damageTypes.FireDamage * 0.5f) + (damageTypes.AirDamage * 2f) + (damageTypes.EarthDamage * 0.5f) + (damageTypes.WaterDamage * 0.5f) + (damageTypes.DeathDamage) + (damageTypes.LifeDamage * .75f));
            else if (armorType == ArmorType.Boss)
                CalculatedDamage = Mathf.RoundToInt((damageTypes.FireDamage) + (damageTypes.AirDamage) + (damageTypes.EarthDamage) + (damageTypes.WaterDamage) + (damageTypes.DeathDamage) + (damageTypes.LifeDamage));

            _hp -= CalculatedDamage;
            attacker.totalDamageDealt += CalculatedDamage;
        }
        if (_sp < 0)
			_sp = 0;
        UpdateHealthBarColor();
    }
    public void DirectMagicDamageTaken(int damage, Turret attacker) // ! add attacker
    {
        _hp -= damage;
        attacker.totalDamageDealt += damage;
        attacker.totalDamageSent += damage;

        byte red = (byte)(255 - (currHP / MaxHealth) * 255);
        byte green = (byte)((currHP / MaxHealth) * 255);
        hpImg.color = new Color32(red, green, 0, 255);
        UpdateHealthBarColor();
    }
    public void MagicDamageTaken(int damage, Turret attacker) // ! add attacker
    {
        if (_sp > 0)
        {
            _sp -= damage;
            attacker.totalShieldDamageDealt += damage;
            attacker.totalDamageSent += damage;
        }
        else
        {
            _hp -= damage;
            attacker.totalDamageDealt += damage;
            attacker.totalDamageSent += damage;
        }
        if (_sp < 0)
            _sp = 0;

        UpdateHealthBarColor();
    }

    private void UpdateHealthBarColor()
    {
        byte red = 255;
        byte green = 255;
        if (((float)_hp / (float)MaxHealth) >= .5f)
        {
            red = (byte)(Mathf.Lerp(255, 0, (((float)_hp - ((float)MaxHealth - (float)_hp)) / (float)MaxHealth)));
            green = 255;
        }
        else
        {
            red = 255;
            green = (byte)(Mathf.Lerp(0, 255, ((float)_hp * 2 / (float)MaxHealth)));
        }
        hpImg.color = new Color32(red, green, 0, 255);
    }

    void Start()
	{
        if (canvas == null)
        {
            canvas = Instantiate(canvasPrefab.gameObject, transform.position, Quaternion.identity, this.transform);
            canvas.name = "Canvas";
        }
        hpImg = transform.FindChild("Canvas").FindChild("HPbar").GetComponent<Image>();
        OnDeathAbilities = GetComponents<OnDeath>();
        OnSpawnAbilities = GetComponents<OnSpawn>();
        gameManager = GameObject.Find ("_GameManager").gameObject;
		_hp = MaxHealth;
		_sp = MaxShields;
		healthBar = transform.FindChild ("Canvas").FindChild ("HPbar").GetComponent<RectTransform> ();
		shieldBar = transform.FindChild ("Canvas").FindChild ("SPbar").GetComponent<RectTransform> ();
		goldUI = transform.FindChild ("Canvas").FindChild ("goldUI").GetComponent<Text> ();
		currHP = MaxHealth;
		currSP = MaxShields;
		shieldBar.sizeDelta = new Vector2 (((float)_sp / (float)MaxShields), 0.2f);
		healthBar.sizeDelta = new Vector2 (((float)_hp / (float)MaxHealth), 0.2f);
		currMovementSpeed = baseMovementSpeed;
		_stunnedTimer = 0;
		_knockbackTimer = 0;
        BloodLustBonus = 0;
        DashBonus = 1;
        attument.FireDamage = 0;
        attument.WaterDamage = 0;
        attument.EarthDamage = 0;
        attument.AirDamage = 0;
        attument.DeathDamage = 0;
        attument.LifeDamage = 0;
        foreach (OnSpawn ability in OnSpawnAbilities)
        {
            ability.Activate();
        }
	}

    public void UpdateMovement()
    {
        currMovementSpeed = baseMovementSpeed * (1 - ChillMovementPenalty) * (1 - SlowMovementPenalty) * (1 - StunPenalty) * DashBonus + BloodLustBonus;
    }

    void FixedUpdate()
	{
        if (_hp <= 0)
        {
            Death();
        }
        if (_hp > MaxHealth)
            _hp = MaxHealth;

        if (_stunnedTimer > 0)
			_stunnedTimer -= Time.deltaTime;

		if (_knockbackTimer > 0)
			_knockbackTimer -= Time.deltaTime;

		if (GetComponent<UnityEngine.AI.NavMeshAgent> ().speed != currMovementSpeed)
			GetComponent<UnityEngine.AI.NavMeshAgent> ().speed = currMovementSpeed;

		if (currHP != _hp) {
            if (currHP <= 0)
            {
                currHP = 0;
                _hp = 0;
            }
            else
                currHP = _hp;
            float hpWidth = ((float)_hp / (float)MaxHealth);
			healthBar.sizeDelta = new Vector2 (hpWidth, 0.2f);
		}
		if (currSP != _sp) {
			currSP = _sp;
			float spWidth = ((float)_sp / (float)MaxShields);
			shieldBar.sizeDelta = new Vector2 (spWidth, 0.2f);
            
		}

		if (_sp > 0) {
			
		}



		if (isRegeneratingHealth) {
			RegenHealth ();
		}
	}

	void RegenHealth()
	{
		if (_hp < MaxHealth && HealthRegenRate > 0) {
			_regRate -= Time.deltaTime;
			if (_regRate <= 0) {
				_hp += (int)(HealthRegenAmount * (1-PoisonRegenModifier));
				_regRate = HealthRegenRate;
			}
		}
	}



	void Death()
	{
        foreach (OnDeath ability in OnDeathAbilities)
        {
            ability.Activate();
        }

        gameManager.GetComponent<WinLoseManager> ().RemoveEnemyCount ();
		PlayerManager.Gold += GoldRewardForKill;
		canvas.transform.SetParent(null, true);
		canvas.transform.position = this.transform.position;
        goldUI.fontStyle = FontStyle.Bold;
        goldUI.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 1);
        goldUI.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
		goldUI.fontSize = 50;
        goldUI.color = Color.yellow;
		goldUI.text = "+" + GoldRewardForKill.ToString ();
		Destroy (canvas, 0.75f);
		Destroy (gameObject);
	}

	public void Heal()
	{
		_hp = MaxHealth;
		_sp = MaxShields;
		GoldRewardForKill = 0;
	}

    public void addEffectImage(Image image)
    {
        //Instantiate()
        //canvas.transform.FindChild("Effects");
    }
}
