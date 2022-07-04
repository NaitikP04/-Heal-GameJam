using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] Transform targetDestination;
    [SerializeField] float speed;

    Rigidbody2D rgbd2d;

    private void Awake()
    {
        rgbd2d = GetComponent<Rigidbody2D>();
        //targetGameobject = targetDestination.gameObject;
    }

    private void FixedUpdate()
    {
        Vector3 direction = (targetDestination.position - transform.position).normalized;
        rgbd2d.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health othersHealth = collision.gameObject.GetComponent<Health>();
        
        if (othersHealth)       //collision.gameObject == targetGameobject
        {
            Debug.Log("Enemy hit");
            //othersHealth.health -= 1;
        }

    }

}
