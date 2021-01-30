using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public float Damage;
    internal float TimeAlive;

    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        TimeAlive = 0;

    }
    void FixedUpdate()
    {
        TimeAlive += Time.deltaTime;

        // Just destroy after a while
        if (TimeAlive > 5)
        {
            Destroy(gameObject);
        }

        //var nextPosition = transform.forward * Speed * Time.deltaTime;
        _rigidbody.velocity = transform.forward * Speed * Time.deltaTime;
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
            target.HitPoints -= Damage;
            if (target.Alive == false)
            {
                Destroy(gameObject);
            }
        }
    }

}
