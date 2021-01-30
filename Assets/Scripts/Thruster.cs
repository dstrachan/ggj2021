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
    public float ThrustForce;
    public ThrustDirection ThrustDirection;

    internal Light LightEffect;

    // Start is called before the first frame update
    void Start()
    {
        LightEffect = GetComponentInChildren<Light>();
        LightEffect.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
