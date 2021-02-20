using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class ShipData
{
    public Vector3 localPosition;
    public Quaternion rotation;
    public CellType cellType;
    public ThrustDirection thrustDirection;
}

[Serializable]
public class GameData
{
    public List<ShipData> shipData = new List<ShipData>();
    public float score;
    public float totalScore;
}

public class ShipLoader : MonoBehaviour
{
    public static bool clearData = true;
    public GameTimer gameTimer;
    public string shipName = "ship";

    [SerializeField] private GameObject _shipPrefab;
    [SerializeField] private GameObject _hullPrefab;
    [SerializeField] private GameObject _gunPrefab;
    [SerializeField] private GameObject _pointDefencePrefab;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private GameObject _thrusterPrefab;

    private ShipGrid _shipGrid;
    private GameData _gameData;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoad()
    {
        if (clearData)
        {
            PlayerPrefs.DeleteAll(); // This will delete all ships - use with caution.
        }
    }

    private void Awake()
    {
        ImportFromPlayerPrefs(shipName);
        Debug.Log($"Score: {_gameData.score}; Total Score: {_gameData.totalScore}");
    }

    private void Start()
    {
        _shipGrid = FindObjectOfType<ShipGrid>();
    }

    public GameObject Load() => ImportFromPlayerPrefs(shipName);

    public void Save() => ExportToPlayerPrefs(shipName);

    public GameObject ImportFromPlayerPrefs(string key) => ImportFromJson(PlayerPrefs.GetString(key, string.Empty));

    public GameObject ImportFromJson(string json)
    {
        var obj = Instantiate(_shipPrefab);

        var gameData = JsonUtility.FromJson<GameData>(json);
        if (gameData == null)
        {
            _gameData = new GameData();
            gameTimer.SetTimer(_gameData);

            return obj;
        }

        gameTimer.SetTimer(gameData);
        
        var grid = obj.GetComponentInChildren<ShipGrid>();
        foreach (var cell in gameData.shipData)
        {
            switch (cell.cellType)
            {
                case CellType.Hull:
                    CreateAndAdd(obj, grid, cell, _hullPrefab);
                    break;
                case CellType.Gun:
                    CreateAndAdd(obj, grid, cell, _gunPrefab);
                    break;
                case CellType.PointDefense:
                    CreateAndAdd(obj, grid, cell, _pointDefencePrefab);
                    break;
                case CellType.ShieldGenerator:
                    CreateAndAdd(obj, grid, cell, _shieldPrefab);
                    break;
                case CellType.Missile:
                    CreateAndAdd(obj, grid, cell, _missilePrefab);
                    break;
                case CellType.Thruster:
                    var thruster = Instantiate(_thrusterPrefab, cell.localPosition, cell.rotation, obj.transform);
                    grid.Add((int)cell.localPosition.x, (int)cell.localPosition.z, thruster);
                    thruster.GetComponent<ShipCell>().thrustDirection = cell.thrustDirection;
                    thruster.GetComponent<Thruster>().thrustDirection = cell.thrustDirection;
                    thruster.GetComponentInChildren<ThrustDirectionIndicator>().transform.rotation = cell.thrustDirection switch
                    {
                        ThrustDirection.Forward => Quaternion.AngleAxis(0, Vector3.up),
                        ThrustDirection.Right => Quaternion.AngleAxis(90, Vector3.up),
                        ThrustDirection.Back => Quaternion.AngleAxis(180, Vector3.up),
                        ThrustDirection.Left => Quaternion.AngleAxis(270, Vector3.up),
                        _ => Quaternion.identity,
                    };
                    break;
            }
        }

        if (SceneManager.GetActiveScene().name != "Shop")
        {
            foreach (var gameObject in GameObject.FindGameObjectsWithTag("ThrustDirection"))
            {
                gameObject.SetActive(false);
            }

        }

        _gameData = gameData;

        return obj;
    }

    private void CreateAndAdd(GameObject obj, ShipGrid grid, ShipData cell, GameObject prefab)
    {
        var part = Instantiate(prefab, cell.localPosition, cell.rotation, obj.transform);
        grid.Add((int)cell.localPosition.x, (int)cell.localPosition.z, part);
    }

    public void ExportToPlayerPrefs(string key) => PlayerPrefs.SetString(key, ExportToJson());

    public string ExportToJson()
    {
        var gameData = new GameData();

        var score = FindObjectOfType<ShipController>().score;
        gameData.score = score;
        gameData.totalScore = _gameData.totalScore + score;

        foreach (var cell in _shipGrid.cells.Values)
        {
            gameData.shipData.Add(new ShipData
            {
                cellType = cell.cellType,
                localPosition = cell.transform.localPosition,
                rotation = cell.transform.localRotation,
                thrustDirection = cell.thrustDirection,
            });                           
        }

        return JsonUtility.ToJson(gameData);
    }


}
