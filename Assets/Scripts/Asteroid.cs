using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    public float distanceToDestroy;
    public float distanceToDestroyBehind;

    public float babyAsteroidMaxSize;
    public float babyAsteroidMinSize;

    public float accuracyPenalty;

    private Rigidbody _rigidbody;
    private GameObject _player;
    private Target _asteroidTarget;

    public MultiParticle deadEffect;

    public GameObject prefab;

    public float DamageFactor;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _asteroidTarget = GetComponent<Target>();

        _rigidbody = GetComponent<Rigidbody>();

        _asteroidTarget.hitPoints = 50 * _rigidbody.mass;

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

    void OnCollisionEnter(Collision collision)
    {
        HitTarget(collision.collider, collision.GetContact(0).point);
    }

    private void HitTarget(Collider collider, Vector3 collisionPoint)
    {
        var target = collider.gameObject.GetComponentInParent<ShipController>();

        if (target != null)
        {
            target.shipHealth -= DamageFactor * _rigidbody.mass;
            if(target.shipHealth <= 0)
            {
                target.ShipDead();
            }
            else
            {
                AsteroidDestroyed();
            }
         
        }
    }

    public void AsteroidDestroyed()
    {
        if (prefab != null)
        {
            if (transform.localScale.x >= 1.4)
            {
                var smallerones = Math.Max(6,(int)Random.Range(transform.localScale.x, transform.localScale.x*2));

                if (smallerones > 3)
                {
                    for (int i = 0; i < smallerones; i++)
                    {
                        var asteroid = Instantiate(prefab, RandomPosition(), Quaternion.identity);

                        var scaler = Random.Range(babyAsteroidMinSize, babyAsteroidMaxSize);

                        asteroid.transform.localScale = new Vector3(scaler, scaler, scaler);
                        var rigidbody = asteroid.GetComponent<Rigidbody>();
                        rigidbody.mass = scaler / 2f;

                        var away = (_player.transform.position - transform.position).normalized;

                        // bit random
                        away = Quaternion.AngleAxis(Random.Range(0, 30), Vector3.up) * away;

                        rigidbody.AddForce(away * -Random.Range(200 * scaler, 400 * scaler));
                        rigidbody.AddTorque(new Vector3(Random.Range(10 * scaler, 50 * scaler), 0, Random.Range(10 * scaler, 50 * scaler))); ;

                    }
                }
            }

        }


        var deadEffect = Instantiate(this.deadEffect, transform.position, Quaternion.identity);
        deadEffect.transform.localScale = new Vector3(transform.localScale.x*0.1f, transform.localScale.y * 0.1f, transform.localScale.z * 0.1f);
        deadEffect.GetComponent<AutoDelete>().Started = true;
        Destroy(gameObject);

    }

    private Vector3 RandomPosition()
    {
        var vector2 = Random.insideUnitCircle.normalized * Random.Range(-0.3f, 0.3f);

        var pos = new Vector3(vector2.x + transform.position.x, 0, vector2.y + transform.position.z);
        return pos;
    }

}
