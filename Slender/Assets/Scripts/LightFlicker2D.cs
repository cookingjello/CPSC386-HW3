using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker2D : MonoBehaviour
{
    private Light2D light2d;

    [Range(0f, 2f)]
    public float flickerAmount = 0.3f;

    public float speed = 10f;

    float baseIntensity;

    void Start()
    {
        light2d = GetComponent<Light2D>();
        baseIntensity = light2d.intensity;
    }

    void Update()
    {
        light2d.intensity = baseIntensity + Mathf.Sin(Time.time * speed) * flickerAmount;
    }
}
