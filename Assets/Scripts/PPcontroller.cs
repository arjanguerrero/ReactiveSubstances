using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PPcontroller : MonoBehaviour
{
    [SerializeField]
    private VisualEffect plasmaParticles;
    [SerializeField]
    private Lamp lamp;

    private Vector3 lampPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lampPos = lamp.transform.position;
        Debug.Log(lampPos);
        plasmaParticles.SetVector3("SpawnOrigin", lampPos);
    }
}
