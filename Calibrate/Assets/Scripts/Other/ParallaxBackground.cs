using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] float parallaxFactor;
    [SerializeField] GameObject displaySprite;
    [SerializeField] Transform[] windowTransforms;
    [SerializeField] float offsetX;

    private List<GameObject> farBackgrounds = new List<GameObject>();

    private Transform cameraTransform;
    private Vector3 lastCameraPos;
    private float textureUnitSizeX;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPos = cameraTransform.position;

        //Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        //Texture2D texture = sprite.texture;
        //textureUnitSizeX = texture.width / sprite.pixelsPerUnit;

        foreach (Transform windowTransform in windowTransforms)
        {
            GameObject farBackground =Instantiate(displaySprite, windowTransform.position, Quaternion.identity) as GameObject;
            farBackground.transform.parent = transform;
            farBackgrounds.Add(farBackground);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaPos = cameraTransform.position - lastCameraPos;
        foreach (GameObject farBackground in farBackgrounds)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(farBackground.transform.position);
            if (screenPoint.x > 0 && screenPoint.y > 0 && screenPoint.x < 1 && screenPoint.y < 1)
            {
                farBackground.transform.position += deltaPos * parallaxFactor;
                lastCameraPos = cameraTransform.position;
                /*if(cameraTransform.position.x-transform.position.x>=textureUnitSizeX)
                {
                    float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                    transform.position = new Vector3(cameraTransform.position.x, transform.position.y);
                }
                */
            }
        }
    }
}
