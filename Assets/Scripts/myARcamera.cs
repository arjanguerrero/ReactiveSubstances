using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myARcamera : MonoBehaviour
{
    [SerializeField]
    private Lamp mobileLamp;

    private Quaternion myRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mobileLamp.transform.position = transform.position;

        myRotation = transform.rotation;
        mobileLamp.transform.rotation =myRotation;

        //-- for applying rotation: transform.Rotate(Quaternion.eulerAngles) / transform.Rotate(Vector3)
    }
}
