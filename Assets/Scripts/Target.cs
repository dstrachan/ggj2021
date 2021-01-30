using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour
{
    public float hitPoints;
  
    internal bool alive = true;
    //internal Collider collider;

    public UnityEvent deadEvents;

    // Use this for initialization
    void Awake()
    {
        //collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (hitPoints <= 0)
        {
            alive = false;

            deadEvents?.Invoke();
        }     
    }   

}