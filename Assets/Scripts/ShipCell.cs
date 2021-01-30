using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    Ghost,
    Core,
    Hull,
    Gun,
    Thruster,
}

public class ShipCell : MonoBehaviour
{
    public ShipCell forward
    {
        get => _grid.Get(x, y + 1);
        set => _grid.Set(x, y + 1, value);
    }
    public ShipCell back
    {
        get => _grid.Get(x, y - 1);
        set => _grid.Set(x, y - 1, value);
    }
    public ShipCell left
    {
        get => _grid.Get(x - 1, y);
        set => _grid.Set(x - 1, y, value);
    }
    public ShipCell right
    {
        get => _grid.Get(x + 1, y);
        set => _grid.Set(x + 1, y, value);
    }

    public int x;
    public int y;
    public CellType cellType;

    private ShipGrid _grid;

    private void Awake()
    {
        _grid = GetComponentInParent<ShipGrid>();
    }
}
