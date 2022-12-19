using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LiquidBar : MonoBehaviour
{
    #region Fields
    [Tooltip("Bar-Z material")]
    public Material material;
    private Image mask;

    [Header("--- Bar Script Parameters ---")]

    [Tooltip("Do the transition effect automaticaly when the fill amount reach 1")]
    public bool automaticTransitionEffect;

    [Tooltip("Amount of the bar shown, (f.ex: 0.5 is 50%), the bar go to this value more or less quickly depending of the smoothness.")]
    [Range(0, 1)]
    public float targetFillAmount;

    [Tooltip("Defines how fast the bar will go to its target fill amount value")]
    public float smoothness;

    [HideInInspector]
    public float currentFillAmount;

    [Header("--- Shader Parameters ---")]

#if UNITY_EDITOR
    [Tooltip("Only compile in editor, tick to apply all the changes to the material")]
    public bool EDITORUpdateMaterial = true;
#endif

    [Space]
    [Tooltip("Tick this if you'll have more than one instance of this bar. Otherwise the parameters like the fill amount will be shared")]
    public bool instanciatedMaterial = false;

    [Header("Colors")]
    [Tooltip("The bar color, handles transparency too")]
    [ColorUsage(true, true)]
    public Color barColor;

    [Tooltip("The background color, handles transparency too")]
    public Color backgroundColor;

    [Header("UVs")]

    [Range(0.01f, 1)]
    [Tooltip("The resolution of the bar, try 0.2 for a pixelated result")]
    public float resolution = 1;

    [Tooltip("Spherize the UV, usefull with a circle mask")]
    public bool spherize = false;

    [Tooltip("Bar rotation")]
    public Rotation rotation;
    public enum Rotation
    {
        Right,
        Left,
        Up,
        Down
    }

    [Header("Inside Noise")]
    [Tooltip("The scale of the noise inside of the bar")]
    [Range(1, 200)]
    public float insideNoiseScale = 25;

    [Tooltip("Defines how visible is the noise inside the bar")]
    [Range(0, 1)]
    public float insideNoiseIntensity = 0.25f;

    [Tooltip("Defines how detailed is the noise inside the bar")]
    [Range(1, 255)]
    public float insideNoiseColorVariation = 50;

    [Header("Border")]
    [Tooltip("The scale of the noise applied to the border, set to 0 for a straight line")]
    [Range(0, 50)]
    public float borderNoiseScale = 3;

    [Tooltip("The amount of distortion applied to the border, set to 0 for a straight line")]
    [Range(0, 0.3f)]
    public float borderDistortionAmount = 0.1f;

    [Tooltip("Defines how reactive the border light is to the fill amount changes. (f.ex: 100 makes the bar lights up to small value variation)")]
    public float borderLightReactivity = 10;


    private Vector2 pixelSize;
    private Vector2 ratio;
    private bool onTransition;
    
    // Cached Property IDs
    private static readonly int MovingAmount = Shader.PropertyToID("_MovingAmount");
    private static readonly int Progress = Shader.PropertyToID("_Progress");
    private static readonly int DissolveTransition = Shader.PropertyToID("_DissolveTransition");
    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private static readonly int Rotation1 = Shader.PropertyToID("_Rotation");
    private static readonly int PixelSize = Shader.PropertyToID("_PixelSize");
    private static readonly int Ratio = Shader.PropertyToID("_Ratio");
    private static readonly int BorderNoiseScale = Shader.PropertyToID("_BorderNoiseScale");
    private static readonly int Colour = Shader.PropertyToID("_Colour");
    private static readonly int NoiseScale = Shader.PropertyToID("_NoiseScale");
    private static readonly int Spherize = Shader.PropertyToID("_Spherize");
    private static readonly int NoiseIntensity = Shader.PropertyToID("_NoiseIntensity");
    private static readonly int BorderDistortionAmount = Shader.PropertyToID("_BorderDistortionAmount");
    private static readonly int BackgroundColour = Shader.PropertyToID("_BackgroundColour");
    private static readonly int NoiseRoundFactor = Shader.PropertyToID("_NoiseRoundFactor");

    #endregion

    #region Start & Update

    private void Awake()
    {
        if (instanciatedMaterial)
        {
            material = new Material(material);
            UpdateMaterial();
            GetComponent<Image>().material = material;
        }
        else material = GetComponent<Image>().material;

        mask = GetComponent<Image>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (EDITORUpdateMaterial)
        {
            UpdateMaterial();
        }
#endif

        if (automaticTransitionEffect)
        {
            if (currentFillAmount >= 0.99f && !onTransition)
            {
                StartTransition();
            }
        }

        float fillAmountDif = Mathf.Abs(currentFillAmount - targetFillAmount);

        material.SetFloat(MovingAmount, fillAmountDif * borderLightReactivity);

        if (!onTransition)
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * smoothness);

        material.SetFloat(Progress, currentFillAmount);
    }

    #endregion

    #region Methods
    public void StartTransition()
    {
        onTransition = true;
        currentFillAmount = 1;

        StopAllCoroutines();
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            material.SetFloat(DissolveTransition, t);
            yield return null;
        }

        currentFillAmount = 0;
        targetFillAmount = 0;

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            material.SetFloat(DissolveAmount, t);
            yield return null;
        }

        material.SetFloat(DissolveAmount, 0);
        material.SetFloat(DissolveTransition, 0);

        onTransition = false;
    }

    private void UpdateMaterial()
    {
        if (mask == null) mask = GetComponent<Image>();

        switch (rotation)
        {
            case Rotation.Down:
                material.SetFloat(Rotation1, 270);
                break;
            case Rotation.Up:
                material.SetFloat(Rotation1, 90);
                break;
            case Rotation.Left:
                material.SetFloat(Rotation1, 180);
                break;
            case Rotation.Right:
                material.SetFloat(Rotation1, 0);
                break;
        }

        pixelSize = mask.preserveAspect ? mask.sprite.rect.size * resolution : GetComponent<RectTransform>().sizeDelta * resolution;
        ratio = spherize ? Vector2.one : pixelSize.normalized;

        material.SetVector(PixelSize, pixelSize);
        material.SetVector(Ratio, ratio);
        material.SetFloat(BorderNoiseScale, borderNoiseScale);
        material.SetColor(Colour, barColor);
        material.SetFloat(NoiseScale, insideNoiseScale);
        material.SetFloat(Spherize, spherize ? 1 : 0);
        material.SetFloat(NoiseIntensity, insideNoiseIntensity);
        material.SetFloat(BorderDistortionAmount, borderDistortionAmount);
        material.SetColor(BackgroundColour, backgroundColor);
        material.SetFloat(NoiseRoundFactor, insideNoiseColorVariation);
    }

    private void OnApplicationQuit()
    {
        // If the transition occurs and the game is stopped, reset the transition effect.

        material.SetFloat(DissolveAmount, 0);
        material.SetFloat(DissolveTransition, 0);
    }

    #endregion

}
