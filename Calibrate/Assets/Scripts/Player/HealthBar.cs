using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject player;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = player.GetComponent<PlayerHealth>().GetHealth();
        slider.value = player.GetComponent<PlayerHealth>().GetHealth();
    }

    // Update is called once per frame
    public void UpdateHealth()
    {
        slider.value = player.GetComponent<PlayerHealth>().GetHealth();
    }
}
