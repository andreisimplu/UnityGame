using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update

    private WeaponManager weapon_Manger;
    public float fireRate = 15f;

    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool is_Aiming;

    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab;

    [SerializeField]
    private Transform arrow_Bow_Start_Position;
    


    private void Awake()
    {
        weapon_Manger = GetComponent<WeaponManager>();

        zoomCameraAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
    }

    void Start()    
    {
        
    }

    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot()
    {
        if(weapon_Manger.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                weapon_Manger.GetCurrentSelectedWeapon().ShootAnimation();

                BulletFired ();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(weapon_Manger.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weapon_Manger.GetCurrentSelectedWeapon().ShootAnimation();
                }

                if(weapon_Manger.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weapon_Manger.GetCurrentSelectedWeapon().ShootAnimation();

                    BulletFired();
                }
                else
                {
                    if (is_Aiming)
                    {
                        weapon_Manger.GetCurrentSelectedWeapon().ShootAnimation();

                        if(weapon_Manger.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArrorOrSpear(true);
                        }
                        else if (weapon_Manger.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            ThrowArrorOrSpear(false);
                        }
                    }
                }

            }
        }
    }

    void ZoomInAndOut() 
    { 
        if(weapon_Manger.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);

                crosshair.SetActive(false);
            }
        }
            if (Input.GetMouseButtonUp(1))
            {
            zoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);

            crosshair.SetActive(true);
            }

            if(weapon_Manger.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM)
            {
            if (Input.GetMouseButtonDown(1))
            {
                weapon_Manger.GetCurrentSelectedWeapon().Aim(true);
                is_Aiming = true;
            }

            }
            if (Input.GetMouseButtonUp(1))
            {
            weapon_Manger.GetCurrentSelectedWeapon().Aim(false);
            is_Aiming = false;
            }

    }


    void ThrowArrorOrSpear(bool throwArrow)
    {
        if (throwArrow)
        {
            GameObject arrow = Instantiate(arrow_Prefab);
            arrow.transform.position = arrow_Bow_Start_Position.position;

            arrow.GetComponent<ArrowBowScript>().Launch();
        }

        else
        {
            GameObject spear = Instantiate(spear_Prefab);
            spear.transform.position = arrow_Bow_Start_Position.position;

            spear.GetComponent<ArrowBowScript>().Launch();
        }

    }


    void BulletFired()
    {
        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (hit.transform.tag == Tags.ENEMY_TAG){
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
    }


}//class
