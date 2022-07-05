using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour
{
    public float health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // The amount of time after getting hurt that we're immune to damage.
    [SerializeField] float invulnerableTime = 0.5f;
    bool invulnerable = false;

    void Update(){
        if(health > numOfHearts){
            health = numOfHearts;
        }
                
        for(int i = 0; i<hearts.Length; i ++){  //for each heart in the array
            if(i < health){
                hearts[i].sprite = fullHeart;
            }
            else{
                hearts[i].sprite = emptyHeart;
            }
                              
            if(i < numOfHearts){
                hearts[i].enabled = true; 
            }
            else{
                hearts[i].enabled = false; 
            } 
        }
    }

    public void TakeDamage(int amount, bool doInvulnerabilty=true)
    {
        // Used to do damage to the player. Can be called from anywhere using Health.TakeDamage(...).
        if (!invulnerable){
            ChangeHealth(-amount);
            if (doInvulnerabilty){ StartCoroutine(TakeDamage_make_invulnerable()); }
        }
    }
    private IEnumerator TakeDamage_make_invulnerable()
    {
            // Private IEnumerator to only be used by TakeDamage or other future, similar scripts.
            // If we need to make the player invulnerable for some other reason, we should just write another
            // script for that. Or make bool invulnerable public or something.
            invulnerable = true;    print("invulnerable!");
            yield return new WaitForSeconds(invulnerableTime);
            invulnerable = false;   print("vulnerable!");
    }

    public void ChangeHealth(int amount)
    {
        // Universal function for changing health. Used by other class functions TakeDamage, etc.
        // Avoid using ChangeHealth() from other scripts wherever possible, for tidyness.
        health += amount; 
    }
}
