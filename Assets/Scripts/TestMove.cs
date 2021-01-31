using UnityEngine;

public class TestMove : MonoBehaviour
{
    private Collider _currentSquare;
    private readonly RaycastHit[] _results = new RaycastHit[2];
    private ShipGrid _grid;

    private GameObject _currentPrefab;
    private GameObject _child;
    private Plane _plane;

    [SerializeField] private GameObject[] _shipPrefabs;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private string _ghostTag;
    [SerializeField] private LayerMask _ghostLayer;
    [SerializeField] private LayerMask _thrustLayer;
    [SerializeField] private string _shopScene;
    [SerializeField] private GameObject _bin;

    private void Awake()
    {
        _grid = FindObjectOfType<ShipGrid>();
        NextPrefab();
    }

    private void Start()
    {
        Physics.gravity = new Vector3(0, -9.1f, 0);

        _grid.UpdateGhosts();
        _plane = new Plane(Vector3.up, 0);
    }

    private void NextPrefab()
    {
        var index = Random.Range(0, _shipPrefabs.Length);
        _currentPrefab = _shipPrefabs[index];
        _child = Instantiate(_currentPrefab, transform.position, _currentPrefab.transform.rotation, transform);
    }

    public void SetChild(CellType cellType)
    {
        if(cellType == CellType.Hull)
        {
            _currentPrefab = _shipPrefabs[0];
        }
        if (cellType == CellType.Gun)
        {
            _currentPrefab = _shipPrefabs[1];
        }
        if (cellType == CellType.Thruster)
        {
            _currentPrefab = _shipPrefabs[0];
        }

        _child = Instantiate(_currentPrefab, transform.position, _currentPrefab.transform.rotation, transform);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _child.SetActive(false);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _child.SetActive(true);
        }

        var scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            if (_child.activeInHierarchy && _child.GetComponent<ShipCell>().cellType != CellType.Hull)
            {
                _child.transform.Rotate(Vector3.up, 90 * scroll);
            }
            else if (_currentSquare != null)
            {
                var thrustDirectionIndicator = _currentSquare.GetComponentInChildren<ThrustDirectionIndicator>();
                thrustDirectionIndicator.transform.Rotate(Vector3.up, 90 * scroll);
                _currentSquare.GetComponent<ShipCell>().thrustDirection = GetThrustDirection(thrustDirectionIndicator.transform.rotation);
            }
        }

        if (_currentSquare == null || _currentSquare.gameObject.layer != LayerMask.NameToLayer("Ghost"))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out var distance))
            {
                var point = ray.GetPoint(distance);
                point.y = 0;

                transform.position = point;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            var ghost = _currentSquare.gameObject;
            if (ghost == _bin)
            {
                Destroy(_child);
            }
            else
            {
                if (!_child.GetComponent<ShipCell>().IsCorrectlyRotated(_currentSquare.GetComponent<ShipCell>()))
                    return;

                var ghostShip = ghost.GetComponent<ShipCell>();
                var shipCell = _child.GetComponent<ShipCell>();
                shipCell.thrustDirection = GetThrustDirection(shipCell.transform.rotation);
                if (shipCell.cellType == CellType.Thruster)
                {
                    shipCell.GetComponent<Thruster>().thrustDirection = shipCell.thrustDirection;
                }
                _grid.Add(ghostShip.x, ghostShip.y, _child);
                _grid.UpdateGhosts();
            }

            _currentSquare.GetComponent<HighlightCell>()?.ResetHighlight();
            _currentSquare = null;

            NextPrefab();
        }
    }

    private ThrustDirection GetThrustDirection(Quaternion rotation) => rotation.eulerAngles.y switch
    {
        0 => ThrustDirection.Forward,
        90 => ThrustDirection.Right,
        180 => ThrustDirection.Back,
        270 => ThrustDirection.Left,
        _ => ThrustDirection.Forward,
    };

    private void FixedUpdate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var n = Physics.RaycastNonAlloc(ray, _results, 100, _ghostLayer | _thrustLayer);
        if (n > 0)
        {
            if (_currentSquare != null)
            {
                _currentSquare.GetComponent<HighlightCell>()?.ResetHighlight();
            }

            var i = 0;
            if (_results[0].collider.gameObject == _child)
            {
                i++; // Skip collision with child
                if (i >= n)
                    return;
            }

            _currentSquare = _results[i].collider;
            if (_currentSquare.gameObject.layer != LayerMask.NameToLayer("Ghost"))
            {
                return;
            }
            else if (_currentSquare.gameObject == _bin || _child.GetComponent<ShipCell>().IsCorrectlyRotated(_currentSquare.GetComponent<ShipCell>()))
            {
                _currentSquare.GetComponent<HighlightCell>()?.HighlightGood();
            }
            else
            {
                _currentSquare.GetComponent<HighlightCell>()?.HighlightBad();
            }

            transform.position = _currentSquare.transform.position; // Snap position
        }
        else if (_currentSquare != null)
        {
            _currentSquare.GetComponent<HighlightCell>()?.ResetHighlight();
            _currentSquare = null;
        }
    }
}
