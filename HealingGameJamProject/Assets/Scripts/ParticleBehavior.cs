using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehavior : MonoBehaviour
{
    Rigidbody2D rgbd2d;
    Vector3 direction;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        rgbd2d = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(BrownianMotion());
        StartCoroutine(Lifetime());

        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    void Update()
    {
        rgbd2d.velocity = direction.normalized * 0.667f;
    }

    IEnumerator BrownianMotion()
    {
        while (true)
        {
            direction = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(2);

        float elapsed = 0;
        while (elapsed < 0.5f){
            this.transform.localScale = this.transform.localScale * 0.99f;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
