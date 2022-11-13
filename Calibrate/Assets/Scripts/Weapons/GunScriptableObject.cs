using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunScriptableObject : ScriptableObject
{
    [SerializeField] float fireRate;
    [SerializeField] int clipSize;
    [SerializeField] float chargeTime;
    [SerializeField] float projectileSpeed;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject projectile;

    public float GetFireRate() { return fireRate; }
    public int GetClipSize() { return clipSize; }
    public float GetChargeTime() { return chargeTime; }
    public float GetProjectileSpeed() { return projectileSpeed; }
    public GameObject GetGun() { return gun; }
    public GameObject GetProjectile() { return projectile; }
}
