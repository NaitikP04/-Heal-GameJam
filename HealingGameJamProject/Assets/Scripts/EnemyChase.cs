using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    Transform targetDestination;
    [SerializeField] float speed;
    Vector3 direction;

    Rigidbody2D rgbd2d;
    Component[] childTransforms;
    SpriteRenderer sprite;
    Animator zombAnimator;
    [HideInInspector] public bool fullyHealed = false;

    [Header("=== PARAMETERS ===")]
    public bool stopRandomly = true;
    [HideInInspector] public bool stopped = false;
    public bool moveRandomly = true;
    bool randomNow;
    Vector3 randomDirection = new Vector3();
    Collider2D zombTrigger;
    Collider2D playerCollider;

    private void Awake()
    {
        targetDestination = GameObject.FindWithTag("Player").transform;
        rgbd2d = GetComponent<Rigidbody2D>();
        childTransforms = GetComponentsInChildren<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        zombAnimator = GetComponent<Animator>();
        //targetGameobject = targetDestination.gameObject;
        zombTrigger = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Collider2D>();
        playerCollider = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (stopRandomly) {StartCoroutine(RandomStops());}
        if (moveRandomly) {StartCoroutine(ReadRandomMovements());}
    }

    private void FixedUpdate()
    {
        if (!fullyHealed){
            if (!stopped)
            {
                // If we're not currently moving in a random direction, move towards the target.
                if (!randomNow) { direction = (targetDestination.position - transform.position); }
                // Otherwise, move in the random direction as determined by ReadRandomMovements().
                else { direction = randomDirection;}

                direction.z = 0f;
                rgbd2d.velocity = direction.normalized * speed;
                // And move all children.
                foreach (Transform trans in childTransforms){ trans.position = this.transform.position; }

            }
            else { rgbd2d.velocity = Vector3.zero; }

            // SPRITE FLIPPING
            if (!playerCollider.IsTouching(zombTrigger)){
                if (direction.x > 0){ sprite.flipX = false; }
                if (direction.x < 0){ sprite.flipX = true; }
            }
        }
        else
        { 
            stopped = true;
            rgbd2d.velocity = Vector3.zero;
        }

        zombAnimator.SetBool("isMoving", stopped);
        
    }

    public IEnumerator StopMovingBriefly(float delay)
    {
        stopped = true;
        yield return new WaitForSeconds(delay);
        if (!playerCollider.IsTouching(zombTrigger)){ stopped = false; }
    }

    IEnumerator RandomStops()
    {
        while (true)
        {
            if (Random.Range(1,7) == 4){
                stopped = true;
                yield return new WaitForSeconds(1);
                if (!fullyHealed){ stopped = false; }
            }
            else { yield return new WaitForSeconds(3); }
        }  
    }

    IEnumerator ReadRandomMovements()
    {
        while (true)
        {
            if (Random.Range(1,5) == 1){
                randomNow = true;
                randomDirection = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0f);
                yield return new WaitForSeconds(1.5f);
                randomNow = false;
            }
            else { yield return new WaitForSeconds(5); }
        }  
    }

}
