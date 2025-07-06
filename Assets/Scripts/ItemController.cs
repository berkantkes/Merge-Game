using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private GridManager _gridManager;
    private SingleGridController _currentGrid;
    
    private int _level;
    private BoardItemFamilyType _boardItemFamilyType;
    private ItemType _itemType;
    private int _gridX;
    private int _gridY;

    public void Initialize(int x, int y, int level, Sprite icon, ItemType itemType, BoardItemFamilyType boardItemFamilyType,
        GridManager gridManager)
    {
        _gridManager = gridManager;
        _level = level;
        _spriteRenderer.sprite = icon;
        _itemType = itemType;
        _boardItemFamilyType = boardItemFamilyType;
        _gridX = x;
        _gridY = y;

        _currentGrid = _gridManager.GetGridAt(_gridX, _gridY);
        
        transform.SetParent(_currentGrid.transform);
        transform.localPosition = Vector3.zero;
    }

}
