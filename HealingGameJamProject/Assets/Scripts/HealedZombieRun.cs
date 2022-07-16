using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealedZombieRun : MonoBehaviour
{
    Transform currentTarget;
    [SerializeField] float speed = 1f;
    Vector3 direction = default(Vector3);
    Rigidbody2D body;
    Component[] childTransforms;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        currentTarget = GameObject.FindWithTag("Player").transform;
        body = gameObject.GetComponent<Rigidbody2D>();
        childTransforms = GetComponentsInChildren<Transform>();
        sprite = GetComponent<SpriteRenderer>();

        InitializeDirection();
        StartCoroutine(FadeRoutine());

        // REPLACE: CODE THAT DOES ATTRACTION ON STARTUP IF WE'RE TOUCHING A COLLIDER
        // if (other.tag == "HealedRun Attractor"){ print("attracted"); }
    }
    private void FixedUpdate()
    {
        body.velocity = direction.normalized * speed;
        // And move all children.
        foreach (Transform trans in childTransforms){ trans.position = this.transform.position; }

        // SPRITE FLIPPING
        if (direction.x > 0){ sprite.flipX = false; }
        if (direction.x < 0){ sprite.flipX = true; }
    }

    IEnumerator FadeRoutine()
    {        
        // THIS IS CODE WHICH MAKES HEALED ZOMBIES FADE OUT
        // THIS IS SO THAT THE PLAYER DOESN'T SEEM THEM WALK THROUGH WALLS
        // A more ideal solution would be pathfinding, but we don't have that luxury sadly :I

        yield return new WaitForSeconds(0.5f);

        float alpha = 100;
        float elapsed = 0;
        float maxTime = 0.5f;
        Color currentColor = sprite.color;

        while (elapsed < maxTime){

                alpha = Mathf.Lerp(1f, 0f, (elapsed/maxTime));
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);

                elapsed += Time.deltaTime;
                yield return null;
            }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "HealedRun Attractor"){
            print("attracted");
            currentTarget = other.transform.parent;
            direction = (currentTarget.position - transform.position);
            direction.z = 0f;
        }
    }

    void InitializeDirection()
    {
        Vector3 cameraPos = GameObject.FindWithTag("MainCamera").transform.position;
        direction = (transform.position - cameraPos);
        direction.z = 0f;

        if (direction.x > 0){ sprite.flipX = false; }
        if (direction.x < 0){ sprite.flipX = true; }
    }
    
}
