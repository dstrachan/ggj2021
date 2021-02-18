using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float bulletSpeed;
    public float range;
    public int numberOfBulletsPerShot = 1;
    public float secondsBetweenShots;

    public Transform shootPoint;

    public GameObject bullet;
    public ShipController player;
    public KeyCode ShootKey;

    private float _nextPossibleShootTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<ShipController>();
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
            LoopThisManyIterationsWithDelay(() =>
            {
                var bullet = Instantiate(this.bullet, shootPoint.position, transform.rotation);

                var projectile = bullet.GetComponent<Projectile>();

                projectile.speed = bulletSpeed;
                projectile.damage = damage;
                projectile.range = range;

                var audio = GetComponent<AudioSource>();

                if(audio != null)
                {
                    audio.Play();
                }

            }, numberOfBulletsPerShot, 0.3f);

             _nextPossibleShootTime = Time.time + secondsBetweenShots;

        }
    }
    private void LoopThisManyIterationsWithDelay(Action function, int iterations, float seconds)
    {
        StartCoroutine(LoopFunc(function, iterations,  seconds));
    }

    private IEnumerator LoopFunc(Action function, int iterations, float seconds)
    {
        for (int i = 0; i < iterations; i++)
        {  
            function();

            yield return new WaitForSeconds(seconds);
        }
    }

    private bool CanShoot() => Time.time > _nextPossibleShootTime;
}
