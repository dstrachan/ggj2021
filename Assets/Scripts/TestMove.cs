using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public LayerMask ghostLayer;

    private Collider currentSquare;
    private readonly RaycastHit[] results = new RaycastHit[1];
    private ShipGrid _grid;

    [SerializeField] private Material ghostMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private GameObject nodePrefab;

    private void Awake()
    {
        _grid = FindObjectOfType<ShipGrid>();
    }

    private void Update()
    {
        if (currentSquare == null)
        {
            var plane = new Plane(Vector3.up, 0);

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out var distance))
            {
                var point = ray.GetPoint(distance);
                point.y = 0;

                transform.position = point;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            var ghost = currentSquare.gameObject;
            var shipNode = ghost.transform.parent.GetComponent<ShipCell>();

            // TODO: Replace nodePrefab with actual ship node
            shipNode.AddNode(nodePrefab, ghost.transform.position);

            currentSquare.GetComponent<Renderer>().material.color = ghostMaterial.color;
            currentSquare = null;
        }
    }

    private void FixedUpdate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var n = Physics.RaycastNonAlloc(ray, results, 100, ghostLayer);
        if (n > 0)
        {
            if (currentSquare != null)
            {
                currentSquare.GetComponent<Renderer>().material.color = ghostMaterial.color;
            }
            currentSquare = results[0].collider;
            currentSquare.GetComponent<Renderer>().material.color = highlightMaterial.color;
            transform.position = currentSquare.transform.position;
        }
        else if (currentSquare != null)
        {
            currentSquare.GetComponent<Renderer>().material.color = ghostMaterial.color;
            currentSquare = null;
        }
    }
}
