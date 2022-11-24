using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShaderController : MonoBehaviour
{

    private Material material;
    private float newSpeed = 10f;
    // private float speedIncrease = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        // material = GetComponent<MeshRenderer>().sharedMaterial;
        material = GetComponent<Renderer>().material; //-- properties will be reset after exiting Play mode
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse pressed");
            // newSpeed = material.GetFloat("Vector1_127c288ab260");
            Debug.Log(material.GetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d")); //-- SurfaceMovementSpeed
            // Debug.Log(material.GetFloat("_SurfaceMovementSpeed"));
            // Debug.Log(material.GetFloat("_SurfaceMovementSpeed"));
            // newSpeed += speedIncrease;
            material.SetFloat("Vector1_127c288ab2604e2ea4bc1d830e541d9d", newSpeed);
        }
    }
}
