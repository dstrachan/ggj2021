using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThrustDirection
{ 
    Forward,
    Left,
    Right
}
public class Thruster : MonoBehaviour
{
    public float thrustForce;
    public ThrustDirection thrustDirection;

    internal Light lightEffect;

    // Start is called before the first frame update
    void Start()
    {
        lightEffect = GetComponentInChildren<Light>();
        if(lightEffect != null)
        {
            lightEffect.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
