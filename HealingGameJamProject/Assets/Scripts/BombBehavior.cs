using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rigiddbody;
    public Vector3 LaunchOffset;

    // Start is called before the first frame update
    void Start()
    {
        var direction = transform.right + Vector3.up;
        GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Impulse);
        transform.Translate(LaunchOffset);   
    }

}
