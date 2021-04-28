using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    public float damage;
    public float bulletSpeed;
    public float range;
    public int numberOfBulletsPerBurst = 1;
    public float delayDuringBurst = 1;
    public float secondsBetweenShots;

    public Transform shootPoint;

    public GameObject bullet;
    public KeyCode ShootKey;

    private float _nextPossibleShootTime;
    private ShipController _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_player.dead &&  SceneManager.GetActiveScene().name != "Shop")
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
                var bullet = Instantiate(this.bullet, shootPoint.position, shootPoint.rotation);

                var projectile = bullet.GetComponent<Projectile>();
                var rb = bullet.GetComponent<Rigidbody>();

                projectile.speed = bulletSpeed * rb.mass;
                projectile.damage = damage;
                projectile.range = range;

                var audio = GetComponent<AudioSource>();

                if(audio != null)
                {
                    audio.Play();
                }

            }, numberOfBulletsPerBurst, delayDuringBurst);

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
