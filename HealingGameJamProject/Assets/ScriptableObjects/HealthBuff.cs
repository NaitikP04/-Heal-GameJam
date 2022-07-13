using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{
    public float HealingAmount;
    
    public override void Apply(GameObject target)
    {
        target.GetComponent<Health>().health += HealingAmount;
    }
}
