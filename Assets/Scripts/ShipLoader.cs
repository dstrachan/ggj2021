using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLoader : MonoBehaviour
{
    [SerializeField] private GameObject _shipPrefab;
    [SerializeField] private GameObject _hullPrefab;
    [SerializeField] private GameObject _gunPrefab;
    [SerializeField] private GameObject _thrusterPrefab;

    private void Awake()
    {
        ImportFromPlayerPrefs("ship");
    }

    public GameObject ImportFromJson(string json)
    {
        var shipData = JsonUtility.FromJson<ShipDataWrapper>(json);
        if (shipData == null)
            return null;

        var obj = Instantiate(_shipPrefab);
        var grid = obj.GetComponentInChildren<ShipGrid>();
        foreach (var cell in shipData.data)
        {
            switch (cell.cellType)
            {
                case CellType.Hull:
                    var hull = Instantiate(_hullPrefab, cell.localPosition, cell.rotation, obj.transform);
                    grid.Add((int)cell.localPosition.x, (int)cell.localPosition.z, hull);
                    break;
                case CellType.Gun:
                    var gun = Instantiate(_gunPrefab, cell.localPosition, cell.rotation, obj.transform);
                    grid.Add((int)cell.localPosition.x, (int)cell.localPosition.z, gun);
                    break;
                case CellType.Thruster:
                    var thruster = Instantiate(_thrusterPrefab, cell.localPosition, cell.rotation, obj.transform);
                    grid.Add((int)cell.localPosition.x, (int)cell.localPosition.z, thruster);
                    thruster.GetComponent<Thruster>().thrustDirection = cell.rotation.eulerAngles.y switch
                    {
                        0 => ThrustDirection.Forward,
                        90 => ThrustDirection.Right,
                        180 => ThrustDirection.Back,
                        270 => ThrustDirection.Left,
                        _ => ThrustDirection.Forward,
                    };
                    break;
            }
        }

        return obj;
    }

    public GameObject ImportFromPlayerPrefs(string key) => ImportFromJson(PlayerPrefs.GetString(key, string.Empty));
}
