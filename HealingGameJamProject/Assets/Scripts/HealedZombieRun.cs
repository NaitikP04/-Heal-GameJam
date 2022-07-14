using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealedZombieRun : MonoBehaviour
{
    Transform currentTarget;
    [SerializeField] float speed = 1f;
    Vector3 direction = default(Vector3);
    Rigidbody2D body;
    Component[] childTransforms;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = GameObject.FindWithTag("Player").transform;
        body = gameObject.GetComponent<Rigidbody2D>();
        childTransforms = GetComponentsInChildren<Transform>();
        sprite = GetComponent<SpriteRenderer>();

        InitializeDirection();

        // REPLACE: CODE THAT DOES ATTRACTION ON STARTUP IF WE'RE TOUCHING A COLLIDER
        // if (other.tag == "HealedRun Attractor"){ print("attracted"); }
    }
    private void FixedUpdate()
    {
        body.velocity = direction.normalized * speed;
        // And move all children.
        foreach (Transform trans in childTransforms){ trans.position = this.transform.position; }

        // SPRITE FLIPPING
        if (direction.x > 0){ sprite.flipX = false; }
        if (direction.x < 0){ sprite.flipX = true; }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "HealedRun Attractor"){
            print("attracted");
            currentTarget = other.transform.parent;
            direction = (currentTarget.position - transform.position);
            direction.z = 0f;
        }
    }

    void InitializeDirection()
    {
        Vector3 cameraPos = GameObject.FindWithTag("MainCamera").transform.position;
        direction = (transform.position - cameraPos);
        direction.z = 0f;

        if (direction.x > 0){ sprite.flipX = false; }
        if (direction.x < 0){ sprite.flipX = true; }
    }
    
}
