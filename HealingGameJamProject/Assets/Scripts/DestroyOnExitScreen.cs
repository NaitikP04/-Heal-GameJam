using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExitScreen : MonoBehaviour
{
    TriggerDestroyGrandparent byeByeGramps;
    Collider2D cameraBoundsCollider;
    Collider2D ourCollider;

    void Start()
    {
        byeByeGramps = gameObject.GetComponentInChildren<TriggerDestroyGrandparent>();
        cameraBoundsCollider = GameObject.FindWithTag("CameraBounds").GetComponent<Collider2D>();
        ourCollider = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!ourCollider.IsTouching(cameraBoundsCollider)){ byeByeGramps.DestroyGrandparent(); }
    }
}
