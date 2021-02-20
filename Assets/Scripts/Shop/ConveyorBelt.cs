using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
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
