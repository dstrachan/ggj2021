using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float TurnSpeed;

    private Rigidbody _rigidbody;

    private Thruster[] _allThrusters;
    private Thruster[] _leftThrusters;
    private Thruster[] _rearThrusters;
    private Thruster[] _rightThrusters;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _allThrusters = GetComponentsInChildren<Thruster>();

        var angles = _allThrusters.Select(w => w.transform.eulerAngles).ToArray();

        _rightThrusters = _allThrusters.Where(w => w.transform.eulerAngles.y == 90).ToArray();
        _leftThrusters = _allThrusters.Where(w => w.transform.eulerAngles.y == 270).ToArray();
        _rearThrusters = _allThrusters.Where(w => w.transform.eulerAngles.y == 0).ToArray();

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var thruster in _allThrusters)
        {
            thruster.LightEffect.enabled = false;
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
            _rigidbody.AddForceAtPosition(thruster.ThrustForce * (thruster.transform.forward) * Time.deltaTime, thruster.transform.position);
            thruster.LightEffect.enabled = true;
        }
    }
}
