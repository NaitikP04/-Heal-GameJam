using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExitScreen : MonoBehaviour
{
    TriggerDestroyGrandparent byeByeGramps;

    void Start()
    {
        byeByeGramps = gameObject.GetComponentInChildren<TriggerDestroyGrandparent>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "CameraBounds"){ byeByeGramps.DestroyGrandparent(); }
    }
}
