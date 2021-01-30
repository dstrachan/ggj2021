using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float Damage;
    public float BulletSpeed;
    public float SecondsBetweenShots;

    public Transform ShootPoint;

    public GameObject Bullet;

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
            var bullet = Instantiate(Bullet, ShootPoint.position, transform.rotation);

            var projectile = bullet.GetComponent<Projectile>();

            projectile.Speed = BulletSpeed;
            projectile.Damage = Damage;
     
            _nextPossibleShootTime = Time.time + SecondsBetweenShots;           
        }
    }

    private bool CanShoot() => Time.time > _nextPossibleShootTime;
}
