using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipGrid : MonoBehaviour
{
    public Dictionary<(int, int), ShipCell> Cells { get; } = new Dictionary<(int, int), ShipCell>();

    private bool _needsUpdate;
    private ShipCell _root;

    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private string _shopScene;

    private void Awake()
    {
        _root = GetComponent<ShipCell>();
        Cells[(0, 0)] = _root;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals(_shopScene)) // TODO: Better approach?
        {
            _root.SpawnGhosts(_ghostPrefab);
        }
    }

    private void LateUpdate()
    {
        if (_needsUpdate)
        {
            ReorderHierarchy(Cells[(0, 0)].transform, 0);
            _needsUpdate = false;
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
