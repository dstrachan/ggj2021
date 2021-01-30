using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Planet : MonoBehaviour
{
    public float Speed;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddTorque(new Vector3(0, 0, Speed));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
