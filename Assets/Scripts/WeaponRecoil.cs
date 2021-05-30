using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CinemachineFreeLook aimCamera;
    [HideInInspector] public CinemachineImpulseSource cameraShake;
    [HideInInspector] public Animator rigController;

    public float verticalRecoil;
    public float horizontalRecoil;
    public float duration;

    float lerp;
    float time;
    // Start is called before the first frame update
    void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    public void GenerateRecoil(string weaponName)
    {
        time = duration;
        cameraShake.GenerateImpulse(Camera.main.transform.forward);
        rigController.Play("weapon_recoil_" + weaponName, 1, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(time > 0)
        {
            lerp = Mathf.Lerp(-horizontalRecoil, horizontalRecoil, Mathf.PingPong(Time.time, 1));
            aimCamera.m_YAxis.Value -= (verticalRecoil * Time.deltaTime)/duration;
            aimCamera.m_XAxis.Value += (lerp * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }

}
