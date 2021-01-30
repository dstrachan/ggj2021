using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float distanceToDestroy;

    public float maxRotation;
    public float minRotation;

    public float maxVelocity;
    public float minVelocity;

    public float accuracyPenalty;

    private Rigidbody _rigidbody;
    private GameObject _player;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = (gameObject.transform.position - _player.transform.position).normalized;

        // Add some random direction
        dir = Quaternion.AngleAxis(Random.Range(-accuracyPenalty, accuracyPenalty), Vector3.up) * dir;      

        _rigidbody.AddForce(dir * -Random.Range(minVelocity, maxVelocity));
        _rigidbody.AddTorque(new Vector3(Random.Range(maxRotation, minRotation), 0, Random.Range(maxRotation, minRotation)));
    }

    void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) > distanceToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
