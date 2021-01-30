using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ShipCell))]
public class ShipGrid : MonoBehaviour
{
    public Dictionary<(int, int), ShipCell> cells { get; } = new Dictionary<(int, int), ShipCell>();

    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private string _ghostTag;

    [SerializeField] private GameObject _shipPrefab;
    [SerializeField] private GameObject _hullPrefab;
    [SerializeField] private GameObject _gunPrefab;
    [SerializeField] private GameObject _thrusterPrefab;

    private void Awake()
    {
        if (!_ghostPrefab.CompareTag(_ghostTag))
        {
            throw new Exception($"Expected tag '{_ghostTag}' on {nameof(_ghostPrefab)}.");
        }

        cells[(0, 0)] = GetComponent<ShipCell>();
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
    }

    public void Add(int x, int y, GameObject obj)
    {
        var old = Get(x, y);
        if (old != null)
        {
            obj.transform.parent = transform;
            obj.transform.position = Get(x, y).transform.position;
        }

        var shipCell = obj.GetComponent<ShipCell>();
        shipCell.x = x;
        shipCell.y = y;
        Set(x, y, shipCell);
    }

    public void Remove(int x, int y)
    {
        var shipCell = Get(x, y);
        if (shipCell != null)
        {
            Destroy(shipCell.gameObject);
            cells.Remove((x, y));
        }
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

    public void UpdateGhosts()
    {
        var keys = new List<(int, int)>();
        foreach (var cell in cells)
        {
            switch (cell.Value.cellType)
            {
                case CellType.Core:
                case CellType.Hull:
                    keys.Add(cell.Key);
                    break;
            }
        }

        foreach ((var x, var y) in keys)
        {
            var cell = Get(x, y);
            AddGhost(cell, Vector3.forward);
            AddGhost(cell, Vector3.back);
            AddGhost(cell, Vector3.left);
            AddGhost(cell, Vector3.right);
        }
    }

    private void AddGhost(ShipCell cell, Vector3 offset)
    {
        var x = cell.x + (int)offset.x;
        var y = cell.y + (int)offset.z;
        if (Get(x, y) == null)
        {
            InstantiateCell(x, y, _ghostPrefab, cell.transform.position + offset);
        }
    }

    private void InstantiateCell(int x, int y, GameObject prefab, Vector3 position)
    {
        var obj = Instantiate(prefab, position, prefab.transform.rotation, transform);
        var shipCell = obj.GetComponent<ShipCell>();
        shipCell.x = x;
        shipCell.y = y;
        Set(x, y, shipCell);
    }

    public string ExportToJson()
    {
        var shipData = new ShipDataWrapper();
        foreach (var cell in cells.Values)
        {
            switch (cell.cellType)
            {
                case CellType.Hull:
                case CellType.Gun:
                case CellType.Thruster:
                    shipData.data.Add(new ShipData
                    {
                        cellType = cell.cellType,
                        localPosition = cell.transform.localPosition,
                        rotation = cell.transform.rotation
                    });
                    break;
            }
        }
        return JsonUtility.ToJson(shipData);
    }

    public void ExportToPlayerPrefs(string key) => PlayerPrefs.SetString(key, ExportToJson());
}

[Serializable]
public class ShipData
{
    public Vector3 localPosition;
    public Quaternion rotation;
    public CellType cellType;
}

[Serializable]
public class ShipDataWrapper
{
    public List<ShipData> data = new List<ShipData>();
}
