using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TriggerDestroy : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
