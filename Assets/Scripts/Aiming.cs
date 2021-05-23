using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Aiming : MonoBehaviour
{   //Changing weight of head, spine1, spine2 beacuse weapon slots are on the same rig 
    public MultiAimConstraint aimingPoseHead;
    public MultiAimConstraint aimingPoseSpine1;
    public MultiAimConstraint aimingPoseSpine2;
    public Rig aimingPose;
    public Rig handIK;
    public GameObject moveCamera;
    public GameObject aimCamera;
    public Transform camera;
    public GameObject aimReticle;
    public Animator rigController;

    RaycastWeapon weapon;
    

    public bool isAiming = false;
    public bool triggerHeld = false;
    public bool isTriggerReady = false;

    private bool weaponReady = false;

    // Start is called before the first frame update
    void Start()
    {
        aimingPoseHead.weight = 0;
        aimingPoseSpine1.weight = 0;
        aimingPoseSpine2.weight = 0;
    }

    public void LookForWeapon(RaycastWeapon weaponClone)
    {
        weapon = weaponClone;
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon)
        {
            CheckAim();
            CheckShoot();
        }
        else
        {
            //handIK.weight = 0.0f;
            //anim.SetLayerWeight(1, 0.0f);
        }
        
    }

    private void CheckShoot()
    {
            if (isAiming && (Input.GetButtonDown("Fire1") || isTriggerReady))
            {
                weapon.Shoot();
                isTriggerReady = false;
            }
            if (isAiming == false || (Input.GetButtonUp("Fire1") || Input.GetAxisRaw("RightTrigger") <= 0))
            {
                weapon.StopShooting();
            }
    }

    private void CheckAim() //Checking aim, setting camera, changing position, changing rotation method
    {
        if (rigController.GetBool("aim_weapon") && !rigController.GetBool("holster_weapon"))
        {
            //overrides["weapon_anim_empty"] = weapon.weaponAimAnimation;
            HandleRightTrigger();
            isAiming = true;
            transform.rotation = Quaternion.Euler(0f, camera.eulerAngles.y, 0f);
            moveCamera.SetActive(false);
            aimCamera.SetActive(true);
            aimReticle.SetActive(true);
            aimingPose.weight += 2f * Time.deltaTime;
            aimingPoseHead.weight = 1;
            aimingPoseSpine1.weight = 1;
            aimingPoseSpine2.weight = 1;
        }
        else
        {
            isAiming = false;
            moveCamera.SetActive(true);
            aimCamera.SetActive(false);
            aimReticle.SetActive(false);
            aimingPose.weight -= 2f * Time.deltaTime;
            aimingPoseHead.weight = 0;
            aimingPoseSpine1.weight = 0;
            aimingPoseSpine2.weight = 0;
        }
    }
    private void HandleRightTrigger()
    {
        if(Input.GetAxisRaw("RightTrigger") != 0)
        {
            triggerHeld = true;
        }
        else if (Input.GetAxisRaw("RightTrigger") == 0 && triggerHeld)
        {
            isTriggerReady = true;
            triggerHeld = false;
        }
    }
}
