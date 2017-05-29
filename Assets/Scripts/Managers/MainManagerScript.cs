using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainManagerScript : MonoBehaviour {
	[Header("-----------------------Manager Setup")]
	public LayerMask layerMask;
	public LayerMask targetsWithStats;
    public Transform TowersHierarchy;
    public Transform FacilityHierarchy;
    public static MainManagerScript Instance;

    [Header("-----------------------Visuals")]
    [SerializeField]
	private Transform SelectionObj;
	[HideInInspector]
	public GameObject selected; //selection effect gameobject

	public Transform RangeVisual;
	private GameObject activeRangeVisual;

	public Transform hoverOverSellEffect;
	private GameObject overSellEffect;

	public Texture2D sellCursorTexture;
    public Texture2D mergeCursorTexture;


    [Header("-----------------------Buildings")]
	public Transform fireElement;
	public Transform waterElement;
	public Transform earthElement;
	public Transform airElement;
    public Transform deathElement;
    public Transform lifeElement;
    public Transform researchFacility;


    [SerializeField]
    public int[] TotalPrices;
    [SerializeField]
    public int[] UpgradePrices;

	private LayerMask towerLayer;
    private LayerMask economyLayer;
    private GameObject currentBuilding;


	private const int BaseTurretCost = 100;
    private const int BaseEconomyBuildingCost = 200;

    [HideInInspector]
	public GameObject selectedTower; // selected tower
    [HideInInspector]
    public Turret selectedTowerTurret; // <Turret> Script Component attached to the selected tower

	[Header("-----------------------UI Setup")]
	public Text towerNameText;
	public Transform TypeImage;
    public Transform tierTextUI;
    public Transform fireRateTextUI;
	public Material nodeMaterial;
	public Image UI_towerImage;
	public Text UI_FireDamageText;
	public Text UI_AirDamageText;
	public Text UI_WaterDamageText;
	public Text UI_EarthDamageText;
    public Text UI_DeathDamageText;
    public Text UI_LifeDamageText;
    public Text UI_MergeCost_num;
    public RectTransform UI_Upgrade;
    public RectTransform UI_Merge;
    public Image PassiveAbility01;

    //Merging stuff

	public Image SpecialAbility01;
	public bool sellingMode;
    public bool mergeSelectTowerMode;
    public bool lockSelecting;

    Ray ray;
    RaycastHit hit;
    RaycastHit hitForSell;
    RaycastHit hitForMerge;
    void Awake()
	{
		Application.targetFrameRate = 200;
	}

	// Use this for initialization
	void Start () {
		towerLayer = 1 << 11;
        economyLayer = 1 << 12;
		ResetUI ();
		sellingMode = false;
        mergeSelectTowerMode = false;
        lockSelecting = false;
        Instance = this;

    }
	
	// Update is called once per frame
	
    void Update () {
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);



		if (currentBuilding != null) {
			if (Input.GetMouseButtonDown (1)) {
                CancelBuild();
            }
		} else {
            if(!lockSelecting)
                BuildingClicked();
        }

		if (Physics.Raycast (ray, out hit, 1000, layerMask)) {
			BuildingManager ();
		}
        MergeSelectTower();
        SellTower();
		Debug.DrawRay (ray.origin, ray.direction * 1000);
	}



    void CancelBuild()
    {
        Destroy(currentBuilding);
        currentBuilding = null;
        nodeMaterial.color = new Color32(255, 255, 255, 0);
    }

    void BuildingClicked()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift) && !sellingMode)
        {

            if (Physics.Raycast(ray, out hit, 1000, targetsWithStats))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Tower"))
                {
                    if (selected != null)
                        if (hit.collider.gameObject == selectedTower)
                            return;
                    selectedTower = hit.collider.gameObject;
                    selectedTowerTurret = selectedTower.GetComponent<Turret>();
                    UI_SetTowerInfo();
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EconomyBuilding"))
                {
                    if (selected != null)
                        if (hit.collider.gameObject == selectedTower)
                            return;
                    ResetUI();
                    selectedTower = hit.collider.gameObject;
                    UI_SetEconomyInfo();
                }
            }
        }
    }

    



	void BuildingManager()
	{
		if (currentBuilding != null) {
			nodeMaterial.color = new Color32 (255,255,255,255);
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Node")) {
				currentBuilding.transform.position = hit.collider.transform.position;
				if (Input.GetMouseButtonDown (0)) {
					{
						if (hit.collider.gameObject.GetComponent<BuildSpot> ().occupied)
							return;
                        if (currentBuilding.layer == LayerMask.NameToLayer("Tower"))
                        {
                            PlayerManager.Gold -= BaseTurretCost;
                            currentBuilding.GetComponent<Turret>().enabled = true;
                            hit.collider.gameObject.GetComponent<BuildSpot>().setOccupied();
                            currentBuilding.GetComponent<Turret>().occupiedNode = hit.collider.gameObject;
                            if (Input.GetKey(KeyCode.LeftShift) && PlayerManager.Gold >= BaseTurretCost)
                            {
                                GameObject lastBuilding = currentBuilding;
                                currentBuilding.transform.parent = TowersHierarchy;
                                currentBuilding = (GameObject)Instantiate(lastBuilding, transform.position, Quaternion.identity);
                                currentBuilding.GetComponent<Turret>().enabled = false;
                                currentBuilding.name = lastBuilding.name;
                            }
                            else
                            {
                                currentBuilding.transform.parent = TowersHierarchy;
                                currentBuilding = null;
                                nodeMaterial.color = new Color32(255, 255, 255, 0);
                            }
                        }
                        else
                        {
                            PlayerManager.Gold -= BaseEconomyBuildingCost;
                            currentBuilding.GetComponent<ResearchFacility>().enabled = true;
                            hit.collider.gameObject.GetComponent<BuildSpot>().setOccupied();
                            currentBuilding.GetComponent<ResearchFacility>().occupiedNode = hit.collider.gameObject;
                            currentBuilding.transform.parent = TowersHierarchy;
                            currentBuilding = null;
                            nodeMaterial.color = new Color32(255, 255, 255, 0);
                        }

					}
				}
			}
		}

	}
    public void BuildDeathElement()
    {
        CancelSellMode();
        if (currentBuilding != null)
        {
            Destroy(currentBuilding);
            currentBuilding = null;
        }
        if (PlayerManager.Gold < BaseTurretCost)
            return;
        currentBuilding = (GameObject)Instantiate(deathElement.gameObject, Input.mousePosition, Quaternion.identity);
        currentBuilding.GetComponent<Turret>().enabled = false;
        currentBuilding.name = deathElement.name;
    }
    public void BuildLifeElement()
    {
        CancelSellMode();
        if (currentBuilding != null)
        {
            Destroy(currentBuilding);
            currentBuilding = null;
        }
        if (PlayerManager.Gold < BaseTurretCost)
            return;
        currentBuilding = (GameObject)Instantiate(lifeElement.gameObject, Input.mousePosition, Quaternion.identity);
        currentBuilding.GetComponent<Turret>().enabled = false;
        currentBuilding.name = lifeElement.name;
    }
    public void BuildFireElement()
	{
		CancelSellMode ();
		if(currentBuilding != null)
		{
			Destroy (currentBuilding);
			currentBuilding = null;
		}
		if (PlayerManager.Gold < BaseTurretCost)
			return;
		currentBuilding = (GameObject)Instantiate (fireElement.gameObject, Input.mousePosition, Quaternion.identity);
		currentBuilding.GetComponent<Turret> ().enabled = false;
		currentBuilding.name = fireElement.name;
	}

	public void BuildWaterElement()
	{
		CancelSellMode ();
		if(currentBuilding != null)
		{
			Destroy (currentBuilding);
			currentBuilding = null;
		}
		if (PlayerManager.Gold < BaseTurretCost)
			return;
		currentBuilding = (GameObject)Instantiate (waterElement.gameObject, Input.mousePosition, Quaternion.identity);
		currentBuilding.GetComponent<Turret> ().enabled = false;
		currentBuilding.name = waterElement.name;
	}

	public void BuildEarthElement()
	{
		CancelSellMode ();
		if(currentBuilding != null)
		{
			Destroy (currentBuilding);
			currentBuilding = null;
		}
		if (PlayerManager.Gold < BaseTurretCost)
			return;
		currentBuilding = (GameObject)Instantiate (earthElement.gameObject, Input.mousePosition, Quaternion.identity);
		currentBuilding.GetComponent<Turret> ().enabled = false;
		currentBuilding.name = earthElement.name;
	}

	public void BuildAirElement()
	{
		CancelSellMode ();
		if(currentBuilding != null)
		{
			Destroy (currentBuilding);
			currentBuilding = null;
		}
		if (PlayerManager.Gold < BaseTurretCost)
			return;
		currentBuilding = (GameObject)Instantiate (airElement.gameObject, Input.mousePosition, Quaternion.identity);
		currentBuilding.GetComponent<Turret> ().enabled = false;
		currentBuilding.name = airElement.name;
	}

    public void BuildResearchFacility()
    {
        CancelSellMode();
        if (currentBuilding != null)
        {
            Destroy(currentBuilding);
            currentBuilding = null;
        }
        if (PlayerManager.Gold < BaseEconomyBuildingCost)
            return;
        currentBuilding = (GameObject)Instantiate(researchFacility.gameObject, Input.mousePosition, Quaternion.identity);
        currentBuilding.GetComponent<ResearchFacility>().enabled = false;
        currentBuilding.name = researchFacility.name;
    }

    public void SelectSell()
	{
		Cursor.SetCursor (sellCursorTexture, Vector2.zero, CursorMode.Auto);
		sellingMode = true;
	}
	
	void SellTower()
	{
		if (!sellingMode)
			return;
		
		if (Physics.Raycast (ray, out hitForSell, 1000, towerLayer|economyLayer)) {
			if (overSellEffect == null)
				overSellEffect = (GameObject)Instantiate (hoverOverSellEffect.gameObject, hitForSell.transform.position, Quaternion.identity, hitForSell.collider.transform);
			else if (overSellEffect.transform.parent != hitForSell.transform) {
                overSellEffect.transform.position = hitForSell.transform.position;
                overSellEffect.transform.parent = hitForSell.transform;
			}


            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift) && sellingMode && hitForSell.collider.gameObject.layer == LayerMask.NameToLayer("Tower"))
            {
                int sellPayback = 0;
                switch (hitForSell.collider.gameObject.GetComponent<Turret>().Tier)
                {
                    case 1:
                        sellPayback = TotalPrices[0];
                        break;
                    case 2:
                        sellPayback = TotalPrices[1];
                        break;
                    case 3:
                        sellPayback = TotalPrices[2];
                        break;
                    case 4:
                        sellPayback = TotalPrices[3];
                        break;
                    case 5:
                        sellPayback = TotalPrices[4];
                        break;
                    case 6:
                        sellPayback = TotalPrices[5];
                        break;
                }
                hitForSell.collider.gameObject.GetComponent<Turret>().occupiedNode.GetComponent<BuildSpot>().occupied = false;
                hitForSell.collider.gameObject.GetComponent<Turret>().occupiedNode = null;
                hitForSell.collider.gameObject.GetComponent<Turret>().Unsubscribe();
                Destroy(hitForSell.collider.gameObject);
                PlayerManager.Gold += sellPayback;
                ResetUI();
            }
            else if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift) && sellingMode && hitForSell.collider.gameObject.layer == LayerMask.NameToLayer("EconomyBuilding"))
            {
                int sellPayback = 100;
                hitForSell.collider.gameObject.GetComponent<ResearchFacility>().occupiedNode = null;
                hitForSell.collider.gameObject.GetComponent<ResearchFacility>().Unsubscribe();
                Destroy(hitForSell.collider.gameObject);
                PlayerManager.Gold += sellPayback;
                ResetUI();
            }
		} else if (overSellEffect != null)
					Destroy (overSellEffect);
		if (Input.GetMouseButtonDown (1) && !Input.GetKey (KeyCode.LeftShift) && sellingMode) {
			CancelSellMode ();
		}
	}

	void CancelSellMode()
	{
			if (overSellEffect != null)
				Destroy (overSellEffect);
			Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
			sellingMode = false;
	}


	void UI_SetEconomyInfo()
	{
        if (selected == null)
            selected = (GameObject)Instantiate(SelectionObj.gameObject, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        selected.SetActive(false);
        selected.transform.position = selectedTower.transform.position + new Vector3(0, 0.5f, 0);
        if (!selected.activeInHierarchy)
            selected.SetActive(true);

        selected.transform.position = selectedTower.transform.position + new Vector3(0, 0.5f, 0);
        tierTextUI.GetComponent<Text> ().text = "Economy";
		fireRateTextUI.GetComponent<Text>().text = "x";
	}

	void UI_SetTowerInfo()
	{
        if (selected == null)
            selected = (GameObject)Instantiate(SelectionObj.gameObject, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        selected.SetActive(false);
        selected.transform.position = selectedTower.transform.position + new Vector3(0, 0.5f, 0);
        if (!selected.activeInHierarchy)
            selected.SetActive(true);
        
		UI_towerImage.sprite = selectedTowerTurret.Image;

		//UI_AbilityImage();
		//UI_SpecialAbilityImage ();
		switch (selectedTowerTurret.turretType) {
		case TurretType.SingleTarget:
			TypeImage.GetComponent<Image>().sprite = Resources.Load ("Images/SingleTarget", typeof(Sprite)) as Sprite;
			TypeImage.GetComponent<Tooltip> ().tooltipText = "Attacks a single target.";
			break;
		case TurretType.MultipleTarget:
			TypeImage.GetComponent<Image>().sprite = Resources.Load ("Images/MultiTarget", typeof(Sprite)) as Sprite;
			TypeImage.GetComponent<Tooltip> ().tooltipText = "Attacks " + selectedTowerTurret.maxTargets.ToString() + " targets at once.";
			break;
		case TurretType.AreaDamage:
			TypeImage.GetComponent<Image>().sprite = Resources.Load ("Images/AreaOfEffect", typeof(Sprite)) as Sprite;
			TypeImage.GetComponent<Tooltip> ().tooltipText = "Attacks in an area of " + selectedTowerTurret.AreaOfEffect.ToString() + " units with a value of " + selectedTowerTurret.damageFallOff.ToString() + " units of damage fall off.";
			break;
		case TurretType.ChainAttack:
			TypeImage.GetComponent<Image>().sprite = Resources.Load ("Images/ChainHit", typeof(Sprite)) as Sprite;
			TypeImage.GetComponent<Tooltip> ().tooltipText = "Attacks a target and bounces off to a maximum of " + selectedTowerTurret.bounces.ToString() + " targets";
			break;
		}
		UI_FireDamageText.text = (selectedTowerTurret.finalDamageValues.FireDamage).ToString ();
		UI_AirDamageText.text = (selectedTowerTurret.finalDamageValues.AirDamage).ToString ();
		UI_WaterDamageText.text = (selectedTowerTurret.finalDamageValues.WaterDamage).ToString ();
		UI_EarthDamageText.text = (selectedTowerTurret.finalDamageValues.EarthDamage).ToString ();
        UI_DeathDamageText.text = (selectedTowerTurret.finalDamageValues.DeathDamage).ToString();
        UI_LifeDamageText.text = (selectedTowerTurret.finalDamageValues.LifeDamage).ToString();

        fireRateTextUI.GetComponent<Text>().text = hit.collider.GetComponent<Turret>().finalDamageValues.fireRate.ToString();
        tierTextUI.GetComponent<Text>().text = hit.collider.GetComponent<Turret>().Tier.ToString();
		towerNameText.text = selectedTower.name;

        UI_MergeCost_num.text = UpgradePrices[selectedTowerTurret.Tier-1].ToString();




		if (activeRangeVisual != null)
			Destroy (activeRangeVisual);
		activeRangeVisual = (GameObject)Instantiate (RangeVisual.gameObject, hit.collider.transform.position, Quaternion.identity);
		var rangePart1 = activeRangeVisual.transform.GetChild (0).GetComponent<ParticleSystem> ().shape;
		rangePart1.radius = selectedTowerTurret.range;
		var rangePart2 = activeRangeVisual.transform.GetChild (1).GetComponent<ParticleSystem> ().shape;
		rangePart2.radius = selectedTowerTurret.range;
		var rangePart3 = activeRangeVisual.transform.GetChild (2).GetComponent<ParticleSystem> ().shape;
		rangePart3.radius = selectedTowerTurret.range;
		var rangePart4 = activeRangeVisual.transform.GetChild (3).GetComponent<ParticleSystem> ().shape;
		rangePart4.radius = selectedTowerTurret.range;

        //Merging/Upgrade panel
        if (selectedTowerTurret.upgradesTo == null)
        {
            UI_Merge.gameObject.SetActive(true);
            UI_Upgrade.gameObject.SetActive(false);
        }
        else
        {
            UI_Merge.gameObject.SetActive(false);
            UI_Upgrade.gameObject.SetActive(true);
        }
            

		/*bool dealsFire = false;
		bool dealsAir = false;
		bool dealsWater = false;
		bool dealsEarth = false;

		if (selectedTowerTurret.damages.FireDamage > 0)
			dealsFire = true;
		if (selectedTowerTurret.damages.AirDamage > 0)
			dealsAir = true;
		if (selectedTowerTurret.damages.WaterDamage > 0)
			dealsWater = true;
		if (selectedTowerTurret.damages.EarthDamage > 0)
			dealsEarth = true;

        ParticleSystem.MainModule partModule = activeRangeVisual.transform.GetChild(0).GetComponent<ParticleSystem>().main;

        if (dealsFire)
            activeRangeVisual.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = new Color32 (255, 125, 25, 255);
		else {
			if (dealsAir)
				activeRangeVisual.transform.GetChild (0).GetComponent<ParticleSystem> ().startColor = new Color32 (255, 255, 255, 255);
			else if (dealsWater)
				activeRangeVisual.transform.GetChild (0).GetComponent<ParticleSystem> ().startColor = new Color32 (0, 145, 255, 255);
			else if (dealsEarth)
				activeRangeVisual.transform.GetChild (0).GetComponent<ParticleSystem> ().startColor = new Color32 (0, 255, 0, 255);
		}


		if (dealsAir)
			activeRangeVisual.transform.GetChild (1).GetComponent<ParticleSystem> ().startColor = new Color32 (255, 255, 255, 255);
		else {
			if (dealsWater)
				activeRangeVisual.transform.GetChild (1).GetComponent<ParticleSystem> ().startColor = new Color32 (0, 145, 255, 255);
			else if (dealsEarth)
				activeRangeVisual.transform.GetChild (1).GetComponent<ParticleSystem> ().startColor = new Color32 (0, 255, 0, 255);
			else if (dealsFire)
				activeRangeVisual.transform.GetChild (1).GetComponent<ParticleSystem> ().startColor = new Color32 (255, 125, 25, 255);
		}


		if (dealsWater)
			activeRangeVisual.transform.GetChild (3).GetComponent<ParticleSystem> ().startColor = new Color32 (0, 145, 255, 255);
		else {
			if (dealsEarth)
				activeRangeVisual.transform.GetChild (3).GetComponent<ParticleSystem> ().startColor = new Color32 (0, 255, 0, 255);
			else if (dealsFire)
				activeRangeVisual.transform.GetChild (3).GetComponent<ParticleSystem> ().startColor = new Color32 (255, 125, 25, 255);
			else if (dealsAir)
				activeRangeVisual.transform.GetChild (3).GetComponent<ParticleSystem> ().startColor = new Color32 (255, 255, 255, 255);
		}
		
		if (dealsEarth)
			activeRangeVisual.transform.GetChild (2).GetComponent<ParticleSystem> ().startColor = new Color32 (0, 255, 0, 255);
		else {
			if (dealsFire)
				activeRangeVisual.transform.GetChild (2).GetComponent<ParticleSystem> ().startColor = new Color32 (255, 125, 25, 255);
			else if (dealsAir)
				activeRangeVisual.transform.GetChild (2).GetComponent<ParticleSystem> ().startColor = new Color32 (255, 255, 255, 255);
			else if (dealsWater)
				activeRangeVisual.transform.GetChild (2).GetComponent<ParticleSystem> ().startColor = new Color32 (0, 145, 255, 255);
		}*/
	}

	/*void UI_AbilityImage()
	{
		if (selectedTowerTurret.onHitEffects.Length == 0) {
			PassiveAbility01.sprite = null;
			return;
		}
		switch (selectedTowerTurret.onHitEffects[0]) {
		default:
			break;
		case EffectNames.Slow:
			PassiveAbility01.sprite = Resources.Load("Images/Effect_Slow", typeof(Sprite)) as Sprite;
			PassiveAbility01.GetComponent<Tooltip>().tooltipText = "<color=cyan>Slows</color> the enemies it hit by " + selectedTowerTurret.effectValues[0].ValueB.ToString() + "% for " + selectedTowerTurret.effectValues[0].ValueA.ToString() + " seconds.";
			break;
		case EffectNames.Burn:
			PassiveAbility01.sprite = Resources.Load("Images/Effect_Burn", typeof(Sprite)) as Sprite;
			PassiveAbility01.GetComponent<Tooltip>().tooltipText = "<color=red>Burns</color> the enemies it hit for " + selectedTowerTurret.effectValues[0].ValueA.ToString() + " seconds, dealing " + (selectedTowerTurret.effectValues[0].ValueC/10).ToString() + " damage every " + selectedTowerTurret.effectValues[0].ValueB.ToString() + " second.";
			break;
		case EffectNames.Poison:
			PassiveAbility01.sprite = Resources.Load("Images/Effect_Poison", typeof(Sprite)) as Sprite;
			PassiveAbility01.GetComponent<Tooltip>().tooltipText = "<color=green>Poisons</color> the enemies it hit for " + selectedTowerTurret.effectValues[0].ValueA.ToString() + " seconds, dealing " + (selectedTowerTurret.effectValues[0].ValueC/10).ToString() + " damage every " + selectedTowerTurret.effectValues[0].ValueB.ToString() + " second and reducing the health regeneration amount by " + selectedTowerTurret.effectValues[0].ValueD.ToString() + "%.";
			break;
		case EffectNames.Stun:
			PassiveAbility01.sprite = Resources.Load("Images/Effect_Stun", typeof(Sprite)) as Sprite;
			PassiveAbility01.GetComponent<Tooltip>().tooltipText = "<color=gray>Stuns</color> the enemies it hit for " + selectedTowerTurret.effectValues[0].ValueA.ToString() + " seconds.";
			break;
		case EffectNames.Knockback:
			PassiveAbility01.sprite = Resources.Load("Images/Effect_Knockup", typeof(Sprite)) as Sprite;
			break;
		}
	}

	void UI_SpecialAbilityImage()
	{
		if (selectedTowerTurret.specialEffects.Length == 0) {
			SpecialAbility01.sprite = null;
			return;
		}

		switch (selectedTowerTurret.specialEffects[0].gameEffect.name) {
		default:
			break;
		case "BurningGround":
			SpecialAbility01.sprite = Resources.Load ("Images/Effect_Burn", typeof(Sprite)) as Sprite;
			SpecialAbility01.GetComponent<Tooltip> ().tooltipText = "Deals <color=red>" + (selectedTowerTurret.specialEffects [0].damage.FireDamage / 10).ToString () + "</color>/" + (selectedTowerTurret.specialEffects [0].damage.AirDamage / 10).ToString () + "/<color=blue>" + (selectedTowerTurret.specialEffects [0].damage.WaterDamage / 10).ToString () + "</color>/<color=brown>" + (selectedTowerTurret.specialEffects [0].damage.EarthDamage / 10).ToString () + "</color> damage  per " + selectedTowerTurret.specialEffects [0].tickFrequency.ToString () + " second over " + selectedTowerTurret.specialEffects [0].duration.ToString () + " seconds in an area of " + selectedTowerTurret.specialEffects [0].radius.ToString () + ".";
				UI_SpecialAbilitySpecialEffect ();
			break;
		case "IcyGround":
			SpecialAbility01.sprite = Resources.Load ("Images/Effect_Slow", typeof(Sprite)) as Sprite;
			SpecialAbility01.GetComponent<Tooltip> ().tooltipText = "Deals <color=red>" + (selectedTowerTurret.specialEffects [0].damage.FireDamage / 10).ToString () + "</color>/" + (selectedTowerTurret.specialEffects [0].damage.AirDamage / 10).ToString () + "/<color=blue>" + (selectedTowerTurret.specialEffects [0].damage.WaterDamage / 10).ToString () + "</color>/<color=brown>" + (selectedTowerTurret.specialEffects [0].damage.EarthDamage / 10).ToString () + "</color> damage  per " + selectedTowerTurret.specialEffects [0].tickFrequency.ToString () + " second over " + selectedTowerTurret.specialEffects [0].duration.ToString () + " seconds in an area of " + selectedTowerTurret.specialEffects [0].radius.ToString () + ".";
				UI_SpecialAbilitySpecialEffect ();
			break;
		case "PoisonGround":
			SpecialAbility01.sprite = Resources.Load("Images/Effect_Poison", typeof(Sprite)) as Sprite;
			SpecialAbility01.GetComponent<Tooltip> ().tooltipText = "Deals <color=red>" + (selectedTowerTurret.specialEffects [0].damage.FireDamage / 10).ToString () + "</color>/" + (selectedTowerTurret.specialEffects [0].damage.AirDamage / 10).ToString () + "/<color=blue>" + (selectedTowerTurret.specialEffects [0].damage.WaterDamage / 10).ToString () + "</color>/<color=brown>" + (selectedTowerTurret.specialEffects [0].damage.EarthDamage / 10).ToString () + "</color> damage  per " + selectedTowerTurret.specialEffects [0].tickFrequency.ToString () + " second over " + selectedTowerTurret.specialEffects [0].duration.ToString () + " seconds in an area of " + selectedTowerTurret.specialEffects [0].radius.ToString () + ".";
				UI_SpecialAbilitySpecialEffect ();
			break;
		}
	}

	void UI_SpecialAbilitySpecialEffect()
	{
		if (selectedTowerTurret.specialEffects [0].effects.Length > 0) {
			SpecialAbility01.GetComponent<Tooltip> ().tooltipText += "";
			return;
		}
		switch (selectedTowerTurret.specialEffects [0].effects[0]) {
		default:
			break;
		case EffectNames.Slow:
			SpecialAbility01.GetComponent<Tooltip> ().tooltipText += " Every tick also <color=cyan>slows</color> the enemies it hit by " + selectedTowerTurret.specialEffects[0].effectValues[0].ValueB.ToString() + "% for " + selectedTowerTurret.specialEffects[0].effectValues[0].ValueA.ToString() + " seconds.";
			break;
		case EffectNames.Burn:
			SpecialAbility01.GetComponent<Tooltip> ().tooltipText += " Every tick also <color=red>burns</color> the enemies it hit for " + selectedTowerTurret.specialEffects[0].effectValues[0].ValueA.ToString() + " seconds, dealing " + (selectedTowerTurret.specialEffects[0].effectValues[0].ValueC/10).ToString() + " damage every " + selectedTowerTurret.specialEffects[0].effectValues[0].ValueB.ToString() + " second.";
			break;
		case EffectNames.Poison:
			SpecialAbility01.GetComponent<Tooltip> ().tooltipText += " Every tick also <color=green>poisons</color> the enemies it hit for " + selectedTowerTurret.specialEffects[0].effectValues[0].ValueA.ToString() + " seconds, dealing " + (selectedTowerTurret.specialEffects[0].effectValues[0].ValueC/10).ToString() + " damage every " + selectedTowerTurret.specialEffects[0].effectValues[0].ValueB.ToString() + " second and reducing the health regeneration amount by " + selectedTowerTurret.specialEffects[0].effectValues[0].ValueD.ToString() + "%.";
			break;
		case EffectNames.Stun:
			SpecialAbility01.GetComponent<Tooltip> ().tooltipText += " Every tick also <color=gray>stuns</color> the enemies it hit for " + selectedTowerTurret.specialEffects[0].effectValues[0].ValueA.ToString() + " seconds.";
			break;
		}
	}*/



	void ResetUI()
	{
        if(selected==null)
            selected = (GameObject)Instantiate(SelectionObj.gameObject, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        if (selected.activeInHierarchy)
            selected.SetActive(false);
		if (activeRangeVisual != null)
			Destroy (activeRangeVisual);
		UI_towerImage.sprite = null;
		TypeImage.GetComponent<Image>().sprite = null;
		UI_FireDamageText.text = "";
		UI_AirDamageText.text = "";
		UI_WaterDamageText.text = "";
		UI_EarthDamageText.text = "";
        UI_DeathDamageText.text = "";
        UI_LifeDamageText.text = "";
		towerNameText.text = "";
        TypeImage.GetComponent<Tooltip>().tooltipText = "";
		PassiveAbility01.GetComponent<Tooltip> ().tooltipText = "";
		SpecialAbility01.GetComponent<Tooltip> ().tooltipText = "";
        fireRateTextUI.GetComponent<Text>().text = "";
        tierTextUI.GetComponent<Text>().text = "";
		selectedTower = null;
		PassiveAbility01.sprite = null;
        UI_Merge.gameObject.SetActive(false);
        UI_Upgrade.gameObject.SetActive(false);
        UI_MergeCost_num.text = "";
    }

    #region UpgradeAndMerge

    public void Upgrade()
    {
        CancelSellMode();
        if (PlayerManager.Gold < UpgradePrices[selectedTowerTurret.Tier - 1])
            return;
        if (selectedTower == null)
            return;
        Turret turret = selectedTowerTurret;
        if (turret.upgradesTo == null)
            return;
        PlayerManager.Gold -= UpgradePrices[turret.Tier-1];
        GameObject newTurret = Instantiate(turret.upgradesTo.gameObject, turret.transform.position, Quaternion.identity, TowersHierarchy) as GameObject;
        newTurret.GetComponent<Turret>().occupiedNode = turret.occupiedNode;
        turret.Unsubscribe();
        Destroy(turret.gameObject);
        ResetUI();

    }

    public void NewMerge()
    {
        CancelSellMode();
        if (PlayerManager.Gold < UpgradePrices[selectedTowerTurret.Tier - 1])
            return;
        if (selectedTower == null)
            return;
        if (selectedTowerTurret.Merging.Count <= 0)
            return;
        mergeSelectTowerMode = true;
        lockSelecting = true;
        Cursor.SetCursor(mergeCursorTexture, Vector2.zero, CursorMode.Auto);
        //Change Cursor

        //Hilight Turrets Available for merge
        for (int i = 0; i < selectedTowerTurret.Merging.Count; i++)
        {
            
            switch (selectedTowerTurret.Merging[i].MergesWith.gameObject.tag)
            {
                case "EnchancedAir":
                    HighlightEnchancedTowers.Instance.Hilight("AIR", selectedTower);
                    break;
                case "EnchancedEarth":
                    HighlightEnchancedTowers.Instance.Hilight("EARTH", selectedTower);
                    break;
                case "EnchancedFire":
                    HighlightEnchancedTowers.Instance.Hilight("FIRE", selectedTower);
                    break;
                case "EnchancedWater":
                    HighlightEnchancedTowers.Instance.Hilight("WATER", selectedTower);
                    break;
                case "EnchancedDeath":
                    HighlightEnchancedTowers.Instance.Hilight("DEATH", selectedTower);
                    break;
                case "EnchancedLife":
                    HighlightEnchancedTowers.Instance.Hilight("LIFE", selectedTower);
                    break;
            }
            
        }
            
    }

    GameObject mergeTower = null;
    GameObject mergingResult=null;
    void MergeSelectTower()
    {
        if (!mergeSelectTowerMode)
            return;
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            mergeSelectTowerMode = false;
            lockSelecting = false;
            HighlightEnchancedTowers.Instance.Unhilight();
        }
        if (Physics.Raycast(ray, out hitForMerge, 1000, towerLayer))
            if (Input.GetMouseButtonDown(0) && hitForMerge.collider.gameObject.layer == LayerMask.NameToLayer("Tower"))
        { 
                for (int i = 0; i < selectedTowerTurret.Merging.Count; i++)
            {
                if (selectedTowerTurret.Merging[i].MergesWith == null)
                    continue;
                if (hitForMerge.collider.gameObject == selectedTower)
                    continue;
                if (hitForMerge.collider.gameObject.tag != selectedTowerTurret.Merging[i].MergesWith.tag)
                    continue;
                mergeTower = hitForMerge.collider.gameObject;
                mergingResult = selectedTowerTurret.Merging[i].Result.gameObject;
             }
        }
        if (mergeTower == null)
            return;
        PlayerManager.Gold -= UpgradePrices[selectedTowerTurret.Tier];
        GameObject newTurret = Instantiate(mergingResult, selectedTowerTurret.transform.position, Quaternion.identity, TowersHierarchy) as GameObject;
        newTurret.GetComponent<Turret>().occupiedNode = selectedTowerTurret.occupiedNode;
        mergeTower.GetComponent<Turret>().occupiedNode.GetComponent<BuildSpot>().occupied = false;
        selectedTowerTurret.Unsubscribe();
        mergeTower.GetComponent<Turret>().Unsubscribe();
        if(selectedTower.GetComponent<EnchancedTower>())
            selectedTower.GetComponent<EnchancedTower>().RemoveFromList();
        if (mergeTower.GetComponent<EnchancedTower>())
            mergeTower.GetComponent<EnchancedTower>().RemoveFromList();
        Destroy(selectedTowerTurret.gameObject);
        Destroy(mergeTower);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        ResetUI();
        mergeSelectTowerMode = false;
        lockSelecting = false;
        HighlightEnchancedTowers.Instance.Unhilight();
    }
    #endregion
}
