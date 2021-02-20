using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour
{
    public AnimationCurve sizeDistribution;
    public float maxSize;
    public float minSize;

    public int maxAsteroids;
    public int maxSpawnRadius;
    public int minSpawnRadius;

    public float maxVelocity;
    public float minVelocity;

    public float maxRotation;
    public float minRotation;

    public float distanceToDestory = 200;
    public float distanceToDestroyBehind = 80;

    public float spawnRatePerMinute;

    public GameObject center;

    public GameObject prefab;

    private float _nextPossibleSpawnTime;

    private GameObject[] asteroids;

    void Start()
    {
        center = GameObject.FindGameObjectWithTag(Tags.Player);
        asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        //Update asteroid count every 2 seconds
        RunEachThisManySeconds(() => asteroids = GameObject.FindGameObjectsWithTag("Asteroid"), 5);
    }

    void Update()
    {
        if (asteroids.Length < maxAsteroids && CanSpawn())
        {
            var asteroid = Instantiate(prefab, new Vector3(1000,1000,1000), Quaternion.identity);
            asteroid.transform.parent = gameObject.transform;

            RandomlyPlaceAsteroid(asteroid);

            _nextPossibleSpawnTime = Time.time + (60.0f / spawnRatePerMinute);
        }

        foreach (var asteroid in asteroids)
        {
            MoveAsteroidBackToPlayer(asteroid);
        }
    }

    private void MoveAsteroidBackToPlayer(GameObject asteroid)
    {
        if (center != null && asteroid != null)
        {
            var lookAtPlayer = center.transform.position - asteroid.transform.position;

            var dot = Vector3.Dot(center.GetComponent<Rigidbody>().velocity.normalized, lookAtPlayer);

            var destroyDistance = distanceToDestory;
            if (dot > 0)
            {
                destroyDistance = distanceToDestroyBehind;
            }
            if (Vector3.Distance(center.transform.position, asteroid.transform.position) > destroyDistance)
            {
                RandomlyPlaceAsteroid(asteroid);
            }
        }
    }

    private void RandomlyPlaceAsteroid(GameObject asteroid)
    {
        var vector2 = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, maxSpawnRadius);

        var pos = new Vector3(vector2.x + center.transform.position.x, 0, vector2.y + center.transform.position.z);

        asteroid.transform.position = pos;

        var scaler = sizeDistribution.RandomFromCurve(minSize, maxSize);

        asteroid.transform.localScale = new Vector3(scaler, scaler, scaler);

        var rigidbody = asteroid.GetComponent<Rigidbody>();
        rigidbody.mass = asteroid.transform.localScale.x * scaler;

        asteroid.GetComponent<Target>().hitPoints = 50 * rigidbody.mass;

        // Add some random direction
        var dir = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * transform.forward;

        rigidbody.AddForce(dir * Random.Range(minVelocity, maxVelocity));
        rigidbody.AddTorque(new Vector3(Random.Range(maxRotation * scaler, minRotation * scaler), 0, Random.Range(maxRotation * scaler, minRotation * scaler)));
    }


    private bool CanSpawn() => Time.time > _nextPossibleSpawnTime;

    private void RunEachThisManySeconds(Action function, float seconds)
    {
        StartCoroutine(LoopFunc(function, seconds));
    }

    private IEnumerator LoopFunc(Action function, float seconds)
    {
        while (true)
        {
            function();

            yield return new WaitForSeconds(seconds);
        }
    }
}
