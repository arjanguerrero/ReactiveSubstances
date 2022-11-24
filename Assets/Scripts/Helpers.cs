using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    //-- map sample in range a1-a2 to range b1-b2
    public float Map(float sample, float a1, float a2, float b1, float b2)
    {
        return b1 + (sample - a1) * (b2 - b1) / (a2 - a1);
    }
}
