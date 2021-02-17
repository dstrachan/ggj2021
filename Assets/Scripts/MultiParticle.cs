using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MultiParticle : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    private void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    public void Play()
    {
        if (particleSystems.Any())
        {
            foreach (var system in particleSystems)
            {
                system.Play();
            }
        }
    }

    public void Stop()
    {
        if (particleSystems.Any())
        {
            foreach (var system in particleSystems)
            {
                system.Stop();
            }
        }
    }
}