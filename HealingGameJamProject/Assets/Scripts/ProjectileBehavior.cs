using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{

    public float speed;
    public GameObject impacteffect;

    private Rigidbody2D rigiddbody;
    
    void Start()
    {
        rigiddbody = GetComponent<Rigidbody2D>();
        rigiddbody.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D (Collider2D col)
        {
            if (col.tag == "Enemy")
            {
                Instantiate(impacteffect, transform.position, Quaternion.identity);
                Destroy(GameObject.FindWithTag("Enemy"));
                Destroy(this.gameObject);
            }
        }
}
