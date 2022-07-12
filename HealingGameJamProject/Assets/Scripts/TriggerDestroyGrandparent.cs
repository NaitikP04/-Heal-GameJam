using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDestroyGrandparent : MonoBehaviour
{
    public void DestroyGrandparent()
    {
        Destroy(this.gameObject.transform.parent.parent.gameObject);
    }
}
