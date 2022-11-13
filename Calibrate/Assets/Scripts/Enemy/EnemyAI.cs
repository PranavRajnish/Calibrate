using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] EnemyScriptableObject enemy;
    [SerializeField] LayerMask layerMask;

    RaycastHit2D hit;

    private float curSearchTimer=0f;
    public bool isSearching = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curSearchTimer -= Time.deltaTime;
        if (curSearchTimer >= 0) { isSearching = true; }
        else {isSearching = false; }
    }
    public bool CheckAggro()
    {
        hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position).normalized, enemy.GetAggroRange(), layerMask.value);
        if (hit.collider == null)
        {
            return false;
        }
        else if (hit.collider.gameObject.GetComponent<PlayerHealth>() != null)
        {
            curSearchTimer = enemy.GetSearchTime();
            return true;
        }
        else { return false; }
    }
}
