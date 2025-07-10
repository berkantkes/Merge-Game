using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private GridManager _gridManager;
    private SingleGridController _currentGrid;
    
    private BoardItemFamilyType _boardItemFamilyType;
    private ItemType _itemType;
    private int _level;
    private int _gridX;
    private int _gridY;
    private int _defaultLayerValue = 2;
    private int _selectedLayerValue = 4;
    private ItemState _currentState = ItemState.Idle;

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
        SetDefaultLayer();
    }

    public void GoOriginGrid(SingleGridController gridController)
    {
        float speed = 70f;
        float distance = Vector3.Distance(transform.position, gridController.transform.position); 
        float duration = Mathf.Min(distance / speed, 0.2f);

        _currentState = ItemState.Moving; 

        transform.DOMove(gridController.transform.position, duration)
            .OnComplete(() =>
            {
                SetDefaultLayer();
                _currentState = ItemState.Idle;
                PlayBumpAnimation();
            });
    }

    public void PlayCreateItemAnimation()
    {
        PlayBumpAnimation();
    }

    public void PlayGenerateItemAnimation(SingleGridController generatorGrid)
    {
        SetSelectedLayer();
        
        transform.position = generatorGrid.transform.position;
        
        float speed = 20f;
        float distance = Vector3.Distance(transform.localPosition, Vector3.zero);
        float duration = Mathf.Clamp(distance / speed, 0.35f, 0.5f);
        
        _currentState = ItemState.Moving;

        MoveToPositionWithScale(Vector3.zero, duration, 1.5f, () =>
        {
            SetDefaultLayer();
            _currentState = ItemState.Idle;
            PlayBumpAnimation();
        });
    }  
    
    private void MoveToPositionWithScale(Vector3 targetLocalPos, float duration, float peakScale, TweenCallback onComplete)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOLocalMove(targetLocalPos, duration).SetEase(Ease.OutSine));

        sequence.Join(
            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one * peakScale, duration * 0.5f).SetEase(Ease.OutQuad))
                .Append(transform.DOScale(Vector3.one, duration * 0.5f).SetEase(Ease.InQuad))
        );

        sequence.OnComplete(onComplete);
    }
    
    public void PlayBumpAnimation(float bumpScale = 0.94f, float duration = 0.3f)
    {
        transform.localScale = Vector3.one;
        transform.DOKill(); 

        Sequence bump = DOTween.Sequence();

        bump.Append(transform.DOScale(Vector3.one * bumpScale, duration * 0.33f).SetEase(Ease.OutQuad));
        bump.Append(transform.DOScale(Vector3.one * 1.04f, duration * 0.33f).SetEase(Ease.InOutQuad));
        bump.Append(transform.DOScale(Vector3.one, duration * 0.34f).SetEase(Ease.InQuad));
    }

    public void SetSelected()
    {
        SetSelectedLayer();
    }

    private void SetSelectedLayer()
    {
        _spriteRenderer.sortingOrder = _selectedLayerValue;
    }

    private void SetDefaultLayer()
    {
        _spriteRenderer.sortingOrder = _defaultLayerValue;
    }

    public bool IsSelectable()
    {
        return _currentState == ItemState.Idle;
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
    
    private enum ItemState
    {
        Idle,
        Moving
    }
}
