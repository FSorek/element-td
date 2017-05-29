using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BulletPoolingManager : MonoBehaviour {

	public SimpleProjectile pooledProjectile;
    public int pooledAmount = 100;
    public List<SimpleProjectile> projetiles;
    public static BulletPoolingManager Instance;
    void Awake()
    {
        Instance = this;
    }

        // Use this for initialization
   /* void Start () {
        projetiles = new List<SimpleProjectile>();
        for(int i = 0; i<pooledAmount; i++)
        {
            SimpleProjectile proj = (SimpleProjectile)Instantiate(pooledProjectile);
            proj.gameObject.SetActive(false);
            projetiles.Add(proj);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}*/
}
