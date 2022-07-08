using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    Transform targetDestination;
    [SerializeField] float speed;

    Rigidbody2D rgbd2d;
    Component[] childTransforms;
    SpriteRenderer sprite;
    Animator zombAnimator;

    [HideInInspector] public bool stopped = false;

    [Header("=== PARAMETERS ===")]
    public bool stopRandomly = true;
    public bool moveRandomly = true;
    bool randomNow;
    Vector3 randomDirection = new Vector3();

    private void Awake()
    {
        targetDestination = GameObject.FindWithTag("Player").transform;
        rgbd2d = GetComponent<Rigidbody2D>();
        childTransforms = GetComponentsInChildren<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        zombAnimator = GetComponent<Animator>();
        //targetGameobject = targetDestination.gameObject;
    }

    private void Start()
    {
        if (stopRandomly) {StartCoroutine(RandomStops());}
        if (moveRandomly) {StartCoroutine(ReadRandomMovements());}
    }

    private void FixedUpdate()
    {
        if (!stopped)
        {
            // Move self...
            Vector3 direction;
            // If we're not currently moving in a random direction, move towards the target.
            if (!randomNow) { direction = (targetDestination.position - transform.position).normalized; }
            // Otherwise, move in the random direction as determined by ReadRandomMovements().
            else { direction = randomDirection.normalized;}

            rgbd2d.velocity = direction.normalized * speed;
            // And move all children.
            foreach (Transform trans in childTransforms){ trans.position = this.transform.position; }

            // SPRITE FLIPPING
            if (direction.x > 0){ sprite.flipX = false; }
            if (direction.x < 0){ sprite.flipX = true; }
        }
        else
        {
            rgbd2d.velocity = Vector3.zero;
        }

        zombAnimator.SetBool("isMoving", stopped);
        
    }

    IEnumerator RandomStops()
    {
        while (true)
        {
            if (Random.Range(1,7) == 4){
                stopped = true;
                yield return new WaitForSeconds(1);
                stopped = false;
            }
            else { yield return new WaitForSeconds(3); }
        }  
    }

    IEnumerator ReadRandomMovements()
    {
        while (true)
        {
            if (Random.Range(1,4) == 3){
                randomNow = true;
                randomDirection = new Vector3(Random.Range(0f,1f),Random.Range(0f,1f),0f);
                yield return new WaitForSeconds(1.5f);
                randomNow = false;
            }
            else { yield return new WaitForSeconds(5); }
        }  
    }

}
