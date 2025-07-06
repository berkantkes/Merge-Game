using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private List<SingleGridController> _grids;
    
    private SingleGridController[,] gridArray;

    public void Initialize()
    {
        int maxX = 0;
        int maxY = 0;

        foreach (SingleGridController grid in _grids)
        {
            if (grid.GetGridX() > maxX) maxX = grid.GetGridX();
            if (grid.GetGridY() > maxY) maxY = grid.GetGridY();
        }

        int width = maxX + 1;
        int height = maxY + 1;

        gridArray = new SingleGridController[width, height];

        foreach (SingleGridController grid in _grids)
        {
            int x = grid.GetGridX();
            int y = grid.GetGridY();

            if (gridArray[x, y] != null)
            {
                Debug.LogWarning($"Duplicate grid at ({x},{y}) detected. It will be overwritten.");
            }

            gridArray[x, y] = grid;
        }

        Debug.Log($"Grid initialized with size: {width}x{height} | Total cells: {gridArray.Length}");
    }
    
    public SingleGridController GetGridAt(int x, int y)
    {
        if (gridArray == null)
        {
            Debug.LogError("Grid array is not initialized.");
            return null;
        }

        if (x >= 0 && x < gridArray.GetLength(0) && y >= 0 && y < gridArray.GetLength(1))
        {
            return gridArray[x, y];
        }

        Debug.LogWarning($"Invalid grid position: ({x}, {y})");
        return null;
    }
    
    public List<SingleGridController> GetEmptyGrids()
    {
        List<SingleGridController> emptyGrids = new List<SingleGridController>();
        foreach (var grid in _grids)
        {
            if (!grid.HasItem())
                emptyGrids.Add(grid);
        }
        return emptyGrids;
    }

}