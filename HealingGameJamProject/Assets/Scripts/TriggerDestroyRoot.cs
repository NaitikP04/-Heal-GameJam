using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDestroyRoot : MonoBehaviour
{
    public void DestroyRoot()
    {
        Destroy(this.gameObject.transform.root.gameObject);
    }
}
