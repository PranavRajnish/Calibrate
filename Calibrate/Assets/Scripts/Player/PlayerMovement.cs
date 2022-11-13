using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forces")]
    [SerializeField] float forwardMoveSpeed = 5f;
    [SerializeField] float backwardMoveSpeed = 3.5f;
    [SerializeField] float maxSpeed = 7f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float jumpGrav = 5f;
    [SerializeField] float momentumMultiplier = 7f;
    [SerializeField] float minAirMomentum = 1.5f;
    [SerializeField] float maxAirMomentum = 3f;
    [SerializeField] float upSlopeSpeed = 5f;
    [SerializeField] float downSlopeSpeed = 10f;
    [Range(0, 10)]
    [SerializeField] float airTurnResistance = 5f;
    [Space]
    [SerializeField] float slideResistanceNormal=2f;
    [Space]
    [Header("Controls")]
    [SerializeField] float slopeControlY;
    [SerializeField] float slideControlX;
    [Header("Timers")]
    [SerializeField] float earlyJumpTimer = 0.2f;
    [SerializeField] float airMomentumTimer = 0.5f;
    [SerializeField] float slideTimer = 1f;
    [Space]
    [Header("Colliders")]
    [SerializeField] Vector2 startCollider;
    [SerializeField] Vector2 startColliderOffset;
    [SerializeField] float crouchColliderHeight;
    [SerializeField] float crouchOffsetY;

    Rigidbody2D rb;
    CapsuleCollider2D myFeet;
    BoxCollider2D myBody;
    Animator animator;

    float armLength;

    float curEarlyJumpTimer = 0f;
    float curAirMomentumTimer = 0f;
    float curSlideTimer = 0f;
    float curMoveSpeed;
    float moveVariable;

    bool jumpButtonPressed = false;
    bool crouchButtonPressed = false;
    bool isFacingRight = true;
    bool isTouchingGround = false;
    bool isFacingCrosshair;

    public bool isCrouched = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myFeet = GetComponent<CapsuleCollider2D>();
        myBody = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        startCollider = myBody.size;
        startColliderOffset = myBody.offset;
    }

    private void Update()
    {
        moveVariable = Input.GetAxisRaw("Horizontal");
        curEarlyJumpTimer -= Time.deltaTime;
        curAirMomentumTimer -= Time.deltaTime;
        curSlideTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.W))
        {
            curEarlyJumpTimer = earlyJumpTimer;
            jumpButtonPressed = true;
        }
        if (Input.GetKey(KeyCode.LeftControl)) { crouchButtonPressed = true; }
        else
        {
            crouchButtonPressed = false;
        }
        //if (Input.GetKeyDown(KeyCode.LeftControl)) { animator.SetBool("isCrouched",true);}
        //else if (Input.GetKeyUp(KeyCode.LeftControl)){ animator.SetBool("isCrouched", false);}
    }

    void FixedUpdate()
    {
        //Debug.Log(moveVariable);
        Move(moveVariable);
        if (jumpButtonPressed)
        {
            Jump();
        }
        if (crouchButtonPressed && myFeet.IsTouchingLayers(LayerMask.GetMask("Foreground"))) { Crouch(); }
        else
        {
            myBody.size = startCollider;
            myBody.offset = startColliderOffset;
            isCrouched = false;
            animator.SetBool("isCrouched", false);
        }
        FastFall();
        JumpAnimations();
        SlideAnimations();
    }

    private void Move(float x)
    {

        if (x == 1)
        {
            if (transform.localScale.x == 1)
            {
                animator.SetFloat("isFacingCrosshair", 0);
                curMoveSpeed = forwardMoveSpeed;
            }
            else
            {
                animator.SetFloat("isFacingCrosshair", 1);
                curMoveSpeed = backwardMoveSpeed;
            }
            animator.SetBool("isRunning", true);
        }
        else if (x == -1)
        {
            if (transform.localScale.x == 1)
            {
                animator.SetFloat("isFacingCrosshair", 1);
                curMoveSpeed = backwardMoveSpeed;
            }
            else
            {
                animator.SetFloat("isFacingCrosshair", 0);
                curMoveSpeed = forwardMoveSpeed;
            }
            animator.SetBool("isRunning", true);
        }
        else if (x == 0)
        {
            animator.SetBool("isRunning", false);
        }

        float moveBy = x * SlowAirTurn(curMoveSpeed, x);
        moveBy = KeepAirMomentum(moveBy, x);
        moveBy = SlopeSpeed(moveBy, x);
        moveBy = SlideSpeed(moveBy, x);
        //Debug.Log(moveBy);
        //Debug.Log(rb.velocity);
        //Debug.Log(curAirMomentumTimer);

        if (Mathf.Abs(rb.velocity.x) <= maxSpeed)
        {
            rb.AddForce(new Vector2(moveBy, 0), ForceMode2D.Force);
        }
        else
        {
            rb.velocity = new Vector2((maxSpeed * x), rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Foreground")) && curEarlyJumpTimer > 0)
        {
            curEarlyJumpTimer = 0;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            curEarlyJumpTimer = earlyJumpTimer;
            jumpButtonPressed = false;
        }

    }

    private void FastFall()
    {
        if (rb.velocity.y < -0.2)
        {
            rb.AddForce(new Vector2(0, -jumpGrav));
        }
    }

    private bool IsMovingRight()
    {
        if (rb.velocity.x >= 0) { return true; }
        else { return false; }
    }

    private float SlowAirTurn(float moveSpeed, float x)
    {
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Foreground")) == false)
        {
            curAirMomentumTimer = airMomentumTimer;
            isTouchingGround = false;
            if (IsMovingRight() && x == -1)
            {
                moveSpeed = moveSpeed * Mathf.Exp(-airTurnResistance);
            }
            else if (IsMovingRight() == false && x == 1)
            {
                moveSpeed = moveSpeed * Mathf.Exp(-airTurnResistance);
            }
        }
        return moveSpeed;
    }
    private float KeepAirMomentum(float moveSpeed, float x)
    {
        if (curAirMomentumTimer >= 0 && (myFeet.IsTouchingLayers(LayerMask.GetMask("Foreground")) == true))
        {
            moveSpeed = momentumMultiplier * x * Mathf.Clamp(Mathf.Abs(rb.velocity.x), minAirMomentum, maxAirMomentum);
        }
        return moveSpeed;
    }

    private void Crouch()
    {
        if (isCrouched == false) { animator.SetBool("isCrouched", true); curSlideTimer = slideTimer; }
        //rb.velocity = new Vector2(0f, rb.velocity.y);
        myBody.size = new Vector2(startCollider.x, crouchColliderHeight);
        myBody.offset = new Vector2(startColliderOffset.x, -crouchOffsetY);
        isCrouched = true;
    }

    private void JumpAnimations()
    {
        if (rb.velocity.y >= 0.15 && myFeet.IsTouchingLayers(LayerMask.GetMask("Foreground")) == false) { animator.SetTrigger("isJumping"); }
        else if (rb.velocity.y < 0.15 && myFeet.IsTouchingLayers(LayerMask.GetMask("Foreground")) == false) { animator.SetTrigger("isFalling"); }
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Foreground"))) { animator.SetTrigger("isGrounded"); }
    }

    private float SlopeSpeed(float moveSpeed,float x)
    {
        if(myFeet.IsTouchingLayers(LayerMask.GetMask("Foreground")) == true && Mathf.Abs(rb.velocity.y)>slopeControlY)
        {
            if (rb.velocity.y > 0) { var newSpeed = upSlopeSpeed * x; return newSpeed; }
            else { var newSpeed = downSlopeSpeed * x; return newSpeed; }
        }
        else
        {
            return moveSpeed;
        }
    }
    private void SlideAnimations()
    {
        if (Mathf.Abs(rb.velocity.x) > slideControlX) { animator.SetFloat("isSliding", 1); }
        else { animator.SetFloat("isSliding", 0); }
    }

    private float SlideSpeed(float moveSpeed,float x)
    {
        if (isCrouched)
        {
            if (curSlideTimer >=0)
            {
                moveSpeed = moveSpeed * Mathf.Exp(-slideResistanceNormal);
            }
            else
            {
                moveSpeed = 0;
            }
            return moveSpeed;
        }
        else { return moveSpeed; }
    }
}
