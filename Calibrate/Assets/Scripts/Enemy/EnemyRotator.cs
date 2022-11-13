using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotator : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;

    // Update is called once per frame
    void Update()
    {
        if (enemy.GetComponent<EnemyAI>().isSearching == true)
        {
            var dir = player.transform.position - transform.position;
            dir.Normalize();
            if (dir.x < 0)
            {
                enemy.transform.localScale = new Vector2(-1f, 1f);
            }
            else
            {
                enemy.transform.localScale = new Vector2(1f, 1f);
            }
            float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (enemy.transform.localScale.x == 1)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ+180);
            }
        }
    }
}
