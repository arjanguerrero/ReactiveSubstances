using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLooper : MonoBehaviour
{
    public float stepp = 0.01f;

    private float initXPos;
    private float xPos;
    private float yPos;
    private float zPos;

    // Start is called before the first frame update
    void Start()
    {
        initXPos = transform.position.x;
        xPos = transform.position.x;
        yPos = transform.position.y;
        zPos = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        xPos -= stepp;
        Vector3 newPos = new Vector3(xPos, yPos, zPos);
        transform.position = newPos;

        if (xPos < -11f)
        {
            xPos = initXPos;
        }
    }
}
