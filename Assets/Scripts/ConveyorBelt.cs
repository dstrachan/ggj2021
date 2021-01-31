using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    //public float speed = 2f;

    //private Rigidbody _rigidbody;

    //private void Start()
    //{
    //    _rigidbody = GetComponent<Rigidbody>();
    //}

    //private void FixedUpdate()
    //{
    //    var distance = transform.forward * speed * Time.deltaTime;
    //    _rigidbody.position -= distance;
    //    _rigidbody.MovePosition(_rigidbody.position + distance);
    //}

    public float speed;

    private float currentScroll;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        currentScroll += Time.deltaTime * speed;
        _renderer.material.mainTextureOffset = new Vector2(0, currentScroll);
    }

    private void OnCollisionStay(Collision collision)
    {
        collision.rigidbody.velocity = transform.forward * speed;
    }
}
