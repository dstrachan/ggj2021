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

    public float shopTime = 30;
    public float flightTime = 60;
    public float scoreMultiplier = 15;
    public string shipName = "ship";

    [SerializeField] private GameObject _shipPrefab;
    [SerializeField] private GameObject _hullPrefab;
    [SerializeField] private GameObject _gunPrefab;
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
            SetTimer(_gameData);
            return obj;
        }

        SetTimer(gameData);

        var grid = obj.GetComponentInChildren<ShipGrid>();
        foreach (var cell in gameData.shipData)
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

    public void ExportToPlayerPrefs(string key) => PlayerPrefs.SetString(key, ExportToJson());

    public string ExportToJson()
    {
        var gameData = new GameData();

        var score = FindObjectOfType<ShipController>().score;
        gameData.score = score;
        gameData.totalScore = _gameData.totalScore + score;

        foreach (var cell in _shipGrid.cells.Values)
        {
            switch (cell.cellType)
            {
                case CellType.Hull:
                case CellType.Gun:
                case CellType.Thruster:
                    gameData.shipData.Add(new ShipData
                    {
                        cellType = cell.cellType,
                        localPosition = cell.transform.localPosition,
                        rotation = cell.transform.localRotation,
                        thrustDirection = cell.thrustDirection,
                    });
                    break;
            }
        }

        return JsonUtility.ToJson(gameData);
    }

    private void SetTimer(GameData gameData)
    {
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            FindObjectOfType<GameTimer>().TimeLeftSeconds = shopTime + ((gameData.score + 1) * scoreMultiplier);
        }
        else
        {
            FindObjectOfType<GameTimer>().TimeLeftSeconds = flightTime + ((gameData.score + 1) * scoreMultiplier);
        }
    }
}
