using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGrid : MonoBehaviour
{
    public Dictionary<(int, int), ShipCell> Cells { get; } = new Dictionary<(int, int), ShipCell>();

    private void Start()
    {
        var shipCell = GetComponent<ShipCell>();
        Set(0, 0, shipCell);
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
    }

    public void ClearHighlight(Color color)
    {
        foreach (var cell in Cells)
        {
            cell.Value.GetComponent<Renderer>().material.color = cell.Value.IsGhost ? color : Color.white;
        }
    }

    public void Highlight(int x, int y, Color color, Color highlightColor)
    {
        ClearHighlight(color);

        var shipCell = Get(x, y);
        if (shipCell != null && shipCell.IsGhost)
        {
            shipCell.GetComponent<Renderer>().material.color = highlightColor;
        }
    }
}
