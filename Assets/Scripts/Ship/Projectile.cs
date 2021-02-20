using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    internal float speed;
    internal float damage;
    internal float range;

    public float explosionRange;

    protected Rigidbody _rigidbody;
    protected GameObject _player;

    public MultiParticle hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player);

        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.AddForce(transform.forward * speed);
    }

    private void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) > range)
        {
            Destroy(gameObject);
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
            if (explosionRange > 0)
            {
                Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, explosionRange);
                foreach (var hitCollider in hitColliders)
                {
                    var alsoTarget = hitCollider.GetComponent<Target>();
                    if (alsoTarget)
                    {
                        alsoTarget.hitPoints -= damage * (1.0f/ Vector3.Distance(transform.position, alsoTarget.transform.position));
                    }
                }
            }

            var isMissile = GetComponent<Missile>();
            if (isMissile)
            {
                target.targeted = false;
            }
            target.hitPoints -= damage;

            var deadEffect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            deadEffect.GetComponent<AutoDelete>().Started = true;

            Destroy(gameObject);          
        }
    }

}
