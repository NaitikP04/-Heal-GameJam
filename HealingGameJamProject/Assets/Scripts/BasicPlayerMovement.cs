using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer sprite;

    public float maxVelocity = 5f;
    public float accelerationTime = 1f;
    public float decelerationTime = 1f;
    float X_velocityModifier = 0f;
    float Y_velocityModifier = 0f;

    float horizontalAmt;
    float verticalAmt;
    bool X_moving = false;
    bool Y_moving = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalAmt = Input.GetAxisRaw("Horizontal");
        verticalAmt = Input.GetAxisRaw("Vertical");

        // HORIZONTAL MOVEMENT //

        if (Mathf.Abs(horizontalAmt) > 0 && X_moving == false){
            // Only true if we WEREN'T moving horizontally, but we are now.
            X_moving = true;
            StartCoroutine(AccelerateDecelerate(true, horizontalAmt, accelerationTime));
            }
        if (horizontalAmt == 0 && X_moving == true){
            // Only true if we WERE moving horizontally, but aren't anymore.
            X_moving = false;
            StartCoroutine(AccelerateDecelerate(true, horizontalAmt, decelerationTime));
            }

        // VERTICAL MOVEMENT //

        if (Mathf.Abs(verticalAmt) > 0 && Y_moving == false){
            // Only true if we WEREN'T moving vertically, but we are now.
            Y_moving = true;
            StartCoroutine(AccelerateDecelerate(false, verticalAmt, accelerationTime));
            }
        if (verticalAmt == 0 && Y_moving == true){
            // Only true if we WERE moving vertically, but aren't anymore.
            Y_moving = false;
            StartCoroutine(AccelerateDecelerate(false, verticalAmt, decelerationTime));
            }

        // SPRITE FLIPPING
        
        if (horizontalAmt > 0){ sprite.flipX = true; }
        if (horizontalAmt < 0){ sprite.flipX = false; }

    }

    IEnumerator AccelerateDecelerate(bool axis, float acceleration, float maxTime)
    {
        // Lerps between the current velocity modifier and either 1 or 0, mimicking acceleration or deceleration respectively.
        // PARAMETERS:
        // bool axis --- true: X axis - false: Y axis
        // float accelerate --- -1: accelerate left (lerp to -1) - 0: decelerate (lerp to 0) - 1: accelerate right (lerp to 1)
        // float maxTime --- the amount of time to lerp for

        float start;
        switch (axis){  case true: start = X_velocityModifier;  break;
                        case false: start = Y_velocityModifier; break;} 
        float end = acceleration;

        float elapsed = 0;
        float velocityModifier;

        while (elapsed < maxTime){

                velocityModifier =  Mathf.Lerp(start, end, (elapsed / maxTime));

                bool moving;
                switch (axis){
                    case true:
                        moving = X_moving;
                        X_velocityModifier = velocityModifier;
                        break;
                    case false:
                        moving = Y_moving;
                        Y_velocityModifier = velocityModifier;
                        break;
                    }

                if (Mathf.Abs(acceleration) == 1 && moving == false){ break; }  // If we WERE accelerating but we stopped moving, stop accelerating.
                if (acceleration == 0 && moving == true){ break; }              // If we WERE decelerating but we started moving, stop decelerating.

                elapsed += Time.deltaTime;
                yield return null;

            }
    }

    // FixedUpdate is NOT called once per frame
    void FixedUpdate()
    {
        body.velocity = new Vector2(X_velocityModifier*maxVelocity,
                                    Y_velocityModifier*maxVelocity);
    }

}