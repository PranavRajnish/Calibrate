using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] int minRotation;
    [SerializeField] int maxRotation;

    private void FixedUpdate()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if (player.GetComponent<PlayerMovement>().isCrouched == false)
        {
            if (player.transform.localScale.x == 1)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180 + rotationZ);
            }
            /*if(rotationZ<-90||rotationZ>90)
            {
                if(player.transform.eulerAngles.y==0)
                {
                    transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
                }
                else if(player.transform.eulerAngles.y==180)
                {
                    transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
                }
            }*/
        }
    }
}
