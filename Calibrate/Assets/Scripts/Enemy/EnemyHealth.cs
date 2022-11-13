using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject enemy;

    private float health;
    // Start is called before the first frame update
    void Start()
    {
        health = enemy.GetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float amount)
    {
        if (health > amount)
        {
            health -= amount;
        }
        else
        {
            Die();
        }
    }
    public void Heal(float amount)
    {
        health += amount;
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
