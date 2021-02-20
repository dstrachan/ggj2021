using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SphereCollider))]
public class Seeker : MonoBehaviour
{
    public bool exclusiveSeeking = false;
    public bool alwaysTargetClosest;
    public bool losesTargetWhenOutOfRange;

    private float targetLostDistance;

    internal SphereCollider targetArea;
    internal Target currentTarget;

    private bool _inert;

    void Start()
    {
        targetArea = GetComponent<SphereCollider>();
        targetLostDistance = targetArea.radius * targetArea.transform.localScale.x;
       
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            targetArea.enabled = false;
            _inert = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!_inert)
        {
            if (currentTarget != null && alwaysTargetClosest)
            {
                var newTarget = other.gameObject.GetComponent<Target>();
                if (Vector3.Distance(newTarget.transform.position, transform.position) < Vector3.Distance(currentTarget.transform.position, transform.position))
                {
                    currentTarget.targeted = false;
                    SetTarget(newTarget);
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!_inert)
        {
            if (currentTarget == null)
            {
                var target = other.gameObject.GetComponent<Target>();
                SetTarget(target);
            }
        }
    }

    void SetTarget(Target target)
    {
        if (target != null && !target.targeted)
        {
            target.targeted = exclusiveSeeking;
            currentTarget = target;
        }
    }

    private void Update()
    {
        if (!_inert)
        {
            if (losesTargetWhenOutOfRange &&
            currentTarget != null &&
            Vector3.Distance(transform.position, currentTarget.transform.position) >= targetLostDistance)
            {
                currentTarget.targeted = false;
                currentTarget = null;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (currentTarget != null)
        {
            Gizmos.DrawRay(transform.position, currentTarget.transform.position - transform.position);
        }
    }

}