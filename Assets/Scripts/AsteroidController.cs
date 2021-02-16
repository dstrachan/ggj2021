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

    public float spawnRatePerMinute;

    public GameObject prefab;

    private GameObject _player;
    private float _nextPossibleSpawnTime;

    private int AsteroidCount = 0;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        AsteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Length;

        //Update asteroid count every 2 seconds
        RunEachThisManySeconds(() => AsteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Length, 2);
    }

    void Update()
    {
        if (AsteroidCount < maxAsteroids && CanSpawn())
        {
            var vector2 = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, maxSpawnRadius);

            var pos = new Vector3(vector2.x + _player.transform.position.x, 0, vector2.y + _player.transform.position.z);

            var asteroid = Instantiate(prefab, pos, Quaternion.identity);
            asteroid.transform.parent = gameObject.transform;

            var scaler = (sizeDistribution.Evaluate(Random.value) * maxSize-minSize) + minSize;

            asteroid.transform.localScale = new Vector3(scaler, scaler, scaler);

            var rigidbody = asteroid.GetComponent<Rigidbody>();
            rigidbody.mass = asteroid.transform.localScale.x * scaler;

            // Add some random direction
            var dir = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * transform.forward;

            rigidbody.AddForce(dir * -Random.Range(minVelocity* scaler, maxVelocity* scaler));
            rigidbody.AddTorque(new Vector3(Random.Range(maxRotation * scaler, minRotation * scaler), 0, Random.Range(maxRotation * scaler, minRotation * scaler)));

            _nextPossibleSpawnTime = Time.time + (60.0f / spawnRatePerMinute);
        }
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
