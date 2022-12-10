using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    [SerializeField]
    private Helpers helpers;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //-- calculate distance between this object and another object
        // float distance = Vector3.Distance(otherObject.transform.position, transform.position);
        //-- normalize distance
        // float normalizedDistance = helpers.Map(distance, 0f, distanceLimit, 1f, 0f); //-- normalized, inverted distance: 1f to 0f

        //-- use time to modify values
        // float usableValue =+ Time.deltaTime; //-- add a stable value (deltaTime) every second
        // float usableValue2 = usableValue * Time.time; //-- multiply value by passing time (in seconds)
    }
}
