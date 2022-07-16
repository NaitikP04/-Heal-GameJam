using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCardBehavior : MonoBehaviour
{
    BasicPlayerMovement movement;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        movement = GameObject.FindWithTag("Player").GetComponent<BasicPlayerMovement>();
        movement.enabled = false;
        
        sprite = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(TitleCardRoutine());
    }

    // Update is called once per frame
    public IEnumerator TitleCardRoutine()
    {
        movement.enabled = false;
        yield return new WaitForSeconds(3f);
        movement.enabled = true;

        float alpha = 100;
        float elapsed = 0;
        float maxTime = 2f;
        Color currentColor = sprite.color;

        while (elapsed < maxTime){

                alpha = Mathf.Lerp(1f, 0f, (elapsed/maxTime));
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);

                elapsed += Time.deltaTime;
                yield return null;
            }

        sprite.enabled = false;
        sprite.color = new Color(1f,1f,1f,1f);
    }

}
