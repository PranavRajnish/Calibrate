using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject crosshairPrefab;
    [SerializeField] GunScriptableObject gunSO;
    [SerializeField] GameObject gunHand;
    [SerializeField] Transform crouchHand;
    [SerializeField] GameObject ammoBar;
    [SerializeField] int minRotation;
    [SerializeField] int maxRotation;
    [SerializeField] float crouchGunRotation;

    Vector3 aimPos;
    Quaternion crouchQuat;

    public GameObject crosshair;
    public float projectileAngle;

    private GameObject crouchGun;
    private GameObject gun;

    private float curFireRate;
    private int curClipSize;
    private bool isReloading=false;

    private bool shootButtonPressed = false;
    bool runOnce = true;

    private void Start()
    {
        crosshair = Instantiate(crosshairPrefab, transform.position, Quaternion.identity) as GameObject;
        gun =Instantiate(gunSO.GetGun(), gunHand.transform.position, Quaternion.identity, gunHand.transform) as GameObject;
        crouchQuat.eulerAngles = new Vector3(0, 0, crouchGunRotation);
        crouchGun = Instantiate(gunSO.GetGun(), crouchHand.position,crouchQuat,crouchHand) as GameObject;
        crouchGun.GetComponent<SpriteRenderer>().sortingOrder = -1;
        crouchGun.SetActive(false);
        curFireRate = gunSO.GetFireRate();
        curClipSize = gunSO.GetClipSize();
    }
    private void Update()
    {
        curFireRate -= Time.deltaTime;
        if (Input.GetMouseButton(0)) { shootButtonPressed = true; }
        else { shootButtonPressed = false; }
    }
    private void FixedUpdate()
    {
        if (GetComponent<PlayerMovement>().isCrouched==false)
        {
            if (runOnce) { crosshair.SetActive(true); runOnce = false; }
            Vector2 shootPos = MoveCrosshair(crosshair);
            StartCoroutine(CheckReload());
            if (shootButtonPressed == true)
            {
                Shoot(shootPos);
            }
        }
        else
        {
            crosshair.SetActive(false);
            runOnce = true;
        }
        CrouchAnimation();
    }

    private Vector2 MoveCrosshair( GameObject crosshair)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        aimPos = Camera.main.ScreenToWorldPoint(mousePos);
        var difference = (Vector2)aimPos - (Vector2)gunHand.transform.position;
        difference.Normalize();
        var restraint= Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if (restraint >maxRotation || restraint < minRotation)
        {           
            transform.localScale = new Vector2(-1f, 1f);
        }
        else
        {
            transform.localScale = new Vector2(1f, 1f);
        }
        crosshair.transform.position = (Vector2)aimPos;
        return crosshair.transform.position;
    }

    private void Shoot(Vector2 shootPos)
    {
        if (curFireRate <= 0 && isReloading==false)
        {
            curClipSize -= 1;
            ammoBar.GetComponent<AmmoBar>().ShotFiredUI(curClipSize);
            FireContinuously(shootPos);
        }        
    }

    private void FireContinuously(Vector2 shootPos)
    {
        Vector2 aimDirection = (shootPos - new Vector2(gunHand.transform.position.x, gunHand.transform.position.y));
        aimDirection.Normalize();
        GameObject projectile = Instantiate(gunSO.GetProjectile(), gunHand.transform.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = (aimDirection * gunSO.GetProjectileSpeed());
        projectileAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        projectile.GetComponent<Projectile>().SetProjectileAngle(projectileAngle);
        projectile.transform.Rotate(0, 0,projectileAngle);
        curFireRate = gunSO.GetFireRate();
        Destroy(projectile, 5f);
    }

    IEnumerator CheckReload()
    {
        if(curClipSize<=0)
        {
            isReloading = true;
            curClipSize = gunSO.GetClipSize();
            yield return new WaitForSeconds(gunSO.GetChargeTime());
            ammoBar.GetComponent<AmmoBar>().ReloadUI();
            isReloading = false;
        }
    }
    public float GetMouseAngle()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        return rotationZ;
    }
    private void CrouchAnimation()
    {
        if(GetComponent<PlayerMovement>().isCrouched)
        {
            crouchGun.SetActive(true);
            gun.SetActive(false);
        }
        else
        {
            gun.SetActive(true);
            crouchGun.SetActive(false);
        }
    }
}
