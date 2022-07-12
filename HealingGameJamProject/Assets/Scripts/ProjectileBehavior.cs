using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    //private Vector3 shootDir;
    public float speed =20f;
    public Rigidbody2D rigiddbody;
    
    void Start()
    {
        rigiddbody = GetComponent<Rigidbody2D>();
        rigiddbody.velocity = transform.forward * speed;
    }

    void OnTriggerEnter2D (Collider2D col)
    {
            if (col.tag == "Enemy")
            {
                Destroy(GameObject.FindWithTag("Enemy"));
                Destroy(this.gameObject);
            }
    }

}

