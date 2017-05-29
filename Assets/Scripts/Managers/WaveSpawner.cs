using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class WaveSpawner : MonoBehaviour {

    public static WaveSpawner Instance;

	private float waveCountdown;
	private float spawnCountdown;
	private SpawnState state = SpawnState.COUNTING;
	private int spawnCounter;
	private GameObject effChild;

	private int nextWave = 0;
	public enum SpawnState { SPAWNING, WAITING, COUNTING}
	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public int spawnCount;
		public int enemiesPerSpawn;
		public float timeBetweenSpawns = 1f;
        public float DelayNextWave = 0f;
	}


	[Header("-----------------------UI Setup")]
	public Text nextW0;
	public Text nextW1;
	public Text nextW2;
    public Text nextW3;
    public Text timer;


	[Header("-----------------------Game Setup")]
	public Transform spawnPoint;
	public GameObject SpawnEffect;

	[Header("-----------------------Wave Setup")]
	public float timeToFirstWave = 15f;
	public float timeBetweenWaves = 5f;
	public Wave[] waves;





    void Start () {
		waveCountdown = timeToFirstWave;
		spawnCountdown = 0.1f;
		spawnCounter = waves [nextWave].spawnCount;
		effChild = spawnPoint.transform.GetChild (0).gameObject;
		nextW0.text = waves [nextWave].name;
		nextW1.text = waves [nextWave+1].name;
		nextW2.text = waves [nextWave+2].name;
        nextW3.text = waves [nextWave+3].name;

        Instance = this;
    }
	

	void Update () {

		if (waveCountdown <= 0) {
			spawnCountdown -= Time.deltaTime;
	
		} else {
			waveCountdown -= Time.deltaTime;
			timer.text = Mathf.RoundToInt (waveCountdown).ToString ();
		}
		if (spawnCounter == 0) {
			UpdateWave ();



            if (nextWave < waves.Length)
            {
                nextW0.text = waves[nextWave].name;
            }
            else nextW0.text = "";


            if ((nextWave + 1) < waves.Length)
            {
                nextW1.text = waves[nextWave + 1].name;
            }
            else nextW1.text = "";


            if ((nextWave + 2) < waves.Length)
            {
                nextW2.text = waves[nextWave + 2].name;
            }
            else nextW2.text = "";

            if ((nextWave + 3) < waves.Length)
            {
                nextW3.text = waves[nextWave + 3].name;
            }
            else nextW3.text = "";
        }

	}

	void FixedUpdate()
	{
		if (state != SpawnState.SPAWNING && spawnCountdown <= 0 && spawnCounter > 0) {
			StartCoroutine (SpawnWave (waves[nextWave]));
			spawnCounter -= 1;
            if (spawnCounter == 0)
                DelegatesEventsManager.WaveFinished();
		}
	}

	void UpdateWave()
	{

		if (nextWave + 1 > waves.Length - 1) {
			return;
		} else
			nextWave++;
		waveCountdown = timeBetweenWaves + waves[nextWave-1].DelayNextWave;
		spawnCounter = waves [nextWave].spawnCount;
		spawnCountdown = 0.1f;
	}

	IEnumerator SpawnWave(Wave _wave)
	{
		state = SpawnState.SPAWNING;

		for (int i = 0; i < _wave.enemiesPerSpawn; i++) {
				SpawnEnemy (_wave.enemy);
				GetComponent<WinLoseManager> ().AddEnemyCount ();
			}
		GameObject spawnEff = (GameObject)Instantiate (SpawnEffect, effChild.transform.position-new Vector3(0,1,0f), Quaternion.Euler(-90,0,0));
		Destroy (spawnEff, 2f);
		state = SpawnState.WAITING;
		spawnCountdown = waves[nextWave].timeBetweenSpawns;
		yield break;
	}

	void SpawnEnemy(Transform _enemy)
	{
		Instantiate (_enemy, spawnPoint.position, Quaternion.identity);
	}

	public void SkipTimer()
	{
		waveCountdown = 0f;
		timer.text = Mathf.RoundToInt (waveCountdown).ToString ();
	}

    public void DEV_skipTo(int waveNum)
    {
        waveCountdown = 10f;
        if (waveNum > waves.Length)
            return;
        int goldValue = 0;
        int skippedWaves = waveNum - nextWave;
        if(skippedWaves>0)
        {
            for(int i=0; i<skippedWaves; i++)
            {
                goldValue += waves[nextWave].enemy.GetComponent<EnemyStat>().GoldRewardForKill * waves[nextWave].enemiesPerSpawn * waves[nextWave].spawnCount;
                spawnCounter = 0;
            }
        }
        PlayerManager.Gold += goldValue;
    }
}
