using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy",menuName ="Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField] float health;
    [SerializeField] float damage;
    [SerializeField] float moveSpeed;
    [SerializeField] float aggroRange;
    [SerializeField] float searchTime;
    [SerializeField] GameObject weapon;
    [SerializeField] Sprite sprite;
    [SerializeField] Sprite weaponSprite;

    public float GetHealth() { return health; }
    public float GetDamage() { return damage; }
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetSearchTime() { return searchTime; }
    public float GetAggroRange() { return aggroRange; }
    public GameObject GetWeapon() { return weapon; }
    public Sprite GetSprite() { return sprite; }
    public Sprite GetWeaponSprite() { return weaponSprite; }
}
