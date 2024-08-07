using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    
    public GameObject particle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"){ 
            for (int i = 0; i < 8; i++){
                GameObject p = Instantiate(particle, this.transform.position, Quaternion.identity, this.transform.parent.transform);
                p.GetComponent<ParticleBehavior>().color = powerupEffect.particleColor;
            }

            powerupEffect.Apply(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
