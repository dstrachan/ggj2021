using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public GameObject arrow;

    public float indicatorDistance;

    private GameObject _player;
    private GameObject _arrowInstance;
    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player");
        _arrowInstance = Instantiate(arrow);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (gameObject.transform.position - _player.transform.position).normalized * indicatorDistance;        
        _arrowInstance.transform.position = 
            new Vector3(_player.transform.position.x + dir.x, 0, _player.transform.position.z + dir.z);

        _arrowInstance.transform.LookAt(gameObject.transform);
    }
}
