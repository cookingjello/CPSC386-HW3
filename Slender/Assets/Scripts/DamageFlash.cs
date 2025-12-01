/*
    This script was written with the VS Vode build in AI assistance. All prompts associated with the creation of this script
    can be found in the project documentation. 
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    public static DamageFlash Instance { get; private set; }

    [Header("Runtime/Editor - UI setup")]
    [Tooltip("Assign a full-screen UI Image here in the Inspector. This script will use that image to flash red when damaged.")]
    public Image flashImage;

    [Tooltip("How long a single flash lasts (seconds)")]
    public float flashDuration = 0.4f;

    [Tooltip("Default maximum alpha for the flash (0-1). Lower = more transparent.")]
    [Range(0f, 1f)]
    public float defaultMaxAlpha = 0.25f; // more transparent by default

    void Awake()
    {
        // Basic singleton reference for convenient access. Do not auto-create UI elements here — keep setup simple for class.
        if (Instance == null) Instance = this; else if (Instance != this) Destroy(gameObject);

        if (flashImage == null)
        {
            Debug.LogWarning("DamageFlash: no flashImage assigned in inspector. Add a UI Image to use the flash.");
            return;
        }

        // Ensure starting alpha is zero
        var c = flashImage.color;
        c.a = 0f;
        flashImage.color = c;
        flashImage.raycastTarget = false; // don't block clicks
    }

    public void Flash(float duration = -1f, float maxAlpha = -1f)
    {
        if (flashImage == null) return;
        if (duration <= 0f) duration = flashDuration;
        if (maxAlpha < 0f) maxAlpha = defaultMaxAlpha;
        maxAlpha = Mathf.Clamp01(maxAlpha);
        StopAllCoroutines();
        StartCoroutine(DoFlash(duration, maxAlpha));
    }

    IEnumerator DoFlash(float duration, float maxAlpha)
    {
        // Quickly ramp up to maxAlpha then fade out
        float half = Mathf.Max(0.02f, duration * 0.25f);
        float timer = 0f;

        var c = flashImage.color;

        // ramp up
        while (timer < half)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / half;
            c.a = Mathf.Lerp(0f, maxAlpha, t);
            flashImage.color = c;
            yield return null;
        }

        // fade out
        timer = 0f;
        float fade = Mathf.Max(0.02f, duration - half);
        while (timer < fade)
        {
            timer += Time.unscaledDeltaTime;
            float t = 1f - (timer / fade);
            c.a = Mathf.Lerp(0f, maxAlpha, t);
            flashImage.color = c;
            yield return null;
        }

        c.a = 0f;
        flashImage.color = c;
    }
}
