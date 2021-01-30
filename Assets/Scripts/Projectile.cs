using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public float range;

    private Rigidbody _rigidbody;
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {

        if(Vector3.Distance(_player.transform.position, transform.position) > range)
        {
            Destroy(gameObject);
        }

        //var nextPosition = transform.forward * Speed * Time.deltaTime;
        _rigidbody.velocity = _player.GetComponent<Rigidbody>().velocity + transform.forward * speed * Time.deltaTime;
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
