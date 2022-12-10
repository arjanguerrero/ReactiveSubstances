using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    public AudioSource source1;

    [SerializeField]
    private Helpers helpers;

    // Start is called before the first frame update
    void Start()
    {
        source1.volume = 1f; //-- full volume
        source1.Play(); //-- equivalent to checking "Play on Awake" in the Inspector
    }

    // Update is called once per frame
    void Update()
    {
        //-- bond the volume range (0-1) to another range
        //-- check Helpers script
        // sourceELow.volume = helpers.Map(inputValue, rangeAmin, rangeAmax, rangeBmin, rangeBmax);

        // if (condition)
        // {
        //     source1.Stop();
        // }
    }
}
