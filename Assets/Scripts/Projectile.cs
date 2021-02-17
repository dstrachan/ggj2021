using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    internal float speed;
    internal float damage;
    internal float range;

    public float maxSpeed;

    private Rigidbody _rigidbody;
    private GameObject _player;

    public Seeker Seeker;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");

        _rigidbody.AddForce(transform.forward * speed);


        Gizmos.color = Color.red;

    }

    void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) > range)
        {
            Destroy(gameObject);
        }

        if (Seeker != null && Seeker.CurrentTarget != null)
        {
            var dir = (Seeker.CurrentTarget.transform.position - transform.position).normalized;

            _rigidbody.AddForce(dir * maxSpeed, ForceMode.Force);
            
            if (maxSpeed > 0 && (_rigidbody != null && _rigidbody.velocity.magnitude > maxSpeed))
            {
                float brakeSpeed = _rigidbody.velocity.magnitude - maxSpeed;

                Vector3 normalisedVelocity = _rigidbody.velocity.normalized;
                Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;

                _rigidbody.AddForce(-brakeVelocity);  // apply opposing brake force
            }
          

        }

      

    }

    private void FixedUpdate()
    {
        if (Seeker != null && Seeker.CurrentTarget != null)
        {
            transform.LookAt(Seeker.CurrentTarget.transform);
        }
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
