using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] GameObject healthBar;

    bool isAlive=true;
    // Start is called before the first frame update
    void Start()
    {
        
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
            health = 0;
            Die();
        }
        healthBar.GetComponent<HealthBar>().UpdateHealth();
    }
    public void Heal(float amount)
    {
        health += amount;
        healthBar.GetComponent<HealthBar>().UpdateHealth();
    }
    public float GetHealth()
    {
        return health;
    }
    private void Die()
    {
        isAlive = false;
        Time.timeScale = 0.0f;
    }
}
