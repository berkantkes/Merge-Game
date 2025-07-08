using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private bool _isSelectable = true;
    private int _defaultLayerValue = 2;
    private int _selectedLayerValue = 4;

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
        _currentGrid.PlaceItem(this);
        
        transform.SetParent(_currentGrid.transform);
        transform.localPosition = Vector3.zero;
        _spriteRenderer.sortingOrder = _defaultLayerValue;
    }

    public void BackOriginGrid(SingleGridController gridController)
    {
        transform.DOMove(gridController.transform.position, .2f)
            .OnComplete((() =>
            {
                _spriteRenderer.sortingOrder = _defaultLayerValue;
                _isSelectable = true;
            }));
    }

    public void PlayCreateItemAnimation(SingleGridController generatorGrid)
    {
        transform.position = generatorGrid.transform.position;
        transform.DOLocalMove(Vector3.zero, .2f);
    }

    public void SetSelected()
    {
        _spriteRenderer.sortingOrder = _selectedLayerValue;
    }

    public SingleGridController GetCurrentGrid()
    {
        return _currentGrid;
    }
    public int GetLevel()
    {
        return _level;
    }

    public BoardItemFamilyType GetBoardItemFamilyType()
    {
        return _boardItemFamilyType;
    }

    public ItemType GetItemType()
    {
        return _itemType;
    }

}
