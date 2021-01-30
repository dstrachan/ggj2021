using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMove : MonoBehaviour
{
    private Collider _currentSquare;
    private readonly RaycastHit[] _results = new RaycastHit[1];
    private ShipGrid _grid;

    private GameObject _currentPrefab;
    private GameObject _child;

    [SerializeField] private Material _ghostMaterial;
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private GameObject[] _shipPrefabs;
    [SerializeField] private GameObject _ghostPrefab;
    [SerializeField] private string _ghostTag;
    [SerializeField] private LayerMask _ghostLayer;
    [SerializeField] private string _shopScene;

    private void Awake()
    {
        _grid = FindObjectOfType<ShipGrid>();
        NextPrefab();
    }

    private void Start()
    {
        _grid.Get(0, 0).SpawnGhosts(_ghostPrefab);
    }

    private void NextPrefab()
    {
        var index = Random.Range(0, _shipPrefabs.Length);
        _currentPrefab = _shipPrefabs[index];
        if (_child != null)
        {
            Destroy(_child);
        }
        _child = Instantiate(_currentPrefab, transform.position, _currentPrefab.transform.rotation, transform);
    }

    private void Update()
    {
        if (_currentSquare == null)
        {
            var plane = new Plane(Vector3.up, 0);

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out var distance))
            {
                var point = ray.GetPoint(distance);
                point.y = 0;

                transform.position = point;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            var ghost = _currentSquare.gameObject;
            var parentNode = ghost.transform.parent.GetComponent<ShipCell>();

            // TODO: Replace nodePrefab/ghostPrefab with actual ship nodes
            var childNode = parentNode.AddNode(_currentPrefab, ghost.transform.position);
            childNode?.SpawnGhosts(_ghostPrefab);

            _currentSquare.GetComponent<Renderer>().material.color = _ghostMaterial.color;
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
                _currentSquare.GetComponent<Renderer>().material.color = _ghostMaterial.color;
            }
            _currentSquare = _results[0].collider;
            _currentSquare.GetComponent<Renderer>().material.color = _highlightMaterial.color;
            transform.position = _currentSquare.transform.position;
        }
        else if (_currentSquare != null)
        {
            _currentSquare.GetComponent<Renderer>().material.color = _ghostMaterial.color;
            _currentSquare = null;
        }
    }
}
