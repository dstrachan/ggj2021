using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ClickableThing : MonoBehaviour
{
    public TestMove testMove;

    void Start()
    {
        testMove = GameObject.FindGameObjectWithTag(Tags.TestMove).GetComponent<TestMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {


                if (hit.transform.gameObject == gameObject)
                {
                    var shipCell = hit.transform.gameObject.GetComponent<ShipCell>();

                    testMove.SetChild(shipCell.cellType);

                    Destroy(gameObject);
                }
            }
        }
    }
}
