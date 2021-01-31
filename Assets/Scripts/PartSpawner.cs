using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PartSpawner : MonoBehaviour
{

    public GameObject Part;
    public UnityEvent ClickEvent;

    public float spawnTime;
    private float _nextPossibleShootTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_nextPossibleShootTime < Time.time)
        {
            _nextPossibleShootTime = Time.time + spawnTime;
            var partInstance = Instantiate(Part, transform.position, Quaternion.identity);
            var rb = partInstance.AddComponent<Rigidbody>();
            partInstance.AddComponent<ClickableThing>();
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        }

    }
}
