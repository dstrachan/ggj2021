using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public int maxAsteroids;
    public int spawnRadius;
    public int spawnRatePerMinute;

    public UnityEngine.Object prefab;

    GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private List<GameObject> asteroids = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if(asteroids.Count < maxAsteroids)
        {
            var vector2 = UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
            var pos = new Vector3(vector2.x + _player.transform.position.x, 0, vector2.y + _player.transform.position.z);

            GameObject asteroid = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
            asteroid.transform.parent = gameObject.transform;

            asteroids.Add(asteroid);
        }
    }
}
