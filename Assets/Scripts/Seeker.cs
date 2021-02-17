using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Seeker : MonoBehaviour
{
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
            if (other.gameObject.GetComponent<Target>())
            {
                CurrentTarget = other.gameObject;
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