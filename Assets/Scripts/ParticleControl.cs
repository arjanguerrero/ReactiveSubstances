using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    public float movRange = 10f;
    public float speed = 0.5f;

    [SerializeField]
    private Helpers helpers;
    // public Helpers helpers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // xPos = Mathf.Sin(Time.time * 0.1f);

        // float xPos = movRange * Mathf.PerlinNoise(Time.time * speed, 0.0f);
        float xPos = movRange * helpers.Map(Mathf.PerlinNoise(Time.time * speed, 0.0f), 0f, 1f, -1f, 1f);
        // xPos = helpers.Map(xPos, 0f, 1f, -1f, 1f);
        // Debug.Log("xPos= " + xPos);
        Vector3 pos = transform.position;
        pos.x = xPos;

        transform.position = pos;
    }
}
