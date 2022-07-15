using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGZombSpawner : MonoBehaviour
{

    // Donut-based spawn range from: https://answers.unity.com/questions/1580130/i-need-to-instantiate-an-object-inside-a-donut-ins.html
    // SHOUTOUT TO BRACKEYS!!!!

    [SerializeField] GameObject zomb;
    public float innerRadius = 10f;
    public float outerRadius = 20f;

    public enum SpawnState {SPAWNING, WAITING, COUNTING};
    
    [System.Serializable]
    public class Wave
    {
        public string name;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f;

    public SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
        //StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
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
            nextWave = 0;
            Debug.Log("Completed all waves! Looping..."); //change to a game completed screen
        }
        else
        {
            nextWave++;
        }
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
        for(int i = 0; i < _wave.count; i++)
        {
            SpawnZomb();
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
    }

    void SpawnZomb()
    {
        Instantiate(zomb, GetSpawnPoint(), Quaternion.identity, this.transform);
    }

    Vector3 GetSpawnPoint()
    {
        float ratio = innerRadius / outerRadius;
        float radius = Mathf.Sqrt(Random.Range(ratio*ratio, 1f)) * outerRadius;
        return Random.insideUnitCircle.normalized * radius;
    }
}
