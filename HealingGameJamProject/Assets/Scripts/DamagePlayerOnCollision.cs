using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerOnCollision : MonoBehaviour
{
    GameObject player;
    Health health;
    Collider2D playerBody;
    Collider2D selfBody;
    public bool doDamage = true;

    [SerializeField] int damageAmount = 1;
    
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        health = player.GetComponent<Health>();
        playerBody = player.GetComponent<Collider2D>();
        selfBody = gameObject.GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (doDamage && other == playerBody){ health.TakeDamage(damageAmount); }
    }
}
