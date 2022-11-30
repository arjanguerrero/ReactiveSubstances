using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElektraControl : MonoBehaviour
{
    public AudioSource source;
    public AudioClip vibrationClip;

    [SerializeField]
    private Lamp lamp1;

    [SerializeField]
    private Helpers helpers;

    private Material material;
    private float dist;
    private float dispSpeed;
    private float noiseDetail;
    private float noiseScale;
    private float borderPower;

    // public float ReloadTime = 1f; //-- seconds
    // private float ReloadTimer = 0; //-- seconds

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material; //-- properties will be reset after exiting Play mode

        // source.PlayOneShot(vibrationClip); //-- can specify a float volume as second parameter: (vibrationClip, 0.5f)
        
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(lamp1.transform.position, transform.position);
        dispSpeed = helpers.Map(dist, 0f, 6f, 1f, 0f);
        noiseDetail = helpers.Map(dist, 0f, 6f, 1f, 22f);
        noiseScale = helpers.Map(dist, 0f, 6f, 1.3f, 0.005f);
        borderPower = helpers.Map(dist, 0f, 6f, 6.6f, 0f);
        material.SetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d", dispSpeed);
        material.SetFloat("_NoiseDetail", noiseDetail);
        material.SetFloat("Vector1_80680889906d48848eb8aa94785bcd56", noiseScale);
        material.SetFloat("Vector1_e862223f582d4b9e998ad4aa2179934d", borderPower);

        Debug.Log("displacement speed:");
        Debug.Log(material.GetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d")); //-- SurfaceMovementSpeed
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
