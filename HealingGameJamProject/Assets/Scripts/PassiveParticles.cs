using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveParticles : MonoBehaviour
{
    [SerializeField] GameObject particle;
    [SerializeField] Color particleColor;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnParticles());
    }

    IEnumerator SpawnParticles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3,6));

            for (int i = 0; i < 4; i++){
                    GameObject p = Instantiate(particle, this.transform.position, Quaternion.identity, this.transform.parent.transform);
                    p.GetComponent<ParticleBehavior>().color = particleColor;
                }
        }
    }
}
