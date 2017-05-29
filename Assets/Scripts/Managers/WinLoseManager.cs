using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class WinLoseManager : MonoBehaviour {
	[HideInInspector]

	private int enemiesOnMap;
    public int maxUnits;
	private bool gameOver;

	[Header("UI Setup")]
	public Text enemyCount;

    public static WinLoseManager Instance;
	// Use this for initialization
	void Start () {
        Instance = this;
		enemiesOnMap = 0;
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (enemyCount.text != enemiesOnMap.ToString ()) {
			enemyCount.text = enemiesOnMap.ToString ();
		}
		if(enemiesOnMap >= maxUnits && !gameOver)
        {
            SceneManager.LoadScene("JeffIsKill");
			gameOver = true;
        }
	}

	public void AddEnemyCount ()
	{
		enemiesOnMap += 1;
	}

	public void RemoveEnemyCount()
	{
		enemiesOnMap -= 1;
	}
}
