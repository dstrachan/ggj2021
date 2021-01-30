using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    private Rigidbody _rigidbody;

    private Thruster[] _allThrusters;
    private Thruster[] _leftThrusters;
    private Thruster[] _rearThrusters;
    private Thruster[] _rightThrusters;

    private GameObject _player;

    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _rigidbody = GetComponent<Rigidbody>();
        _allThrusters = GetComponentsInChildren<Thruster>();

        var angles = _allThrusters.Select(w => w.transform.eulerAngles).ToArray();

        _rightThrusters = _allThrusters.Where(w => w.thrustDirection == ThrustDirection.Right).ToArray();
        _leftThrusters = _allThrusters.Where(w => w.thrustDirection == ThrustDirection.Left).ToArray();
        _rearThrusters = _allThrusters.Where(w => w.thrustDirection == ThrustDirection.Forward).ToArray();

    }

    //void OnDrawGizmos()
    //{
    //    // Draws a 5 unit long red line in front of the object
    //    Gizmos.color = Color.red;

    //    if (_player != null)
    //    {
    //        var playerVelocity = _player.GetComponent<Rigidbody>().velocity;

    //        Gizmos.DrawRay(transform.position, playerVelocity);
    //    }
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude > maxSpeed)
        {
            float brakeSpeed = _rigidbody.velocity.magnitude - maxSpeed;

            Vector3 normalisedVelocity = _rigidbody.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;

            _rigidbody.AddForce(-brakeVelocity);  // apply opposing brake force
        }

        foreach (var thruster in _allThrusters)
        {
            if (thruster.thrustEffect != null)
            {
                thruster.thrustEffect.gameObject.SetActive(false);
                thruster.thrustEffect.Stop();
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            FireThruster(_rearThrusters);
        }

        if (Input.GetKey(KeyCode.A))
        {
            FireThruster(_leftThrusters);
        }

        if (Input.GetKey(KeyCode.D))
        {
            FireThruster(_rightThrusters);
        }

    }

    private void FireThruster(Thruster[] thrusters)
    {
        foreach (var thruster in thrusters)
        {
            _rigidbody.AddForceAtPosition(thruster.thrustForce * (thruster.transform.forward) * Time.deltaTime, thruster.transform.position);

            if (thruster.thrustEffect != null)
            {
                thruster.thrustEffect.gameObject.SetActive(true);

                thruster.thrustEffect.Play();
            }
        }
    }
}
