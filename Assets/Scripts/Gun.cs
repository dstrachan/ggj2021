using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float bulletSpeed;
    public float secondsBetweenShots;

    public Transform shootPoint;

    public GameObject bullet;

    private float _nextPossibleShootTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (CanShoot())
        {        
            var bullet = Instantiate(this.bullet, shootPoint.position, transform.rotation);

            var projectile = bullet.GetComponent<Projectile>();

            projectile.speed = bulletSpeed;
            projectile.damage = damage;
     
            _nextPossibleShootTime = Time.time + secondsBetweenShots;           
        }
    }

    private bool CanShoot() => Time.time > _nextPossibleShootTime;
}
