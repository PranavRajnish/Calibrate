using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject enemy;
    [SerializeField] Transform player;
    [SerializeField] float collisonCD;
    [SerializeField] float searchColliderRange;
    [SerializeField] LayerMask layerMask;

    public float moveSpeed;
    private float curCollisionCD;
    
    Rigidbody2D rb;
    BoxCollider2D enemyBody;
    CircleCollider2D searchCollider;
    EnemyAI enemyAI;
    RaycastHit2D hit;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyBody = GetComponent<BoxCollider2D>();
        searchCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        enemyAI= GetComponent<EnemyAI>();
        moveSpeed = enemy.GetMoveSpeed();
        curCollisionCD = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        curCollisionCD -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if(enemyAI.CheckAggro()==false)
        {
            moveSpeed = enemy.GetMoveSpeed();
            animator.SetBool("isShooting", false);
        }
        else 
        {
            moveSpeed = 0;
            animator.SetBool("isShooting",true);
        }
        SearchingCollider();
        EnemyMove();
        CheckWallCollision();

    }

    private void EnemyMove()
    {
        if (IsFacingRight())
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

   /* private void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyAI.isSearching == false)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), 1f);
            moveSpeed = enemy.GetMoveSpeed();
        }
        else
        {
            moveSpeed = 0;
        }
    }
   */
    private void CheckWallCollision()
    {
        if (enemyAI.isSearching)
        {
            moveSpeed = 0;
        }
        else
        {
            if (enemyBody.IsTouchingLayers(LayerMask.GetMask("Foreground")) && curCollisionCD < 0)
            {

                transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), 1f);

                curCollisionCD = collisonCD;
            }
        }
    }
    private void SearchingCollider()
    {
        if (enemyAI.isSearching && searchCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            moveSpeed = 0;
        } 
    }
}
