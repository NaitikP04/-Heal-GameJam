using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Powerups/SpeedInvincibility")]
public class SpeedInvincibility : PowerupEffect
{
    public float speedMultiplier;

    public override void Apply(GameObject target)
    {
        target.GetComponent<BasicPlayerMovement>().maxVelocity *= speedMultiplier;
        target.GetComponent<BasicPlayerMovement>().accelerationTime = 0.05f;
        target.GetComponent<Health>().invulnerable = true;
    }
}
