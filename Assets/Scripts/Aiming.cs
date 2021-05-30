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
    
    //responsible for controller input mostly
    public bool isAiming = false;
    public bool triggerHeld = false;
    public bool isTriggerReady = false;

    //responsible for firerate
    bool canShoot = true;

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
    }

    private void CheckShoot()
    {
        switch ((int)weapon.weaponSlot)
        {
            case 0:
                if (isAiming && (Input.GetButtonDown("Fire1") || isTriggerReady))
                {
                    if (canShoot)
                    {
                        StartCoroutine(RifleWait(weapon.fireRate));
                    }
                }

                if (isAiming == false || (Input.GetButtonUp("Fire1") || Input.GetAxisRaw("RightTrigger") <= 0))
                {
                    weapon.StopShooting();
                }
                break;
            case 1:
                if (isAiming && (Input.GetButtonDown("Fire1") || isTriggerReady))
                {
                    if (canShoot)
                    {
                        StartCoroutine(RifleWait(weapon.fireRate));
                    }
                }

                if (isAiming == false || (Input.GetButtonUp("Fire1") || Input.GetAxisRaw("RightTrigger") <= 0))
                {
                    weapon.StopShooting();
                }
                break;
            case 2:
                if (isAiming && (Input.GetButton("Fire1") || Input.GetAxisRaw("RightTrigger") >= 0.1))
                {
                    if (canShoot)
                    {
                        StartCoroutine(RifleWait(weapon.fireRate));
                    }
                }

                if (isAiming == false || (Input.GetButtonUp("Fire1") || Input.GetAxisRaw("RightTrigger") <= 0))
                {
                    weapon.StopShooting();
                }
                break;
        }
    }

    private void CheckAim() //Checking aim, setting camera, changing position, changing rotation method
    {
        if (rigController.GetBool("aim_weapon") && !rigController.GetBool("holster_weapon"))
        {
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

    IEnumerator RifleWait(float waitTime)
    {
        canShoot = false;
        weapon.Shoot();
        isTriggerReady = false;
        yield return new WaitForSeconds(waitTime);
        canShoot = true;
    }
}
