using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float bulletSpeed;
    public float range;
    public float secondsBetweenShots;

    public Transform shootPoint;

    public GameObject bullet;
    public ShipController player;
    public KeyCode ShootKey;

    private float _nextPossibleShootTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.dead)
        {
            if (Input.GetKey(ShootKey))
            {
                Shoot();
            }
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
            projectile.range = range;

            _nextPossibleShootTime = Time.time + secondsBetweenShots;

            GetComponent<AudioSource>()?.Play();
        }
    }

    private bool CanShoot() => Time.time > _nextPossibleShootTime;
}
