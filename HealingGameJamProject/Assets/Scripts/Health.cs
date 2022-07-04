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
    public Sprite halfHeart;

    void Update(){
        if(health > numOfHearts){
            health = numOfHearts;
        }
                
        for(int i = 0; i<hearts.Length; i ++){  //for each heart in the array
            if(i < health){
                hearts[i].sprite = fullHeart;
            }
            // implement a system to show halfHeart when health decreases by 0.5 hearts
            // this always displays an additional half heart, doesnt work properly when health is incremented by 0.5
            // find a fix for this

            else if(i < health + 0.5f){ 
                hearts[i].sprite = halfHeart;
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
}
