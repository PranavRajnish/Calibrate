using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] GameObject bulletImpactPrefab;

    float rotationZ;
    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (collision.GetComponent<EnemyHealth>() && collision.isTrigger==false)
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
            DestroyBullet();
        }
        else if (collision.GetComponent<PlayerHealth>())
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage);
            DestroyBullet();
        }
        else if(collision.GetComponent<CompositeCollider2D>())
        {
            DestroyBullet();
        }
        
    }

    private void DestroyBullet()
    {
        GameObject bulletImpactVFX = Instantiate(bulletImpactPrefab, transform.position, Quaternion.identity) as GameObject;
        bulletImpactVFX.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        Destroy(gameObject);
        Destroy(bulletImpactVFX, 1);
    }

    public void SetProjectileAngle(float projectileAngle)
    {
        rotationZ = projectileAngle;
    }
}
