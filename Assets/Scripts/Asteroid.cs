using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    public float distanceToDestroy;
    public float distanceToDestroyBehind;

    public float maxRotation;
    public float minRotation;

    public float maxVelocity;
    public float minVelocity;

    public float accuracyPenalty;

    private Rigidbody _rigidbody;
    private GameObject _player;

    public ParticleSystem deadEffect;

    public GameObject prefab;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _player = GameObject.FindGameObjectWithTag("Player");
        var playerRb = _player.GetComponent<Rigidbody>();


        var target = playerRb.velocity * 5;

        Vector3 dir = (gameObject.transform.position - (_player.transform.position + target)).normalized;

        // Add some random direction
        dir = Quaternion.AngleAxis(Random.Range(-accuracyPenalty, accuracyPenalty), Vector3.up) * dir;      

        _rigidbody.AddForce(dir * -Random.Range(minVelocity, maxVelocity));
        _rigidbody.AddTorque(new Vector3(Random.Range(maxRotation, minRotation), 0, Random.Range(maxRotation, minRotation)));
    }

    void Update()
    {
        var lookAtPlayer = _player.transform.position - transform.position;

        var dot = Vector3.Dot(_player.GetComponent<Rigidbody>().velocity.normalized, lookAtPlayer);

        var destroy = distanceToDestroy;
        if(dot > 0)
        {
            destroy = distanceToDestroyBehind;
        }
        if (Vector3.Distance(_player.transform.position, transform.position) > destroy)
        {
            Destroy(gameObject);
        }
    }

    public void AsteroidDestroyed()
    {
        if (prefab != null)
        {
            if (transform.localScale.x >= 1.4)
            {
                var smallerones = Random.Range(2, 5);

                for (int i = 0; i < smallerones; i++)
                {
                    var asteroid = Instantiate(prefab, transform.position, Quaternion.identity);

                    var scaler = Random.Range(transform.localScale.x / 6f, transform.localScale.x / 4f);

                    asteroid.transform.localScale = new Vector3(transform.localScale.x * scaler, transform.localScale.y * scaler, transform.localScale.z * scaler);
                }
            }

        }

        var deadEffect = Instantiate(this.deadEffect, transform.position, Quaternion.identity);
        deadEffect.GetComponent<AutoDelete>().Started = true;
        Destroy(gameObject);

    }


}
