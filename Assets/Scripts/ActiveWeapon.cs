using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary = 0,
        Secondary = 1,
        Tertiary = 2
    }
    public Transform crossHairTarget;
    public Transform[] weaponSlots;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;
    public Animator rigController;
    public Cinemachine.CinemachineFreeLook aimCamera;

    RaycastWeapon[] equippedWeapons = new RaycastWeapon[3];
    int activeWeaponIndex;
    Aiming weaponHandler;
    Equipment equipment;
    // Start is called before the first frame update
    void Start()
    {
        weaponHandler = GetComponent<Aiming>();
        equipment = GetComponent<Equipment>();
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
            
        }
    }

    RaycastWeapon GetWeapon(int index)
    {
        if (index < 0 || index >= equippedWeapons.Length)
        {
            return null;
        }
        return equippedWeapons[index];
    }

    // Update is called once per frame
    void Update()
    {
        var weapon = GetWeapon(activeWeaponIndex);
        if (weapon)
        {
            if (Input.GetButtonDown("Holster"))
            {
                ToggleActiveWeapon();
            }
            if (Input.GetButtonDown("Quick1") || Input.GetAxisRaw("DPadX") == -1)
            {
                if (equippedWeapons[0])
                {
                    SetActiveWeapon(WeaponSlot.Primary);
                } 
            }
            if (Input.GetButtonDown("Quick2") || Input.GetAxisRaw("DPadX") == 1)
            {
                if (equippedWeapons[1])
                {
                    SetActiveWeapon(WeaponSlot.Secondary);
                }
            }
            if (Input.GetButtonDown("Quick3") || Input.GetAxisRaw("DPadY") == 1)
            {
                if (equippedWeapons[2])
                {
                    SetActiveWeapon(WeaponSlot.Tertiary);
                }
            }
            if (Input.GetButton("Fire2") || Input.GetAxisRaw("LeftTrigger") > 0)
            {
                rigController.SetBool("aim_weapon", true);
            }
            else
            {
                rigController.SetBool("aim_weapon", false);
            }
        }
    }

    public void Equip(RaycastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.raycastDestination = crossHairTarget;
        weapon.recoil.aimCamera = aimCamera;
        weapon.recoil.rigController = rigController;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        //weapon.transform.localPosition = Vector3.zero;
        //weapon.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        weaponHandler.LookForWeapon(weapon);
        equippedWeapons[weaponSlotIndex] = weapon;
        SetActiveWeapon(newWeapon.weaponSlot);
    }

    void ToggleActiveWeapon()
    {
        bool isHolstered = rigController.GetBool("holster_weapon");
        if (isHolstered)
        {
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }
    }

    void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;
        if (holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }
        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    IEnumerator HolsterWeapon(int index)
    {
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("holster_weapon", true);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    IEnumerator ActivateWeapon(int index)
    {
        var weapon = GetWeapon(index);
        weaponHandler.LookForWeapon(weapon);
        if (weapon)
        {
            equipment.ChangeAmmoUI(index);
            rigController.SetBool("holster_weapon", false);
            rigController.Play("equip_" + weapon.weaponName);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;

    }

    /*[ContextMenu("Save weapon pose")]
    void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(weapon.weaponPoseAnimation);
    }
    [ContextMenu("Save weapon aim pose")]
    void SaveWeaponAimPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(weapon.weaponAimAnimation);
    }*/
}
