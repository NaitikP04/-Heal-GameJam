using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[CreateAssetMenu(menuName = "Powerups/SpeedInvincibility")]
public class SpeedInvincibility : PowerupEffect
{
    public float speedMultiplier;
    public float powerUpTime;

    public override void Apply(GameObject target)
    {           
        MonoBehaviour mono = GameObject.FindObjectOfType<MonoBehaviour>();
        
        mono.StartCoroutine(powerUpForDuration());

        IEnumerator powerUpForDuration()
        {
            target.GetComponent<BasicPlayerMovement>().maxVelocity *= speedMultiplier;
            target.GetComponent<BasicPlayerMovement>().accelerationTime = 0.05f;
            target.GetComponent<Health>().invulnerable = true;
            
            yield return new WaitForSeconds(powerUpTime);
            
            target.GetComponent<BasicPlayerMovement>().maxVelocity /= speedMultiplier;
            target.GetComponent<BasicPlayerMovement>().accelerationTime = 0.3f;
            target.GetComponent<Health>().invulnerable = false;
        }
        
    }
}

