using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public GameObject arrow;
    public TextMesh text;

    public float indicatorDistance;

    private GameObject _player;
    private GameObject _arrowInstance;
    private TextMesh _textInstance;
    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player");
        _arrowInstance = Instantiate(arrow);
        _textInstance = Instantiate(text);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (gameObject.transform.position - _player.transform.position).normalized * indicatorDistance;        
        _arrowInstance.transform.position = new Vector3(_player.transform.position.x + dir.x, 0, _player.transform.position.z + dir.z);

        Vector3 textDir = (gameObject.transform.position - _player.transform.position).normalized * (indicatorDistance-1.5f);

        _textInstance.transform.position = new Vector3(_player.transform.position.x + textDir.x, 0, _player.transform.position.z + textDir.z);

        _textInstance.text = $"{Math.Round(Vector3.Distance(gameObject.transform.position, _player.transform.position))}km";

        _arrowInstance.transform.LookAt(gameObject.transform);

    }
}
