using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCell : MonoBehaviour
{
    public int x { get; private set; }
    public int y { get; private set; }

    public ShipCell Forward
    {
        get => _grid.Get(x, y + 1);
        set => _grid.Set(x, y + 1, value);
    }
    public ShipCell Back
    {
        get => _grid.Get(x, y - 1);
        set => _grid.Set(x, y - 1, value);
    }
    public ShipCell Left
    {
        get => _grid.Get(x - 1, y);
        set => _grid.Set(x - 1, y, value);
    }
    public ShipCell Right
    {
        get => _grid.Get(x + 1, y);
        set => _grid.Set(x + 1, y, value);
    }

    public bool IsGhost => tag.Equals("Ghost");

    private ShipGrid _grid;

    private void Awake()
    {
        _grid = GetComponentInParent<ShipGrid>();
    }

    private ShipCell InstantiateNode(GameObject prefab, Vector3 offset)
    {
        var obj = Instantiate(prefab, transform.position + offset, transform.rotation, transform);
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
            Forward = node;
        }
        else if (z < 0)
        {
            node = InstantiateNode(prefab, Vector3.back);
            Back = node;
        }
        else if (x < 0)
        {
            node = InstantiateNode(prefab, Vector3.left);
            Left = node;
        }
        else if (x > 0)
        {
            node = InstantiateNode(prefab, Vector3.right);
            Right = node;
        }

        return node;
    }

    public void SpawnGhosts(GameObject prefab)
    {
        if (Forward?.IsGhost ?? true)
        {
            Forward = InstantiateNode(prefab, Vector3.forward);
        }
        if (Back?.IsGhost ?? true)
        {
            Back = InstantiateNode(prefab, Vector3.back);
        }
        if (Left?.IsGhost ?? true)
        {
            Left = InstantiateNode(prefab, Vector3.left);
        }
        if (Right?.IsGhost ?? true)
        {
            Right = InstantiateNode(prefab, Vector3.right);
        }
    }
}
