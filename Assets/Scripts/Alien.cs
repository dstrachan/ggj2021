using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Alien : MonoBehaviour
    {


        void OnCollisionEnter(Collision collision)
        {
            HitTarget(collision.collider, collision.GetContact(0).point);
        }


        private void HitTarget(Collider collider, Vector3 collisionPoint)
        {
            var target = collider.gameObject.GetComponentInParent<ShipController>();

            if (target != null)
            {
                target.score += 1;

                Destroy(gameObject);
            }
        }

    }
}
