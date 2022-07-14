using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupEffect : ScriptableObject
{
    public Color particleColor;
    public abstract void Apply(GameObject target);
}
