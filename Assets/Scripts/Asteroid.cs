using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float distanceToDestroy;

    public float maxRotation;
    public float MinRotation;

    public float maxVelocity;
    public float minVelocity;

    public float accuracyPenalty;

    private Rigidbody _rigidbody;
    private GameObject _player;

    public GameObject prefab;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _player = GameObject.FindGameObjectWithTag("Player");

        Vector3 dir = (gameObject.transform.position - _player.transform.position).normalized;

        // Add some random direction
        dir = Quaternion.AngleAxis(Random.Range(-accuracyPenalty, accuracyPenalty), Vector3.up) * dir;      

        _rigidbody.AddForce(dir * -Random.Range(minVelocity, maxVelocity));
        _rigidbody.AddTorque(new Vector3(Random.Range(maxRotation, MinRotation), 0, Random.Range(maxRotation, MinRotation)));
    }

    void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) > distanceToDestroy)
        {
            Destroy(gameObject);
        }
    }

    public void AsteroidDestroyed()
    {
        if (prefab != null)
        {
            if (transform.localScale.x >= 1.3)
            {
                //var pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                var smallerones = Random.Range(4, 7);

                for (int i = 0; i < smallerones; i++)
                {
                    var asteroid = Instantiate(prefab, transform.position, Quaternion.identity);

                    var scaler = Random.Range(transform.localScale.x / 8f, transform.localScale.x / 4f);
                    asteroid.transform.localScale = new Vector3(transform.localScale.x * scaler, transform.localScale.y * scaler, transform.localScale.z * scaler);
                }
            }

        }
        Destroy(gameObject);

    }
}
