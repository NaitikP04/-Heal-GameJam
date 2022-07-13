using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovingOnCollision : MonoBehaviour
{
    EnemyChase chase;

    // Start is called before the first frame update
    void Start()
    {
        chase = gameObject.transform.parent.GetComponent<EnemyChase>();
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        // If we start touching the player, and we are moving, stop moving.
        if (other.gameObject.tag == "Player"
            && chase.stopped == false){ StartCoroutine(WaitThenStop(true));} 
    }

    void OnTriggerExit2D (Collider2D other)
    {
        // If we stop touching the player, and we are moving, stop moving.
        if (other.gameObject.tag == "Player"
            && chase.stopped == true){ StartCoroutine(WaitThenStop(false)); } 
    }

    IEnumerator WaitThenStop(bool toStop)
    {
        yield return new WaitForSeconds(0.2f);
        chase.stopped = toStop;
    }
}
