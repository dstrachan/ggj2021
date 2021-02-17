using UnityEngine;


public class Shield : MonoBehaviour
{
    private SphereCollider sphereCollider;

    private MultiParticle particleSystem;

    void Start()
    {
        particleSystem = GetComponent<MultiParticle>();
    }

    void OnTriggerEnter(Collider other)
    {
        var asteroid = other.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            asteroid.AsteroidDestroyed();
            particleSystem.Play();
        }

    }

}