using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public int maxSpawnRadius;
    public int minSpawnRadius;


    public GameObject prefab;

    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        SpawnPlanet();
        SpawnPlanet();
        SpawnPlanet();
        SpawnPlanet();
    }

    GameObject SpawnPlanet()
    {
        var vector2 = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, maxSpawnRadius);
        var pos = new Vector3(vector2.x + _player.transform.position.x, -10f, vector2.y + _player.transform.position.z);
        var planet = Instantiate(prefab, pos, Quaternion.identity);
        var p = planet.GetComponent<Planet>();
        p.Speed = 8;
        planet.transform.parent = gameObject.transform;

        return planet;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
