using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Turret))]
public class CustomTurretInspector : Editor {
    Turret turret;
    private List<string> projectileNames = new List<string>();
    private int[] projectileIDValues;
    private int projectileIndex;

    private string[] Tiers = {"Tier 1", "Tier 2", "Tier 3", "Tier 4", "Tier 5", "Tier 6"};
    private int[] simpleValues = new int[6] { 1, 2, 3, 4, 5, 6 };

    private bool instantDamage;
    private bool extendedSetup;
    private bool Merge;

    private List<ApplyBehaviour> Abilities = new List<ApplyBehaviour>();
    private bool AbilityWindow;

    private void OnEnable()
    {
        turret = (Turret)target;
        projectileIDValues = new int[GameObject.FindObjectOfType<ProjectilePool>().Objects.Length];
        for (int i = 0; i < GameObject.FindObjectOfType<ProjectilePool>().Objects.Length; i++)
        {
            projectileNames.Add(GameObject.FindObjectOfType<ProjectilePool>().Objects[i].Object.name);
            projectileIDValues[i] = i;
        }
        Abilities.AddRange(turret.GetComponents<ApplyBehaviour>());
    }



    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Basic Visual Setup", EditorStyles.boldLabel);
        turret.ProjectileID = EditorGUILayout.IntPopup("Projectile Model", turret.ProjectileID, projectileNames.ToArray(), projectileIDValues, GUILayout.Width(300));
        turret.ImpactEffect = (GameObject)EditorGUILayout.ObjectField("Impact Effect", turret.ImpactEffect, typeof(GameObject), false, GUILayout.Width(300));
        GUILayout.Space(10);

        EditorGUILayout.LabelField("General Setup", EditorStyles.boldLabel);
        turret.Tier = EditorGUILayout.IntPopup(turret.Tier, Tiers, simpleValues, GUILayout.Width(60));
        EditorGUILayout.BeginHorizontal();
        turret.looksAtTarget = EditorGUILayout.ToggleLeft("Face Target", turret.looksAtTarget, GUILayout.Width(100));
        turret.EnableTargetFocus = EditorGUILayout.ToggleLeft("Focus Target", turret.EnableTargetFocus, GUILayout.Width(100));
        instantDamage = EditorGUILayout.ToggleLeft("Instant Damage", instantDamage, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        if (instantDamage)
        {
            turret.projectileSpeed = 500;
            EditorGUILayout.LabelField("Projectile Speed has been adjusted.");
        }
        else turret.projectileSpeed = EditorGUILayout.IntField("Projectile Speed", (int)turret.projectileSpeed, GUILayout.Width(300));
        turret.range = EditorGUILayout.IntField("Tower Range", (int)turret.range, GUILayout.Width(300));
        turret.allowedTargets = (AllowedTargets)EditorGUILayout.EnumPopup("Allowed Targets", turret.allowedTargets, GUILayout.Width(300));
        switch (turret.turretType = (TurretType)EditorGUILayout.EnumPopup("Turret Type", turret.turretType, GUILayout.Width(300)))
        {
            case TurretType.SingleTarget:
                GUILayout.Space(10);
                EditorGUILayout.LabelField("No additional setup required.");
                break;
            case TurretType.AreaDamage:
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                turret.AreaOfEffect = EditorGUILayout.IntField("Area of Effect Radius", turret.AreaOfEffect, GUILayout.Width(200));
                turret.guidedMissle = EditorGUILayout.ToggleLeft("Guided Missle", turret.guidedMissle, GUILayout.Width(100));
                GUILayout.EndHorizontal();
                turret.damageFallOff = EditorGUILayout.Slider("Damage Falloff", turret.damageFallOff, 0, 1f, GUILayout.Width(300));
                break;
            case TurretType.ChainAttack:
                GUILayout.Space(10);
                turret.chainRadius = EditorGUILayout.IntField("Bounce Range", (int)turret.chainRadius, GUILayout.Width(300));
                turret.bounces = EditorGUILayout.IntSlider("Number of Bounces", turret.bounces, 0, 10, GUILayout.Width(300));
                break;
            case TurretType.MultipleTarget:
                GUILayout.Space(10);
                turret.maxTargets = EditorGUILayout.IntField("Maximum Number of Targets", turret.maxTargets, GUILayout.Width(300));
                break;
            case TurretType.Line:
                GUILayout.Space(10);
                turret.DamageTimer = EditorGUILayout.Slider("Time until damage tick", turret.DamageTimer, .1f, 5f, GUILayout.Width(300));
                turret.DestroyProjectileDelay = EditorGUILayout.Slider("Destruction Delay", turret.DestroyProjectileDelay, 0, 10f, GUILayout.Width(300));
                turret.LineDamageRadius = EditorGUILayout.IntSlider("Damage Radius", turret.LineDamageRadius, 1, 15, GUILayout.Width(300));
                break;
        }
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Damage Values", EditorStyles.boldLabel);
        turret.damages.FireDamage = EditorGUILayout.IntField("Fire Damage", turret.damages.FireDamage, GUILayout.Width(300));
        turret.damages.WaterDamage = EditorGUILayout.IntField("Water Damage", turret.damages.WaterDamage, GUILayout.Width(300));
        turret.damages.AirDamage = EditorGUILayout.IntField("Air Damage", turret.damages.AirDamage, GUILayout.Width(300));
        turret.damages.EarthDamage = EditorGUILayout.IntField("Earth Damage", turret.damages.EarthDamage, GUILayout.Width(300));
        turret.damages.DeathDamage = EditorGUILayout.IntField("Death Damage", turret.damages.DeathDamage, GUILayout.Width(300));
        turret.damages.LifeDamage = EditorGUILayout.IntField("Life Damage", turret.damages.LifeDamage, GUILayout.Width(300));
        GUILayout.Space(10);
        turret.damages.fireRate = EditorGUILayout.FloatField("Fire Rate", turret.damages.fireRate, GUILayout.Width(300));
        GUILayout.Space(20);

        /*EditorGUILayout.LabelField("Abilities", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical("box");
        
        for(int i = 0; i < Abilities.Count; i++)
        {

            Abilities[i].abilityName = EditorGUILayout.TextField("Ability Name", Abilities[i].abilityName);
            Abilities[i].toolTip = EditorGUILayout.TextField("Tooltip", Abilities[i].toolTip);
            Abilities[i].Image = (Sprite)EditorGUILayout.ObjectField("Image", Abilities[i].Image, typeof(Sprite), false);

            if (turret.GetComponent<ApplyChill>())
            {
                turret.GetComponent<ApplyChill>().duration = EditorGUILayout.FloatField("Duration", turret.GetComponent<ApplyChill>().duration);
                turret.GetComponent<ApplyChill>().slowPercentage = EditorGUILayout.Slider("Slow Percentage", turret.GetComponent<ApplyChill>().slowPercentage, 0, 1f);
            }
            if (turret.GetComponent<ApplyStun>())
            {
                turret.GetComponent<ApplyStun>().d
            }
            if (turret.GetComponent<ApplyBurning>())
            {

            }
            if (turret.GetComponent<ApplyPoison>())
            {

            }
            if (turret.GetComponent<ApplyAgony>())
            {

            }
            if (turret.GetComponent<ApplyBleed>())
            {

            }
            if (turret.GetComponent<ApplySlow>())
            {
                turret.GetComponent<ApplySlow>().slowReductionTime = EditorGUILayout.FloatField("Slow Reduction Time", turret.GetComponent<ApplySlow>().slowReductionTime);
                turret.GetComponent<ApplySlow>().duration = EditorGUILayout.FloatField("Duration", turret.GetComponent<ApplySlow>().duration);
                turret.GetComponent<ApplySlow>().slowPercentage = EditorGUILayout.Slider("Slow Percentage", turret.GetComponent<ApplySlow>().slowPercentage, 0, 1f);
                turret.GetComponent<ApplySlow>().slowReductionAmount = EditorGUILayout.Slider("Slow Reduction Amount", turret.GetComponent<ApplySlow>().slowReductionAmount, 0, 1f);
            }
            EditorGUILayout.LabelField("___________________________________________________________________");
            GUILayout.Space(10);
        }

        if(GUILayout.Button("Add New Ability",EditorStyles.miniButton))
        {
            AbilityWindow = !AbilityWindow;
        }
        EditorGUILayout.EndVertical();
        

        EditorGUILayout.Space();


        EditorGUILayout.BeginVertical("box", GUILayout.Width(300));

        if (AbilityWindow)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Add new Ability", EditorStyles.boldLabel, GUILayout.Width(270));
            if (GUILayout.Button("X", EditorStyles.miniButtonRight, GUILayout.Width(30)))
            {
                AbilityWindow = false;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+", EditorStyles.miniButtonRight, GUILayout.Width(30)))
            {
                turret.gameObject.AddComponent<ApplyChill>();
            }
            EditorGUILayout.LabelField("Chill", GUILayout.Width(270));
            EditorGUILayout.EndHorizontal();


        }

        EditorGUILayout.EndVertical();








        */

        GUILayout.Space(20);
        if (extendedSetup = EditorGUILayout.Toggle("Extended Setup", extendedSetup))
        {
            turret.ShootPoint = (Transform)EditorGUILayout.ObjectField("Shoot Point", turret.ShootPoint, typeof(Transform), false);
            turret.Image = (Sprite)EditorGUILayout.ObjectField("Image", turret.Image, typeof(Sprite), false);
            if(Merge = EditorGUILayout.Toggle("Merge", Merge))
            {
                EditorGUILayout.LabelField("Size: " + turret.Merging.Count);
                for(int i = 0; i < turret.Merging.Count; i++)
                {

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Merge Recipe " + (i + 1));
                    if(GUILayout.Button("Delete", EditorStyles.miniButton))
                    {
                        turret.Merging.RemoveAt(i);
                        return;
                    }
                    EditorGUILayout.EndHorizontal();

                    turret.Merging[i].MergesWith = (Transform)EditorGUILayout.ObjectField("Merge With", turret.Merging[i].MergesWith, typeof(Transform), false);
                    turret.Merging[i].Result = (Transform)EditorGUILayout.ObjectField("Result", turret.Merging[i].Result, typeof(Transform), false);
                    GUILayout.Space(5);
                }
                if (GUILayout.Button("Add New Merge", EditorStyles.miniButton))
                {
                    turret.Merging.Add(new MergingList.Recipe());
                }
            }
        }
    }

    private void OnDisable()
    {
        if(target != null)
        if (EditorUtility.IsPersistent(target))
        {
            EditorUtility.SetDirty(target);
        }
    }
}
