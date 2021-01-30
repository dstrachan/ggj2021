using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGrid : MonoBehaviour
{
    public Dictionary<(int, int), ShipCell> Cells { get; } = new Dictionary<(int, int), ShipCell>();

    private bool needsUpdate;

    private void Awake()
    {
        Cells[(0, 0)] = GetComponent<ShipCell>();
    }

    private void LateUpdate()
    {
        if (needsUpdate)
        {
            ReorderHierarchy(Cells[(0, 0)].transform, 0);
            needsUpdate = false;
        }
    }

    public ShipCell Get(int x, int y)
    {
        if (Cells.ContainsKey((x, y)))
        {
            return Cells[(x, y)];
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
        Cells[(x, y)] = value;

        needsUpdate = true;
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
