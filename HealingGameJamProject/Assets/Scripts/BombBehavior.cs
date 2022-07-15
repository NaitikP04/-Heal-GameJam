using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rigiddbody;
    public Vector3 LaunchOffset;
    public Vector3 LaunchDirection;

    // Start is called before the first frame update
    void Start()
    {
        var direction = transform.right + LaunchDirection;
        GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Impulse);
        transform.Translate(LaunchOffset);   
    }
    
    void Update()
    {
        void OnTriggerEnter2D (Collider2D other){
            if (other.tag == "Enemy Trigger"){ StartCoroutine(DestroyAfterTime()); }
            if (other.tag != "Player" && other.tag != "CameraBounds"){ Destroy(this.gameObject); }
        }
    }

    public IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(1);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D enemy in enemies)
        {
            //change status of enemy to being fully healed
            StartCoroutine(enemy.gameObject.GetComponent<GeneralArrowSequence>().FullyHealedSelf());
            Destroy(gameObject);                   
        }
        //after using the bomb, set gunEquipped to true again and set bombEquipped to false       

    }
}



