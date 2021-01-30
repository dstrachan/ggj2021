using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThrustDirection
{ 
    Forward,
    Back,
    Left,
    Right,
}
public class Thruster : MonoBehaviour
{
    public float thrustForce;
    public ThrustDirection thrustDirection;

    internal ParticleSystem thrustEffect;

    // Start is called before the first frame update
    void Start()
    {
        thrustEffect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
