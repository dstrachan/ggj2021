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

        var obj = Instantiate(_shipPrefab);
        foreach (var cell in shipData.data)
        {
            switch (cell.cellType)
            {
                case CellType.Hull:
                    Instantiate(_hullPrefab, cell.localPosition, cell.rotation, obj.transform);
                    break;
                case CellType.Gun:
                    Instantiate(_gunPrefab, cell.localPosition, cell.rotation, obj.transform);
                    break;
                case CellType.Thruster:
                    Instantiate(_thrusterPrefab, cell.localPosition, cell.rotation, obj.transform);
                    break;
            }
        }

        return obj;
    }

    public GameObject ImportFromPlayerPrefs(string key) => ImportFromJson(PlayerPrefs.GetString(key, string.Empty));
}