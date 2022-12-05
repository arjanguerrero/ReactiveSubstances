using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElektraControl : MonoBehaviour
{
    public AudioSource sourceELow;
    public AudioSource sourceEHigh;
    public AudioSource sourceBH;
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
    private float minDispScale = 0.1f;
    [SerializeField]
    private float minDispDetail = 22f;
    [SerializeField]
    private float maxDispSpeed = 0.5f;
    [SerializeField]
    private float maxDispScale = 1.2f;
    [SerializeField]
    private float maxDispDetail = 1f;
    private float dispScaleIncrease;
    private float dispDetailIncrease;
    private float dispSpeedIncrease;
    private float dispScale;
    private float dispDetail;
    private float dispSpeed;
    private float movFactor = 0.0008f;

    [Header("Border Power")]
    [SerializeField]
    private float minBorderPow = 0f;
    [SerializeField]
    private float maxBorderPow = 6f;
    private float borderPower;
    private float borderIncrease;

    private Material material;
    private float dist;
    private bool canCharge = true;
    private float chargeFactor = 0.2f;
    private float dischargeFactor = 0.05f;

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
                material.SetFloat("Vector1_80680889906d48848eb8aa94785bcd56", dispScale);
                dispDetailIncrease = movFactor * helpers.Map(borderIncrease, minBorderPow, maxBorderPow, minDispDetail, maxDispDetail); //-- map from borderIncrease
                dispDetail += dispDetailIncrease;
                material.SetFloat("_NoiseDetail", dispDetail);
                dispSpeedIncrease = movFactor * helpers.Map(borderIncrease, minBorderPow, maxBorderPow, minDispSpeed, maxDispSpeed); //-- map from borderIncrease
                dispSpeed += dispSpeedIncrease;
                material.SetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d", dispSpeed); //-- SurfaceMovementSpeed

                //-- displace mesh (mapped to distance)
                dispScale = helpers.Map(dist, 0f, distLimit, maxDispScale, minDispScale);
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
            borderPower = -1.42f;
            material.SetFloat("Vector1_e862223f582d4b9e998ad4aa2179934d", borderPower);
            dispScale = -0.36f;
            material.SetFloat("Vector1_80680889906d48848eb8aa94785bcd56", dispScale);
            dispDetail = 55f;
            material.SetFloat("_NoiseDetail", dispDetail);
            dispSpeed = 0.05f;
            material.SetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d", dispSpeed); //-- SurfaceMovementSpeed

            sourceBH.Play();
            sourceELow.Stop();
            sourceEHigh.Stop();
        }

        if (!canCharge)
        {
            if (borderPower <= 0)
            {
                //-- discharge
                borderPower += Time.deltaTime * dischargeFactor;
                material.SetFloat("Vector1_e862223f582d4b9e998ad4aa2179934d", borderPower);
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

                sourceELow.Play();
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
