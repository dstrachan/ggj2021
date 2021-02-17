using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedMusicPlay : MonoBehaviour
{

    public float delay = 1.0f;
    void Start()
    {
        GetComponent<AudioSource>().PlayDelayed(delay);
    }

    // Update is called once per frame
    void Update()
    {
        // time_passed += Time.deltaTime;
        // if(time_passed >= delay){
        // }
    }
}
