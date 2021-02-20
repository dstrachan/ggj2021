using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    public float lockOnRange;

    public float maxSpeed;

    public Seeker seeker;
    public ParticleSystem thrust;

    public AudioSource boostClip;

    // Start is called before the first frame update
    void Start()
    {

        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.AddForce(transform.forward * speed);

        // Run after a little bit so the missiles look like they pause a minute before engines and guidance start
        this.RunMethodAfterThisManySeconds(() => 
        {
            if (seeker != null && thrust != null) 
            {
                seeker.GetComponent<SphereCollider>().radius = lockOnRange;

                boostClip.Play();
                seeker.gameObject.SetActive(true);

                thrust.gameObject.SetActive(true);
                _rigidbody.AddForce(transform.forward * speed*2);

            }

        }, 0.6f);

    }

    void Update()
    {

        if (seeker != null && seeker.currentTarget != null)
        {
            var dir = (seeker.currentTarget.transform.position - transform.position).normalized;

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
        if (seeker != null && seeker.currentTarget != null)
        {
            transform.LookAt(seeker.currentTarget.transform);
        }
    }

}
