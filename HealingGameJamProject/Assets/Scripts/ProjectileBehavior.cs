using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    public float speed = 15f;
    public Rigidbody2D rigiddbody;
    
    void Start()
    {
        rigiddbody = GetComponent<Rigidbody2D>();
        rigiddbody.velocity = transform.right * speed;
        StartCoroutine(BulletDecayRoutine());
    }

    void OnTriggerEnter2D (Collider2D other)
    {
            if (other.tag == "Enemy Trigger"){ other.GetComponent<TriggerDestroyGrandparent>().DestroyGrandparent(); }
            if (other.tag != "Player" && other.tag != "CameraBounds"){ Destroy(this.gameObject); }
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

