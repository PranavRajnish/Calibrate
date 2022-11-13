using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [Header("Grapple Values")]
    [SerializeField] float lineWidth = 0.1f;
    [SerializeField] float speed = 50f;
    [SerializeField] float pullForce = 30f;
    [SerializeField] float pullResistance = 3f;
    [SerializeField] float distThresh = 5f;
    [SerializeField] float maxRange = 70f;
    [SerializeField] float hookOffset;
    [Space]
    [Header("References")]
    [SerializeField] GameObject grappleHookPrefab;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform grapplePos;
    [SerializeField] Material mat;
    [SerializeField] GameObject player;
    [Space]
    public GameObject grappleHook;
  
    private LineRenderer line;
    private Vector3 velocity;
    private Shooter shooter;

    CapsuleCollider2D myBody;

    private float startPullRes;
    private bool shootButtonPressed = false;
    private bool pull=false;
    private bool grappleFired = false;


    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        if (!line) { line = gameObject.AddComponent<LineRenderer>(); }
        myBody = player.GetComponent<CapsuleCollider2D>();
        grappleHook = Instantiate(grappleHookPrefab, transform.position, Quaternion.identity) as GameObject;
        grappleHook.SetActive(false);
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.material = mat;
        transform.position = GetGrapplePos2D();
        startPullRes = pullResistance;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2")) { shootButtonPressed = true; }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        ShootGrapple();
        if (!grappleFired) { return; }
        //CheckPlayerCollision();
        if (pull)
        {
            PullPlayer();
        }
        else
        {
            transform.position += velocity * Time.deltaTime;
            grappleHook.transform.position += velocity * Time.deltaTime;
            float distance = Vector2.Distance(transform.position, GetGrapplePos2D());
            if (distance > maxRange)
            {
                grappleFired = false;
                grappleHook.SetActive(false);
                line.SetPosition(0, Vector2.zero);
                line.SetPosition(1, Vector2.zero);
                return;
            }
        }
        line.SetPosition(0, transform.position);
        line.SetPosition(1, GetGrapplePos2D());
    }

    private void MoveGrapple()
    {
        
    }
    private void PullPlayer()
    {
        Vector2 dir = (Vector2)transform.position - GetGrapplePos2D();
        var dist = Vector2.Distance(transform.position, GetGrapplePos2D());
        dir.Normalize();
        /*if (dist < distThresh) { pullResistance = dist; }
        else { pullResistance = startPullRes; }
        var finalForce = pullForce * (dist / pullResistance);*/

        rb.AddForce(dir * pullForce);
    }

    public void ShootGrapple()
    {
        if (shootButtonPressed)
        {
            if (grappleFired == false)
            {
                GameObject crosshair = player.GetComponent<Shooter>().crosshair;
                //var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var worldPos = crosshair.transform.position;
                Vector2 dir = (Vector2)worldPos - GetGrapplePos2D();
                dir.Normalize();
                float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                velocity = dir * speed;
                transform.position = GetGrapplePos2D() + dir;
                grappleHook.SetActive(true);
                grappleHook.transform.position = GetGrapplePos2D() + hookOffset * dir;
                grappleHook.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                pull = false;
                grappleFired = true;
            }
            else if (grappleFired == true)
            {
                line.SetPosition(0, Vector2.zero);
                line.SetPosition(1, Vector2.zero);
                grappleHook.SetActive(false);
                pull = false;
                grappleFired = false;
            }
            shootButtonPressed = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        velocity = Vector2.zero;
        pull = true;
    }
    public void CheckPlayerCollision()
    {
       if( myBody.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            pull = false;
        }
    }

    public Vector2 GetGrapplePos2D() {return new Vector2(grapplePos.position.x, grapplePos.position.y); }
}
