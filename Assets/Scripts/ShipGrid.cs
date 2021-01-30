using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipGrid : MonoBehaviour
{
    public Dictionary<(int, int), ShipCell> cells { get; } = new Dictionary<(int, int), ShipCell>();

    private bool _needsUpdate;

    private void Awake()
    {
        cells[(0, 0)] = GetComponent<ShipCell>();
    }

    private void LateUpdate()
    {
        if (_needsUpdate)
        {
            ReorderHierarchy(cells[(0, 0)].transform, 0);
            _needsUpdate = false;
        }
    }

    public ShipCell Get(int x, int y)
    {
        if (cells.ContainsKey((x, y)))
        {
            return cells[(x, y)];
        }
        return null;
    }

    public void Set(int x, int y, ShipCell value)
    {
        var shipCell = Get(x, y);
        if (shipCell != null)
        {
            Destroy(shipCell.gameObject);
        }
        cells[(x, y)] = value;

        _needsUpdate = true;
    }

    private void ReorderHierarchy(Transform root, int depth)
    {
        for (var i = 0; i < root.childCount; i++)
        {
            var child = root.GetChild(i);
            ReorderHierarchy(child.transform, depth + 1);
            Debug.Log($"{depth} : {child.GetComponent<ShipCell>()}");
        }
    }
}
