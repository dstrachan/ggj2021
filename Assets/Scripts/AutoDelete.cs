using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDelete : MonoBehaviour
{
    private ParticleSystem ps;
    private AudioSource audioSource;

    public bool Started;

    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (ps)
        {
            if (Started && !ps.IsAlive() && (audioSource?.isPlaying ?? false))
            {
                Destroy(gameObject);
            }
        }
    }
}
