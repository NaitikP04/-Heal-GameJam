using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Ammo : MonoBehaviour
{
   public int ammo;

   [SerializeField] Animator suitcaseAmmobar;

    void Update()
    {
        suitcaseAmmobar.SetInteger("Ammo", (int)ammo);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ammo--;
        }
    }
}