using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDelete : MonoBehaviour
{
    private ParticleSystem ps;

    public bool Started;

    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (ps)
        {
            if (Started && !ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
