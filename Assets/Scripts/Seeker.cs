using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    public CapsuleCollider targetArea;
    internal GameObject CurrentTarget;

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