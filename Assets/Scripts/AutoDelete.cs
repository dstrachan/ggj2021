using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoDelete : MonoBehaviour
{
    private ParticleSystem[] effects;
    private AudioSource[] audioSources;
    private bool somethingStillHappening;

    public bool Started;

    public void Start()
    {
        effects = GetComponentsInChildren<ParticleSystem>();
        audioSources = GetComponentsInChildren<AudioSource>();
    }

    public void Update()
    {
        somethingStillHappening = false;
        if (effects.Any())
        {
            foreach (var effect in effects)
            {
                if(effect.IsAlive())
                {
                    somethingStillHappening = true;
                    break;
                }
            }
        }

        if(audioSources.Any())
        {
            foreach (var audio in audioSources)
            {
                if(audio?.isPlaying ?? false)
                {
                    somethingStillHappening = true;
                    break;
                }
            }
        }

        if(!somethingStillHappening)
        {
            Destroy(gameObject);
        }
    
    }
}
