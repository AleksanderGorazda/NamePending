using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{

    public int[] weaponAmmo = new int[] { 100, 100, 100 };

    public int[] magazineAmmo = new int[] { 4, 16, 30 };

    public UI_Ammo ammo;

    public void RemoveAmmoFromEquipment(int ammoType, int ammoAmount)
    {
        weaponAmmo[ammoType] -= ammoAmount;
        
    }

    public void RemoveAmmoFromMagazine(int ammoType, int ammoAmount)
    {
        magazineAmmo[ammoType] -= ammoAmount;
        ammo.SetAmmo(magazineAmmo[ammoType], weaponAmmo[ammoType]);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void ChangeAmmoUI(int ammoType)
    {
        ammo.SetAmmo(magazineAmmo[ammoType], weaponAmmo[(ammoType)]);
    }
}
