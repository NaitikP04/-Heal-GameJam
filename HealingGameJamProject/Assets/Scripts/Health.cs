using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour
{
    public float health;

    Animator suitcaseHealthbar;

    // The amount of time after getting hurt that we're immune to damage.
    [SerializeField] float invulnerableTime = 0.5f;
    public bool invulnerable = false;
    public bool SIBuffActive = false;

    public GameObject gameLossScreen;

    void Update()
    {
        suitcaseHealthbar = GameObject.FindWithTag("Health UI Animator").GetComponent<Animator>();
        suitcaseHealthbar.SetInteger("Health", (int)health);
        
        if (invulnerable){
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,.75f);
        }
        if (!invulnerable){
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
        }
        if (health == 0){
            GameLost();
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
            invulnerable = true;    //print("invulnerable!");
            yield return new WaitForSeconds(invulnerableTime);
            if(!SIBuffActive){ invulnerable = false; }  //print("vulnerable!");
    }

    public void ChangeHealth(int amount)
    {
        // Universal function for changing health. Used by other class functions TakeDamage, etc.
        // Avoid using ChangeHealth() from other scripts wherever possible, for tidyness.
        health += amount; 
    }
    
    void GameLost()
    {
        Time.timeScale = 0f;
        //get GameLostCard from main camera and enable it
        gameLossScreen.SetActive(true);
    }
}
