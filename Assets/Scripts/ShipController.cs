using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float ThrustForce;
    public float TurnSpeed;

    private Rigidbody _rigidbody;
    private Quaternion _targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            _rigidbody.AddForce(ThrustForce * transform.forward * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.AddForce(ThrustForce * (transform.forward * -1) * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {

            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, transform.eulerAngles.y-1, TurnSpeed * Time.deltaTime);
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, transform.eulerAngles.y+1, TurnSpeed * Time.deltaTime);
        }

    }
}
