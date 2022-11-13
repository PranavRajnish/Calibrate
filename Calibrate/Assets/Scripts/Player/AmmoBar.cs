using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] Image bulletImage;
    [SerializeField] float spacing;
    [SerializeField] float bulletsPerLine;
    [SerializeField] GunScriptableObject gun;

    private List<Image> bulletList = new List<Image>();

    private float imageWidth;
    // Start is called before the first frame update
    void Start()
    {
        imageWidth = bulletImage.GetComponent<RectTransform>().rect.width;
        for(int i=0; i<gun.GetClipSize(); i++)
        {
            Image bulletObject=Instantiate(bulletImage,GetComponent<RectTransform>().rect.position + new Vector2(i*(imageWidth +spacing),0),Quaternion.identity) as Image;
            bulletObject.transform.SetParent(transform, false);
            bulletObject.transform.localScale = new Vector3(1, 1, 1);
            bulletList.Add(bulletObject);
        }
    }

    public void ShotFiredUI(int curClipSize)
    {
        bulletList[curClipSize].enabled=false;
    }

    public void ReloadUI()
    {
        foreach(Image bullet in bulletList)
        {
            bullet.enabled = true;
        }
    }
}
