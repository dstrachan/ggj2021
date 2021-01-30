using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    Ghost,
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

    internal int x;
    internal int y;
    public CellType cellType;

    private ShipGrid _grid;

    private void Awake()
    {
        _grid = GetComponentInParent<ShipGrid>();
    }

    public bool IsCorrectlyRotated(ShipCell relativeTo)
    {
        if (cellType == CellType.Hull)
            return true;

        var rotation = transform.rotation.eulerAngles.y;

        // Forward
        if (relativeTo.back?.cellType == CellType.Hull)
        {
            if (rotation == 0)
            {
                if (cellType == CellType.Gun)
                    return true;
            }
            else if (rotation == 180)
            {
                if (cellType == CellType.Thruster)
                    return true;
            }
        }

        // Back
        if (relativeTo.forward?.cellType == CellType.Hull)
        {
            if (rotation == 0)
            {
                if (cellType == CellType.Thruster)
                    return true;
            }
            else if (rotation == 180)
            {
                if (cellType == CellType.Gun)
                    return true;
            }
        }

        // Left
        if (relativeTo.right?.cellType == CellType.Hull)
        {
            if (rotation == 90)
            {
                if (cellType == CellType.Thruster)
                    return true;
            }
            else if (rotation == 270)
            {
                if (cellType == CellType.Gun)
                    return true;
            }
        }

        // Right
        if (relativeTo.left?.cellType == CellType.Hull)
        {
            if (rotation == 90)
            {
                if (cellType == CellType.Gun)
                    return true;
            }
            else if (rotation == 270)
            {
                if (cellType == CellType.Thruster)
                    return true;
            }
        }

        return false;
    }
}
