using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float TurnSpeed;

    private Rigidbody _rigidbody;

    private Thruster[] _leftThrusters;
    private Thruster[] _rearThrusters;
    private Thruster[] _rightThrusters;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        var allThrusters = GetComponentsInChildren<Thruster>();

        _rightThrusters = allThrusters.Where(w => w.transform.eulerAngles.y == 90).ToArray();
        _leftThrusters = allThrusters.Where(w => w.transform.eulerAngles.y == 270).ToArray();
        _rearThrusters = allThrusters.Where(w => w.transform.eulerAngles.y == 0).ToArray();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            foreach (var thruster in _rearThrusters)
            {
                _rigidbody.AddForceAtPosition(thruster.ThrustForce * thruster.transform.forward * Time.deltaTime, thruster.transform.position);
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            foreach (var thruster in _leftThrusters)
            {
                _rigidbody.AddForceAtPosition(thruster.ThrustForce * thruster.transform.forward * Time.deltaTime, thruster.transform.position);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            foreach (var thruster in _rightThrusters)
            {
                _rigidbody.AddForceAtPosition(thruster.ThrustForce * thruster.transform.forward * Time.deltaTime, thruster.transform.position);
            }
        }

    }
}
