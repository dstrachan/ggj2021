using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public int maxAsteroids;
    public int spawnRadius;

    public float spawnRatePerMinute;

    public GameObject prefab;

    private GameObject _player;
    private float _nextPossibleSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private List<GameObject> asteroids = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if(asteroids.Count < maxAsteroids && CanSpawn())
        {
            var vector2 = UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
            var pos = new Vector3(vector2.x + _player.transform.position.x, 0, vector2.y + _player.transform.position.z);

            var asteroid = Instantiate(prefab, pos, Quaternion.identity);
            asteroid.transform.parent = gameObject.transform;

            asteroids.Add(asteroid);

            _nextPossibleSpawnTime = Time.time + (60.0f/spawnRatePerMinute);

        }
    }

    private bool CanSpawn() => Time.time > _nextPossibleSpawnTime;

}
