using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectile;
    public GameObject bomb;

    public int ammo = 5;
    Animator suitcaseAmmobar;

    BasicPlayerMovement movement;
    Quaternion shootDirection;

    public bool gunEquipped = true;
    public bool bombEquipped = false;

    private void Start()
    {
        movement = gameObject.GetComponent<BasicPlayerMovement>();
        suitcaseAmmobar = GameObject.FindWithTag("Ammo UI Animator").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(movement.mostRecentDirection)
        {
            case "up":      shootDirection = Quaternion.Euler(0,0,90);  break; 
            case "left":    shootDirection = Quaternion.Euler(0,0,180); break;  
            case "down":    shootDirection = Quaternion.Euler(0,0,270); break;
            case "right":   shootDirection = Quaternion.Euler(0,0,0);   break;
        }

        if (Input.GetKeyDown(KeyCode.Space) && gunEquipped)
        {
            if (ammo > 0)
            {
                Instantiate(projectile, gameObject.transform.position, shootDirection, this.transform);
                ammo--;
            }
            else { OutOfAmmo(); }
        }

        if (Input.GetKeyDown(KeyCode.Space) && bombEquipped)
        {
            gunEquipped = false;
            Instantiate(bomb, gameObject.transform.position, shootDirection, this.transform);
            //use Physics 2d overlap circle to detect enemies in range, heal all enemies in range
            //Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 5f);
            //foreach (Collider2D enemy in enemies)
            //{
                //if (enemy.gameObject.tag == "Enemy")
                //{
                    //change status of enemy to being fully healed                   
                //}
            //}
            //after using the bomb, set gunEquipped to true again and set bombEquipped to false
        }

        suitcaseAmmobar.SetInteger("Ammo", (int)ammo);
    }
    
    private void OutOfAmmo()
    {
        // Put a sound effect here or something
    }
}
