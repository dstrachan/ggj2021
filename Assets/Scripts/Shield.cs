using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public int shieldSize; 
    public float shieldDamage;
    public float shieldForce;
    public float shieldTotalHitpoints;

    private float _shieldCurrentHitpoints;

    public float damageDoneToShield;

    public ParticleSystem buzzParticles;
    public ParticleSystem shieldBubble;

    private Image _shieldDisplay;
    private SphereCollider sphereCollider;
    private MultiParticle _particleSystem;
    private GameObject _player;

    public AudioSource hitSound;
    public AudioSource powerDown;
    public GameObject emission;

    void Start()
    {
        _shieldCurrentHitpoints = shieldTotalHitpoints;

        _player = GameObject.FindGameObjectWithTag(Tags.Player);
        _shieldDisplay = GameObject.FindGameObjectWithTag(Tags.ShipShield).GetComponent<Image>();

        _particleSystem = GetComponent<MultiParticle>();
        sphereCollider = GetComponent<SphereCollider>();
        var buzz = buzzParticles.shape;
        var shieldBub = shieldBubble.main;
        buzz.radius = shieldSize / 2;
        shieldBub.startSize = shieldSize;
        sphereCollider.radius = shieldSize / 2;
    }

    private void Update()
    {
        if (_shieldDisplay != null)
        {
            _shieldDisplay.fillAmount = (_shieldCurrentHitpoints / shieldTotalHitpoints);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (_shieldCurrentHitpoints > 0)
        {
            var asteroid = other.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                //var target = asteroid.GetComponent<Target>();
                //target.hitPoints -= shieldDamage;

                var rb = asteroid.GetComponent<Rigidbody>();

                _shieldCurrentHitpoints -= rb.mass * damageDoneToShield;

                if(_shieldCurrentHitpoints <= 0)
                {
                    powerDown.Play();

                    if (emission != null)
                    { 
                        var renderer = emission.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material.SetColor("_EmissionColor", Color.gray);
                        }
                    }
                }

                var awayDir = (asteroid.transform.position - _player.transform.position).normalized;

                rb.AddForce(awayDir * shieldForce * rb.mass);

                hitSound.Play();
                _particleSystem.Play();
            }
        }

    }

}