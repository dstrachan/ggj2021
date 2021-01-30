using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    internal float timeAlive;

    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        timeAlive = 0;

    }
    void FixedUpdate()
    {
        timeAlive += Time.deltaTime;

        // Just destroy after a while
        if (timeAlive > 5)
        {
            Destroy(gameObject);
        }

        //var nextPosition = transform.forward * Speed * Time.deltaTime;
        _rigidbody.velocity = transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        HitTarget(collision.collider, collision.GetContact(0).point);
    }


    private void HitTarget(Collider collider, Vector3 collisionPoint)
    {
        var target = collider.gameObject.GetComponent<Target>();

        if (target != null)
        {           
            target.hitPoints -= damage;
            Destroy(gameObject);          
        }
    }

}
