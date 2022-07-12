using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGZombSpawner : MonoBehaviour
{

    // Donut-based spawn range from: https://answers.unity.com/questions/1580130/i-need-to-instantiate-an-object-inside-a-donut-ins.html

    [SerializeField] GameObject zomb;
    public float innerRadius = 10f;
    public float outerRadius = 20f;
    public float delay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Instantiate(zomb, GetSpawnPoint(), Quaternion.identity, this.transform);
            yield return new WaitForSeconds(delay);
        }
    }

    Vector3 GetSpawnPoint()
    {
        float ratio = innerRadius / outerRadius;
        float radius = Mathf.Sqrt(Random.Range(ratio*ratio, 1f)) * outerRadius;
        return Random.insideUnitCircle.normalized * radius;
    }
}
