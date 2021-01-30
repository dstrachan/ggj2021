using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCell : MonoBehaviour
{
    public int x { get; private set; }
    public int y { get; private set; }

    public ShipCell forward
    {
        get => _grid.Get(x, y + 1);
        set => _grid.Set(x, y + 1, value);
    }
    public ShipCell back
    {
        get => _grid.Get(x, y - 1);
        set => _grid.Set(x, y - 1, value);
    }
    public ShipCell left
    {
        get => _grid.Get(x - 1, y);
        set => _grid.Set(x - 1, y, value);
    }
    public ShipCell right
    {
        get => _grid.Get(x + 1, y);
        set => _grid.Set(x + 1, y, value);
    }

    public bool isGhost => tag.Equals("Ghost");

    private ShipGrid _grid;

    private void Awake()
    {
        _grid = GetComponentInParent<ShipGrid>();
    }

    private ShipCell InstantiateNode(GameObject prefab, Vector3 offset)
    {
        var obj = Instantiate(prefab, transform.position + offset, prefab.transform.rotation, transform);
        var shipCell = obj.GetComponent<ShipCell>();
        if (shipCell != null)
        {
            shipCell.x = x + (int)offset.x;
            shipCell.y = y + (int)offset.z;
        }
        return shipCell;
    }

    public ShipCell AddNode(GameObject prefab, Vector3 position)
    {
        var direction = position - transform.position;
        var z = Mathf.Round(direction.z);
        var x = Mathf.Round(direction.x);

        ShipCell node = null;
        if (z > 0)
        {
            node = InstantiateNode(prefab, Vector3.forward);
            forward = node;
        }
        else if (z < 0)
        {
            node = InstantiateNode(prefab, Vector3.back);
            back = node;
        }
        else if (x < 0)
        {
            node = InstantiateNode(prefab, Vector3.left);
            left = node;
        }
        else if (x > 0)
        {
            node = InstantiateNode(prefab, Vector3.right);
            right = node;
        }

        return node;
    }

    public void SpawnGhosts(GameObject prefab)
    {
        if (forward?.isGhost ?? true)
        {
            forward = InstantiateNode(prefab, Vector3.forward);
        }
        if (back?.isGhost ?? true)
        {
            back = InstantiateNode(prefab, Vector3.back);
        }
        if (left?.isGhost ?? true)
        {
            left = InstantiateNode(prefab, Vector3.left);
        }
        if (right?.isGhost ?? true)
        {
            right = InstantiateNode(prefab, Vector3.right);
        }
    }
}
