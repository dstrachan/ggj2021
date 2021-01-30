using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour
{
    public float HitPoints;
  
    internal bool Alive = true;
    internal Collider Collider;

    public UnityEvent DeadEvents;

    // Use this for initialization
    void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (HitPoints <= 0)
        {
            Alive = false;
            Destroy(gameObject);
            DeadEvents?.Invoke();
        }     
    }   

}