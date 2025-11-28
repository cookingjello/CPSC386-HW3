using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Full-screen red flash when the player takes damage. Attach this to a UI GameObject (or it will create one at runtime).
/// Usage: DamageFlash.Instance?.Flash(duration = 0.4f, maxAlpha = 0.8f);
/// </summary>
public class DamageFlash : MonoBehaviour
{
    public static DamageFlash Instance { get; private set; }

    [Header("Runtime/Editor - UI setup")]
    [Tooltip("Optional: assign a UI Image here (full screen) to use for the flash. If null the script will create one at runtime.")]
    public Image flashImage;

    [Tooltip("How long a single flash lasts (seconds)")]
    public float flashDuration = 0.4f;

    [Tooltip("Default maximum alpha for the flash (0-1). Lower = more transparent.")]
    [Range(0f, 1f)]
    public float defaultMaxAlpha = 0.25f; // more transparent by default

    void Awake()
    {
        Debug.Log("DamageFlash: Awake - initializing");
        if (Instance == null) Instance = this; else if (Instance != this) Destroy(gameObject);

        if (flashImage == null)
        {
            CreateRuntimeFlashImage();
            Debug.Log("DamageFlash: created runtime UI for flash");
        }

        if (flashImage != null)
        {
            var c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
            flashImage.raycastTarget = false; // don't block clicks
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Ensure there is an active DamageFlash instance in the scene. Creates one if necessary.
    /// </summary>
    public static DamageFlash EnsureInstance()
    {
        if (Instance != null) return Instance;

        var go = new GameObject("DamageFlashManager");
        // Add component and let its Awake handle creating flash image
        var comp = go.AddComponent<DamageFlash>();
        return comp;
    }

    void CreateRuntimeFlashImage()
    {
        // Create a canvas and fullscreen image so this works even without editor setup.
        var canvasGO = new GameObject("DamageFlashCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000; // draw on top
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        var imageGO = new GameObject("DamageFlashImage");
        imageGO.transform.SetParent(canvasGO.transform, false);
        var image = imageGO.AddComponent<Image>();
        image.color = new Color(1f, 0f, 0f, 0f); // transparent red
        image.rectTransform.anchorMin = Vector2.zero;
        image.rectTransform.anchorMax = Vector2.one;
        image.rectTransform.anchoredPosition = Vector2.zero;
        image.rectTransform.sizeDelta = Vector2.zero;

        flashImage = image;

        // Move manager object under canvas so it survives scene loads but the UI is available
        transform.SetParent(canvasGO.transform, false);
    }

    /// <summary>
    /// Trigger a screen flash. Safe to call repeatedly.
    /// </summary>
    public void Flash(float duration = -1f, float maxAlpha = -1f)
    {
        if (flashImage == null) return;
        if (duration <= 0f) duration = flashDuration;
        if (maxAlpha < 0f) maxAlpha = defaultMaxAlpha;
        maxAlpha = Mathf.Clamp01(maxAlpha);
        StopAllCoroutines();
        Debug.Log($"DamageFlash: Flash called - duration={duration}, maxAlpha={maxAlpha}");
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
