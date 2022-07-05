using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] Transform targetDestination;
    [SerializeField] float speed;

    Rigidbody2D rgbd2d;
    Component[] childTransforms;

    private void Awake()
    {
        rgbd2d = GetComponent<Rigidbody2D>();
        childTransforms = GetComponentsInChildren<Transform>();
        //targetGameobject = targetDestination.gameObject;
    }

    private void FixedUpdate()
    {
        Vector3 direction = (targetDestination.position - transform.position).normalized;
        rgbd2d.velocity = direction.normalized * speed;
        foreach (Transform trans in childTransforms){ trans.position = this.transform.position; }
    }

}
