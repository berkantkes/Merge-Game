using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Setup")]
    [SerializeField] private GameObject _gridPrefab;
    [SerializeField] private int _gridWidth = 5;
    [SerializeField] private int _gridHeight = 5;
    [SerializeField] private Vector2 _startPosition = new Vector2(-5.6f, -5.6f);
    [SerializeField] private float _offset = 2.8f;

    private List<SingleGridController> _grids = new List<SingleGridController>();
    private SingleGridController[,] gridArray;

    public void Initialize()
    {
        gridArray = new SingleGridController[_gridWidth, _gridHeight];

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                Vector3 spawnPosition = new Vector3(
                    _startPosition.x + (x * _offset),
                    _startPosition.y + (y * _offset),
                    0);

                GameObject gridGO = Instantiate(_gridPrefab, spawnPosition, Quaternion.identity, this.transform);
                gridGO.name = $"Grid_{x}x{y}";

                SingleGridController grid = gridGO.GetComponent<SingleGridController>();
                grid.SetGridPosition(x, y); 

                gridArray[x, y] = grid;
                _grids.Add(grid);
            }
        }
    }

    public SingleGridController GetGridAt(int x, int y)
    {
        if (gridArray == null)
            return null;

        if (x >= 0 && x < gridArray.GetLength(0) && y >= 0 && y < gridArray.GetLength(1))
            return gridArray[x, y];

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

    public List<ItemPlacementData> GetGridData()
    {
        List<ItemPlacementData> itemPlacementDataList = new List<ItemPlacementData>();

        foreach (var grid in _grids)
        {
            if (grid.HasItem())
            {
                ItemPlacementData itemPlacementData = new ItemPlacementData(
                    grid.GetGridX(), grid.GetGridY(),
                    grid.GetItem().GetBoardItemFamilyType(),
                    grid.GetItem().GetLevel());

                itemPlacementDataList.Add(itemPlacementData);
            }
        }

        return itemPlacementDataList;
    }
}
