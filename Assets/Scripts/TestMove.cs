using UnityEngine;

public class TestMove : MonoBehaviour
{
    private Collider _currentSquare;
    private readonly RaycastHit[] _results = new RaycastHit[1];
    private ShipGrid _grid;

    private GameObject _currentPrefab;
    private GameObject _child;
    private Plane _plane;

    [SerializeField] private GameObject[] _shipPrefabs;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private string _ghostTag;
    [SerializeField] private LayerMask _ghostLayer;
    [SerializeField] private string _shopScene;
    [SerializeField] private GameObject _bin;

    private void Awake()
    {
        _grid = FindObjectOfType<ShipGrid>();
        NextPrefab();
    }

    private void Start()
    {
        _grid.UpdateGhosts();
        _plane = new Plane(Vector3.up, 0);
    }

    private void NextPrefab()
    {
        var index = Random.Range(0, _shipPrefabs.Length);
        _currentPrefab = _shipPrefabs[index];
        _child = Instantiate(_currentPrefab, transform.position, _currentPrefab.transform.rotation, transform);
    }

    private void Update()
    {
        var scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            if (_child.GetComponent<ShipCell>().cellType != CellType.Hull)
            {
                _child.transform.Rotate(Vector3.up, 90 * scroll);
            }
        }

        if (_currentSquare == null)
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

                var shipCell = ghost.GetComponent<ShipCell>();
                shipCell.thrustDirection = shipCell.transform.localRotation.eulerAngles.y switch
                {
                    0 => ThrustDirection.Forward,
                    90 => ThrustDirection.Right,
                    180 => ThrustDirection.Back,
                    270 => ThrustDirection.Left,
                    _ => ThrustDirection.Forward,
                };
                _grid.Add(shipCell.x, shipCell.y, _child);
                _grid.UpdateGhosts();
            }

            _currentSquare.GetComponent<HighlightCell>().ResetHighlight();
            _currentSquare = null;

            NextPrefab();
        }
    }

    private void FixedUpdate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var n = Physics.RaycastNonAlloc(ray, _results, 100, _ghostLayer);
        if (n > 0)
        {
            if (_currentSquare != null)
            {
                _currentSquare.GetComponent<HighlightCell>().ResetHighlight();
            }

            _currentSquare = _results[0].collider;
            if (_currentSquare.gameObject == _bin || _child.GetComponent<ShipCell>().IsCorrectlyRotated(_currentSquare.GetComponent<ShipCell>()))
            {
                _currentSquare.GetComponent<HighlightCell>().HighlightGood();
            }
            else
            {
                _currentSquare.GetComponent<HighlightCell>().HighlightBad();
            }
            transform.position = _currentSquare.transform.position;
        }
        else if (_currentSquare != null)
        {
            _currentSquare.GetComponent<HighlightCell>().ResetHighlight();
            _currentSquare = null;
        }
    }
}
