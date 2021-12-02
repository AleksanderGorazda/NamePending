using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public ParticleSystem rockDebris;
    public ParticleSystem muzzleFlash;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    //public AnimationClip weaponPoseAnimation;
    //public AnimationClip weaponAimAnimation;
    public string weaponName;
    public ActiveWeapon.WeaponSlot weaponSlot;

    public bool isShooting = false;
    public float fireRate;
    public int flashSize;

    public WeaponRecoil recoil;

    Ray ray;
    RaycastHit hitInfo;

    void Awake()
    {
        recoil = GetComponent<WeaponRecoil>();
    }
    public void Shoot()
    {
        isShooting = true;
        muzzleFlash.Emit(flashSize);
  
        ray.origin = raycastOrigin.position;
        if (weaponName == "shotgun")
        {
            for (int i = 0; i < 10; i++)
            {
                float distance = Vector3.Distance(raycastOrigin.position, raycastDestination.position);
                ray.direction = raycastDestination.position - raycastOrigin.position + new Vector3(0, Random.Range(-0.03f, 0.03f)*distance, Random.Range(-0.03f, 0.03f)*distance);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.point != new Vector3(0.0f, 0.0f, 0.0f))
                    {
                        rockDebris.transform.position = hitInfo.point;
                        rockDebris.transform.forward = hitInfo.normal;
                        rockDebris.Emit(1);
                    }

                    Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
                }
            }
        }
        else
        {

                ray.direction = raycastDestination.position - raycastOrigin.position;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.point != new Vector3(0.0f, 0.0f, 0.0f))
                    {
                        rockDebris.transform.position = hitInfo.point;
                        rockDebris.transform.forward = hitInfo.normal;
                        rockDebris.Emit(1);
                    }

                    Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
                }
        }
        
        recoil.GenerateRecoil(weaponName);
    }

    public void StopShooting()
    {
        isShooting = false;
    }
}
