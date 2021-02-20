using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Turret : MonoBehaviour
{
    private Seeker seeker;
    private Gun[] guns;
    private bool noTarget;

    private void Start()
    {
        seeker = GetComponentInChildren<Seeker>();

        guns = GetComponentsInChildren<Gun>();
        foreach (var gun in guns)
        {
            gun.enabled = false;
        }
    }

    private void Update()
    {
        foreach (var gun in guns)
        {
            gun.enabled = false;
        }

        if (seeker != null && seeker.currentTarget != null)
        {
            foreach (var gun in guns)
            {
                gun.enabled = true;
                gun.Shoot();
            }

            // lead targets a bit 
            var currentTargetRb = seeker.currentTarget.GetComponent<Rigidbody>();

            var target = seeker.currentTarget.transform.position + (currentTargetRb.velocity.normalized);

            transform.LookAt(target);
        }
    }

}