using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] GunScriptableObject gun;
    [SerializeField] EnemyScriptableObject enemy;
    [SerializeField] GameObject player;
    [SerializeField] GameObject gunHand;
    [SerializeField] float aimHeadOffset;

    public float rotationZ;

    private float curFireSpeedTimer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gunGO = Instantiate(gun.GetGun(), gunHand.transform.position, Quaternion.identity) as GameObject;
        gunGO.transform.parent = gunHand.transform;
        curFireSpeedTimer = gun.GetFireRate();
    }

    // Update is called once per frame
    void Update()
    {   
        curFireSpeedTimer -= Time.deltaTime;
        if (GetComponent<EnemyAI>().CheckAggro())
        {
            if (curFireSpeedTimer <= 0)
            {
                Shoot();
            }
        }
    }


    private void Shoot()
    {
        GameObject projectile = Instantiate(gun.GetProjectile(), gunHand.transform.position, Quaternion.identity) as GameObject;
        var dir = (player.transform.position + new Vector3(0f, aimHeadOffset,0f)) - gunHand.transform.position;
        dir.Normalize();
        projectile.GetComponent<Rigidbody2D>().velocity = dir * gun.GetProjectileSpeed();
        rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        projectile.GetComponent<Projectile>().SetProjectileAngle(rotationZ);
        projectile.transform.Rotate(0, 0, rotationZ);
        curFireSpeedTimer = gun.GetFireRate();
        Destroy(projectile, 5f);
    }
}
