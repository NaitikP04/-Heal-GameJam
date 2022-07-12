using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FindClosestEnemy : MonoBehaviour
{
    
    private GameObject FindClosestEnemyWithinRange(float range)
    {
        GameObject[] zomblist;
        zomblist = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
            
        foreach (GameObject zombie in zomblist)
        {
            Vector3 diff = zombie.transform.position - position;
            float currentDistance = diff.sqrMagnitude;

            if (currentDistance <= range && currentDistance < distance)
            {
                closest = zombie;
                distance = currentDistance;
            }
        }

        return closest;
    }

    void Update()
    {
        GameObject closestEnemyTarget = FindClosestEnemyWithinRange(20f);
        Debug.DrawLine (this.transform.position, closestEnemyTarget.transform.position, Color.red);
    }
}
