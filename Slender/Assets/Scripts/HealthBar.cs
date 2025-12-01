/*
    This script was written using this video as guidance: 
    https://youtu.be/XOjd_qU2Ido?si=Q-KN53rnNj4Z9xVc 
    YouTub channel: Brackeys
*/


using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
