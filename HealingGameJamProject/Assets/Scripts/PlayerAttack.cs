using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectile;

    public int ammo = 5;
    [SerializeField] Animator suitcaseAmmobar;

    BasicPlayerMovement movement;
    Quaternion shootDirection;

    private void Start()
    {
        movement = gameObject.GetComponent<BasicPlayerMovement>();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ammo > 0)
            {
                Instantiate(projectile, gameObject.transform.position, shootDirection, this.transform);
                ammo--;
            }
            else { OutOfAmmo(); }
        }

        suitcaseAmmobar.SetInteger("Ammo", (int)ammo);
    }
    
    private void OutOfAmmo()
    {
        // Put a sound effect here or something
    }
}
