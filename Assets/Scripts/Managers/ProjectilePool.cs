using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PooledObject
{
    public SimpleProjectile Object;
    public int Amount;
}

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;
    public GameObject BulletsStorage;
    public PooledObject[] Objects;
    private List<SimpleProjectile>[] pool;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SimpleProjectile temp;
        pool = new List<SimpleProjectile>[Objects.Length];

        for (int count = 0; count < Objects.Length; count++)
        {
            pool[count] = new List<SimpleProjectile>();
            for (int num = 0; num < Objects[count].Amount; num++)
            {
                temp = Instantiate(Objects[count].Object);
                temp.gameObject.SetActive(false);
                temp.transform.parent = BulletsStorage.transform;
                pool[count].Add(temp);
            }
        }
    }
    public SimpleProjectile Activate(int id, Vector3 position, Quaternion rotation)
    {
        for (int count = 0; count < pool[id].Count; count++)
        {
            if (!pool[id][count].gameObject.activeInHierarchy)
            {
                pool[id][count].transform.position = position;
                pool[id][count].transform.rotation = rotation;
                pool[id][count].transform.parent = BulletsStorage.transform;
                //pool[id][count].gameObject.SetActive(true);
                
                return pool[id][count];
            }
        }
        SimpleProjectile newObj = Instantiate(Objects[id].Object) as SimpleProjectile;
        newObj.gameObject.SetActive(false);
        newObj.transform.position = position;
        newObj.transform.rotation = rotation;
        newObj.transform.parent = BulletsStorage.transform;
        pool[id].Add(newObj);
        return newObj;
    }
}