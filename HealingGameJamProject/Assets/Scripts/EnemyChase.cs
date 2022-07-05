using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    Transform targetDestination;
    [SerializeField] float speed;

    Rigidbody2D rgbd2d;
    Component[] childTransforms;
    SpriteRenderer sprite;
    public bool stopped = false;

    private void Awake()
    {
        targetDestination = GameObject.FindWithTag("Player").transform;
        rgbd2d = GetComponent<Rigidbody2D>();
        childTransforms = GetComponentsInChildren<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        //targetGameobject = targetDestination.gameObject;
    }

    private void FixedUpdate()
    {
        if (!stopped)
        {
            // Move self...
            Vector3 direction = (targetDestination.position - transform.position).normalized;
            rgbd2d.velocity = direction.normalized * speed;
            // And move all children.
            foreach (Transform trans in childTransforms){ trans.position = this.transform.position; }

            // SPRITE FLIPPING
            if (direction.x > 0){ sprite.flipX = true; }
            if (direction.x < 0){ sprite.flipX = false; }
        }
        else { rgbd2d.velocity = Vector3.zero; }
    }
}
