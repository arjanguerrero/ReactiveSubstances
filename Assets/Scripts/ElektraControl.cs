using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElektraControl : MonoBehaviour
{
    public AudioSource sourceELow;
    public AudioSource sourceEHigh;
    public AudioSource sourceBH;
    public AudioSource sourceImplosion;
    public AudioClip vibrationClip;

    [SerializeField]
    private Lamp lamp1;

    [SerializeField]
    private Helpers helpers;

    [SerializeField]
    private float distLimit = 6f;

    [Header("Mesh Displacement")]
    [SerializeField]
    private float minDispSpeed = 0.005f;
    [SerializeField]
    private float maxDispSpeed = 0.05f;
    [SerializeField]
    private float minDispScale = 0.1f;
    [SerializeField]
    private float maxDispScale = 1.5f;
    [SerializeField]
    private float minDispDetail = 0.2f;
    [SerializeField]
    private float maxDispDetail = 360f;
    private float dispScaleIncrease;
    private float dispDetailIncrease;
    private float dispSpeedIncrease;
    private float dispScale;
    private float dispDetail;
    private float dispSpeed;
    private float movFactor = 0.0008f;

    [Header("Border Power")]
    [SerializeField]
    private float minBorderPow = 1f;
    [SerializeField]
    private float maxBorderPow = 6f;
    private float borderPower;
    private float borderIncrease;

    private Material material;
    private float dist;
    private bool canCharge = true;
    private float chargeFactor = 0.2f;
    private float dischargeFactor = 0.05f;

    //-- Black Hole settings
    private float bhBorderPower = -1.42f;
    private float bhDispScale = -0.36f;
    private float bhDispDetail = 55f;
    private float bhDispSpeed = 0.05f;

    // public float ReloadTime = 1f; //-- seconds
    // private float ReloadTimer = 0; //-- seconds

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material; //-- properties will be reset after exiting Play mode

        borderPower = minBorderPow;

        // source.PlayOneShot(vibrationClip); //-- can specify a float volume as second parameter: (vibrationClip, 0.5f)
        
        sourceELow.Play();
        sourceELow.volume = 1f;
        sourceEHigh.Play();
        sourceEHigh.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(lamp1.transform.position, transform.position);

        if (canCharge)
        {
            if (dist <= distLimit)
            {
                //-- charge
                float normDistance = helpers.Map(dist, 0f, distLimit, 1f, 0f); //-- normalized, inverted distance: 1f to 0f
                borderIncrease = (normDistance * Time.deltaTime) * chargeFactor; //-- multiply this by incFactor to acc/dec increase
                // Debug.Log("normDistance: " + normDistance);
                // Debug.Log("distance: " + dist);
                // Debug.Log("borderIncrease: " + borderIncrease);
                borderPower += borderIncrease;
                material.SetFloat("Vector1_e862223f582d4b9e998ad4aa2179934d", borderPower);

                //-- displace mesh (cumulative)
                // dispScaleIncrease = movFactor * helpers.Map(borderIncrease, minBorderPow, maxBorderPow, minDispScale, maxDispScale); //-- map from borderIncrease
                // Debug.Log("dispScaleIncrease: " + dispScaleIncrease);
                // dispScale += dispScaleIncrease;
                // material.SetFloat("Vector1_80680889906d48848eb8aa94785bcd56", dispScale);
                dispDetail = helpers.Map(borderPower, minBorderPow, maxBorderPow, minDispDetail, maxDispDetail);
                material.SetFloat("_NoiseDetail", dispDetail);
                dispSpeed = helpers.Map(borderPower, minBorderPow, maxBorderPow, minDispSpeed, maxDispSpeed);
                material.SetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d", dispSpeed); //-- SurfaceMovementSpeed

                //-- displace mesh (mapped to distance)
                float normBorderPow = helpers.Map(borderPower, minBorderPow, maxBorderPow, 0.3f, 1f);
                dispScale = normBorderPow * helpers.Map(dist, 0f, distLimit, maxDispScale, minDispScale);
                material.SetFloat("Vector1_80680889906d48848eb8aa94785bcd56", dispScale);
                // dispDetail = helpers.Map(dist, 0f, distLimit, maxDispDetail, minDispDetail);
                // material.SetFloat("_NoiseDetail", dispDetail);
                // dispSpeed = helpers.Map(dist, 0f, distLimit, maxDispSpeed, 0f);
                // material.SetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d", dispSpeed); //-- SurfaceMovementSpeed
            }
            // else
            // {
            //     //-- don't charge and keep standard mesh
            //     dispScale = minDispScale;
            //     dispDetail = minDispDetail;
            // }

            sourceELow.volume = helpers.Map(borderPower, minBorderPow, maxBorderPow, 1f, 0f);
            sourceEHigh.volume = helpers.Map(borderPower, minBorderPow, maxBorderPow, 0f, 0.8f);
        }

        if (canCharge && borderPower >= maxBorderPow)
        {
            canCharge = false;
            //-- go blackhole
            borderPower = bhBorderPower;
            material.SetFloat("Vector1_e862223f582d4b9e998ad4aa2179934d", borderPower);
            dispScale = bhDispScale;
            material.SetFloat("Vector1_80680889906d48848eb8aa94785bcd56", dispScale);
            dispDetail = bhDispDetail;
            material.SetFloat("_NoiseDetail", dispDetail);
            dispSpeed = bhDispSpeed;
            material.SetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d", dispSpeed); //-- SurfaceMovementSpeed

            sourceImplosion.Play();
            sourceBH.Play();
            // sourceELow.Stop();
            sourceEHigh.Stop();
        }

        if (!canCharge)
        {
            if (borderPower <= 0f)
            {
                //-- discharge
                borderPower += Time.deltaTime * dischargeFactor;
                material.SetFloat("Vector1_e862223f582d4b9e998ad4aa2179934d", borderPower);
                dispDetail = helpers.Map(borderPower, -1.42f, 0f, 55f, minDispDetail);
                material.SetFloat("_NoiseDetail", dispDetail);

                sourceBH.volume = helpers.Map(borderPower, bhBorderPower, 0f, 1f, 0.2f);
                float powFraction = bhBorderPower / 3f;
                if (borderPower >= powFraction)
                {
                    sourceELow.volume = helpers.Map(borderPower, powFraction, 0f, 0f, 1f);
                }
            }
            else
            {
                //-- reset
                canCharge = true;
                dispScale = minDispScale;
                material.SetFloat("Vector1_80680889906d48848eb8aa94785bcd56", dispScale);
                dispDetail = minDispDetail;
                material.SetFloat("_NoiseDetail", dispDetail);
                dispSpeed = minDispSpeed;
                material.SetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d", dispSpeed); //-- SurfaceMovementSpeed

                // sourceELow.Play();
                sourceEHigh.Play();
                sourceBH.Stop();
            }
        }

        //

        // Debug.Log(material.GetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d")); //-- SurfaceMovementSpeed

        //----------TIMER-----------
        //-- every second, this variable decreases by 1
        // ReloadTimer -= Time.deltaTime; // 1/fps

        // if (ReloadTimer <= 0 &&
        //     anotherCondition)
        // {
        //     //-- excecute something
        //     //-- and reset timer
        //     ReloadTimer = ReloadTime;
        // }
    }
}
