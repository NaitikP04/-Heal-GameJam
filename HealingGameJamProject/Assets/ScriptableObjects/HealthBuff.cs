using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{
    public float HealingAmount;
    public override void Apply(GameObject target)
    {
        if(target.GetComponent<Health>().health != 5) {
            target.GetComponent<Health>().health+= HealingAmount;

        }
    }
}