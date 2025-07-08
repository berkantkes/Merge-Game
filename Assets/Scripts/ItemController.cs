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

    public void Initialize(int x, int y, int level, ItemData itemData, GridManager gridManager)
    {
        _gridManager = gridManager;
        _level = level;
        _spriteRenderer.sprite = itemData.Icon;
        _itemType = itemData.ItemType;
        _boardItemFamilyType = itemData.BoardItemFamilyType;
        _gridX = x;
        _gridY = y;

        _currentGrid = _gridManager.GetGridAt(_gridX, _gridY);
        _currentGrid.PlaceItem(this);
        
        transform.SetParent(_currentGrid.transform);
        transform.localPosition = Vector3.zero;
        _spriteRenderer.sortingOrder = _defaultLayerValue;
    }

    public void GoOriginGrid(SingleGridController gridController)
    {
        float distance = Vector3.Distance(transform.position, gridController.transform.position);
        float speed = 70f;
        float duration = distance / speed;

        transform.DOMove(gridController.transform.position, duration)
            .OnComplete(() =>
            {
                _spriteRenderer.sortingOrder = _defaultLayerValue;
                _isSelectable = true;
                PlayBumpAnimation();
            });
    }

    public void PlayCreateItemAnimation()
    {
        PlayBumpAnimation();
    }

    public void PlayGenerateItemAnimation(SingleGridController generatorGrid)
    {
        transform.position = generatorGrid.transform.position;

        float distance = Vector3.Distance(transform.localPosition, Vector3.zero);
        float speed = 60f;
        float duration = distance / speed;

        transform.DOLocalMove(Vector3.zero, duration)
            .OnComplete((() => PlayBumpAnimation()));
    }

    private void PlayBumpAnimation(float bumpScale = 1.1f, float duration = 0.3f)
    {
        transform.localScale = Vector3.one;
        transform.DOKill(); 

        Sequence bump = DOTween.Sequence();

        bump.Append(transform.DOScale(Vector3.one * bumpScale, duration * 0.25f).SetEase(Ease.OutQuad));
        bump.Append(transform.DOScale(Vector3.one * 0.98f, duration * 0.25f).SetEase(Ease.InOutQuad));
        bump.Append(transform.DOScale(Vector3.one * 1.02f, duration * 0.2f).SetEase(Ease.OutQuad));
        bump.Append(transform.DOScale(Vector3.one, duration * 0.3f).SetEase(Ease.InQuad));
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
