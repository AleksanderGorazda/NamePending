using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon weaponPrefab;

    ActiveWeapon activeWeapon;

    private void OnTriggerStay(Collider other)
    {
        activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        if (activeWeapon && Input.GetButtonDown("Equip"))
        {
                RaycastWeapon newWeapon = Instantiate(weaponPrefab);
                activeWeapon.Equip(newWeapon);
            
        }

    }
}
