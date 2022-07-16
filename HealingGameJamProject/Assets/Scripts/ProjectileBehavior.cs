using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    public float speed = 15f;
    public Rigidbody2D rigiddbody;
    [SerializeField] GameObject particle;
    [SerializeField] Color particleColor;
    [SerializeField] Transform particleParent;
    
    void Start()
    {
        rigiddbody = GetComponent<Rigidbody2D>();
        rigiddbody.velocity = transform.right * speed;
        StartCoroutine(BulletDecayRoutine());
    }

    void OnTriggerEnter2D (Collider2D other)
    {
            if (other.tag == "Enemy Trigger"){ other.GetComponent<TriggerDestroyGrandparent>().DestroyGrandparent(); }
            if (other.tag == "Environment"){ Destroy(this.gameObject); SpawnEnvironmentParticles(); }
    }

    void SpawnEnvironmentParticles()
    {
        particleParent = GameObject.FindWithTag("GameManager").transform;
        for (int i = 0; i < 8; i++){
            GameObject p = Instantiate(particle, this.transform.position, Quaternion.identity, particleParent);
            ParticleBehavior pb = p.GetComponent<ParticleBehavior>();
            pb.color = particleColor;
            pb.delay = 0.5f;
        }
    }
    IEnumerator BulletDecayRoutine()
    {
        yield return new WaitForSeconds(2);

        float elapsed = 0;
        while (elapsed < 2){
            this.transform.localScale = this.transform.localScale * 0.99f;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);

    }

}

