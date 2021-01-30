using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Tidy/fix highlight
public class TestMove : MonoBehaviour
{
    public LayerMask ghostLayer;

    private Collider currentSquare;
    private readonly RaycastHit[] results = new RaycastHit[5];
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
        else if (Input.GetMouseButton(0))
        {
            var ghost = currentSquare.gameObject;
            var shipNode = ghost.transform.parent.GetComponent<ShipCell>();

            // TODO: Replace nodePrefab with actual ship node
            shipNode.AddNode(nodePrefab, ghost.transform.position);

            currentSquare = null;
        }
    }

    private void FixedUpdate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var n = Physics.RaycastNonAlloc(ray, results, 100, ghostLayer);
        if (n > 0)
        {
            currentSquare = results[0].collider;
            var shipCell = currentSquare.GetComponent<ShipCell>();
            if (shipCell != null)
            {
                _grid.Highlight(shipCell.X, shipCell.Y, ghostMaterial.color, highlightMaterial.color);
            }
            transform.position = currentSquare.transform.position;
        }
        else
        {
            _grid.ClearHighlight(ghostMaterial.color);
            currentSquare = null;
        }
    }
}
