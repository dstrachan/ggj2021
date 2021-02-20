using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private Thruster[] _allThrusters;
    private Thruster[] _leftThrusters;
    private Thruster[] _rearThrusters;
    private Thruster[] _rightThrusters;
    private Thruster[] _forwardThrusters;

    private GameObject _player;

    public float maxSpeed;

    public float shipHealth;
    public float healthMultipler;

    public float shipHealthTotal;

    public Image healthDisplay;

    public bool dead;

    public float score;

    public MultiParticle deadEffect;

    // Start is called before the first frame update
    void Start()
    {
        shipHealth = GetComponentsInChildren<ShipCell>().Where(w=>w.cellType == CellType.Hull).ToArray().Length * healthMultipler;

        shipHealthTotal = shipHealth;
        _player = GameObject.FindGameObjectWithTag(Tags.Player);

        var hdGameObject = GameObject.FindGameObjectWithTag(Tags.ShipHealth);
       
        if (hdGameObject != null)
        {
            healthDisplay = hdGameObject.GetComponent<Image>();
        }

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = SceneManager.GetActiveScene().name == "Shop";
        _allThrusters = GetComponentsInChildren<Thruster>();

        var angles = _allThrusters.Select(w => w.transform.eulerAngles).ToArray();

        _rightThrusters = _allThrusters.Where(w => w.thrustDirection == ThrustDirection.Right).ToArray();
        _leftThrusters = _allThrusters.Where(w => w.thrustDirection == ThrustDirection.Left).ToArray();
        _rearThrusters = _allThrusters.Where(w => w.thrustDirection == ThrustDirection.Forward).ToArray();
        _forwardThrusters = _allThrusters.Where(w => w.thrustDirection == ThrustDirection.Back).ToArray();
    }

    //void OnDrawGizmos()
    //{
    //    // Draws a 5 unit long red line in front of the object
    //    Gizmos.color = Color.red;

    //    if (_player != null)
    //    {
    //        var playerVelocity = _player.GetComponent<Rigidbody>().velocity;

    //        Gizmos.DrawRay(transform.position, playerVelocity);
    //    }
    //}



    private void OnTriggerEnter(Collider collision)
    {
        HitTarget(collision);
    }

    private void HitTarget(Collider collider)
    {
      
        var poi = collider.gameObject.GetComponentInParent<PointOfInterest>();
        if (poi != null)
        {
            score += 1;
            Destroy(poi.gameObject);
            Destroy(poi._arrowInstance);
            Destroy(poi._textInstance);
        }
    }

    private void Update()
    {
        if (healthDisplay != null)
        {
            healthDisplay.fillAmount = (shipHealth / shipHealthTotal);
        }
    }

    public void ShipDead()
    {
        var children = GameObject.FindGameObjectsWithTag("ShipComponent");

        foreach (var child in children)
        {
            child.transform.parent = null;
            var childrb = child.AddComponent<Rigidbody>();

            var dir = Quaternion.AngleAxis(Random.Range(-100, 100), Vector3.up) * Vector3.forward;

            if (childrb != null)
            {
                childrb.AddForce(dir * -Random.Range(100, 400));
                childrb.AddTorque(new Vector3(Random.Range(0, 100), 0, Random.Range(0, 100)));
            }
        }

        var deadEffect = Instantiate(this.deadEffect, transform.position, Quaternion.identity);
        deadEffect.transform.localScale = new Vector3(1, 1, 1);

        deadEffect.GetComponent<AutoDelete>().Started = true;

        dead = true;
        _rigidbody.velocity = new Vector3(0, 0, 0);
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_rigidbody != null && _rigidbody.velocity.magnitude > maxSpeed)
        {
            float brakeSpeed = _rigidbody.velocity.magnitude - maxSpeed;

            Vector3 normalisedVelocity = _rigidbody.velocity.normalized;
            Vector3 brakeVelocity = normalisedVelocity * brakeSpeed;

            _rigidbody.AddForce(-brakeVelocity);  // apply opposing brake force
        }

        if (_allThrusters != null)
        {
            foreach (var thruster in _allThrusters)
            {
                if (thruster.thrustEffect != null)
                {
                    thruster.thrustEffect.gameObject.SetActive(false);
                    thruster.thrustEffect.Stop();
                    thruster.GetComponent<AudioSource>()?.Pause();
                }
            }
        }

        if (!dead)
        {
            if (Input.GetKey(KeyCode.W))
            {
                FireThruster(_rearThrusters);
            }

            if (Input.GetKey(KeyCode.A))
            {
                FireThruster(_leftThrusters);
            }

            if (Input.GetKey(KeyCode.S))
            {
                FireThruster(_forwardThrusters);
            }

            if (Input.GetKey(KeyCode.D))
            {
                FireThruster(_rightThrusters);
            }
        }

    }

    private void FireThruster(Thruster[] thrusters)
    {
        foreach (var thruster in thrusters)
        {
            _rigidbody.AddForceAtPosition(thruster.thrustForce * (thruster.transform.forward) * Time.deltaTime, thruster.transform.position);

            if (thruster.thrustEffect != null)
            {
                thruster.thrustEffect.gameObject.SetActive(true);

                thruster.thrustEffect.Play();
                thruster.GetComponent<AudioSource>()?.Play();
            }
        }
    }
}
