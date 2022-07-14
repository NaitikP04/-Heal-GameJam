using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealingBomb")]
public class HealingBombPickup : PowerupEffect
{
    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerAttack>().bombEquipped = true;
        target.GetComponent<PlayerAttack>().gunEquipped = false;
    }
}
