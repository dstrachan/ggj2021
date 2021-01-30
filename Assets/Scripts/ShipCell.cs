using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCell : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public ShipCell Forward
    {
        get => _grid.Get(X, Y + 1);
        set => _grid.Set(X, Y + 1, value);
    }
    public ShipCell Back
    {
        get => _grid.Get(X, Y - 1);
        set => _grid.Set(X, Y - 1, value);
    }
    public ShipCell Left
    {
        get => _grid.Get(X - 1, Y);
        set => _grid.Set(X - 1, Y, value);
    }
    public ShipCell Right
    {
        get => _grid.Get(X + 1, Y);
        set => _grid.Set(X + 1, Y, value);
    }

    public bool IsGhost => tag.Equals("Ghost");

    private ShipGrid _grid;

    [SerializeField] private GameObject _ghostPrefab;

    private void Awake()
    {
        _grid = GetComponentInParent<ShipGrid>();
    }

    private void Start()
    {
        if (IsGhost)
            return;

        if (Forward == null)
        {
            Forward = InstantiateNode(_ghostPrefab, Vector3.forward);
        }
        if (Back == null)
        {
            Back = InstantiateNode(_ghostPrefab, Vector3.back);
        }
        if (Left == null)
        {
            Left = InstantiateNode(_ghostPrefab, Vector3.left);
        }
        if (Right == null)
        {
            Right = InstantiateNode(_ghostPrefab, Vector3.right);
        }
    }

    private ShipCell InstantiateNode(GameObject prefab, Vector3 offset)
    {
        var obj = Instantiate(prefab, transform.position + offset, transform.rotation, transform.root);
        var shipCell = obj.GetComponent<ShipCell>();
        if (shipCell != null)
        {
            shipCell.X = X + (int)offset.x;
            shipCell.Y = Y + (int)offset.z;
        }
        return shipCell;
    }

    public void AddNode(GameObject prefab, Vector3 position)
    {
        var direction = transform.position - position;
        if (direction.z < 0)
        {
            var node = InstantiateNode(prefab, Vector3.forward);
            Forward = node;
        }
        else if (direction.z > 0)
        {
            var node = InstantiateNode(prefab, Vector3.back);
            Back = node;
        }
        else if (direction.x > 0)
        {
            var node = InstantiateNode(prefab, Vector3.left);
            Left = node;
        }
        else if (direction.x < 0)
        {
            var node = InstantiateNode(prefab, Vector3.right);
            Right = node;
        }
    }
}
