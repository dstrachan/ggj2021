using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Seeker : MonoBehaviour
{
    public bool ExclusiveSeeking = false;

    internal CapsuleCollider targetArea;
    internal GameObject CurrentTarget;

    private void Start()
    {
        targetArea = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CurrentTarget == null)
        {
            var target = other.gameObject.GetComponent<Target>();
            if (target != null && !target.targeted)
            {
                CurrentTarget = target.gameObject;
                target.targeted = ExclusiveSeeking;
            }
        }
    }

    void OnDrawGizmos()
    {

        if (CurrentTarget != null)
        {
            Gizmos.DrawRay(transform.position, CurrentTarget.transform.position);
        }
    }

}