using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float maxRotation;
    public float minRotation;

    public float maxVelocity;
    public float minVelocity;

    public float accuracyPenalty;

    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        var player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = (gameObject.transform.position - player.transform.position).normalized;

        // Add some random direction
        dir = Quaternion.AngleAxis(Random.Range(-accuracyPenalty, accuracyPenalty), Vector3.up) * dir;      

        _rigidbody.AddForce(dir * -Random.Range(minVelocity, maxVelocity));
        _rigidbody.AddTorque(new Vector3(Random.Range(maxRotation, minRotation), 0, Random.Range(maxRotation, minRotation)));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
