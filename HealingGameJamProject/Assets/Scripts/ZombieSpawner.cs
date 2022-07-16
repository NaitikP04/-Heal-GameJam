using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    // Donut-based spawn range from: https://answers.unity.com/questions/1580130/i-need-to-instantiate-an-object-inside-a-donut-ins.html
    // SHOUTOUT TO BRACKEYS!!!!

    [SerializeField] GameObject greenZombie;
    [SerializeField] GameObject purpleZombie;
    [SerializeField] GameObject blueZombie;
    public float innerRadius = 10f;
    public float outerRadius = 20f;

    public enum SpawnState {SPAWNING, WAITING, COUNTING};
    public enum ZombieColor {Green, Purple, Blue};

    [System.Serializable]
    public class ZombieType
    {
        public int zCount;
        public ZombieColor zType;
    }
    
    [System.Serializable]
    public class Wave
    {
        public string name;
        public List<ZombieType> zombieTypes;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f;

    public SpawnState state = SpawnState.COUNTING;

    public GameObject gameOverScreen;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive()) { WaveCompleted(); }
            else { return; }
        }
        
        if(waveCountdown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnRoutine(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");
        
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1)
        {
            GameWon();
            nextWave = 0;
            Debug.Log("Completed all waves! Looping..."); //change to a game completed screen
        }
        else { nextWave++; }
    }
    
    bool EnemyIsAlive()
    {
        searchCountdown-=Time.deltaTime;

        if(searchCountdown <=0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnRoutine(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        //spawn

        for(int i = 0; i < _wave.zombieTypes.Count; i++)
        {
            for(int j = 0; j < _wave.zombieTypes[i].zCount; j++)
            {   
                switch (_wave.zombieTypes[i].zType)
                {
                    case ZombieColor.Green: SpawnZomb(greenZombie); break;
                    case ZombieColor.Purple: SpawnZomb(purpleZombie); break;
                    case ZombieColor.Blue: SpawnZomb(blueZombie); break;
                }
                print($"{_wave.name}: spawned a {_wave.zombieTypes[i].zType} zombie. {j+1} of {_wave.zombieTypes[i].zCount}.");

                yield return new WaitForSeconds(1f / _wave.rate);
            }
        }
        state = SpawnState.WAITING;
    }

    void SpawnZomb(GameObject zombie)
    {
        Instantiate(zombie, GetSpawnPoint(), Quaternion.identity, this.transform);
    }

    Vector3 GetSpawnPoint()
    {
        float ratio = innerRadius / outerRadius;
        float radius = Mathf.Sqrt(Random.Range(ratio*ratio, 1f)) * outerRadius;
        return Random.insideUnitCircle.normalized * radius;
    }
    
    void GameWon()
    {
        Time.timeScale = 0f;
        //get GameWonCard from main camera and enable it
        gameOverScreen.SetActive(true);
    }
}
