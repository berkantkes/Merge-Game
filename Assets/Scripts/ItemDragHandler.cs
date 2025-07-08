using UnityEngine;

public class ItemDragHandler : IInputHandler
{
    private readonly Camera _camera;
    private readonly IItemMergeService _mergeService;
    private readonly IItemSelector _itemSelector;

    private readonly float _dragThreshold = 10f;
    private Vector3 _initialPointer;
    private bool _isDragging;
    private ItemController _selectedItem;
    private SingleGridController _originGrid;

    public ItemDragHandler(Camera camera, IItemMergeService mergeService, IItemSelector itemSelector)
    {
        _camera = camera;
        _mergeService = mergeService;
        _itemSelector = itemSelector;
    }

    public void OnInputStart(Vector3 screenPos)
    {
        _initialPointer = screenPos;
        (_selectedItem, _originGrid) = _itemSelector.TrySelectItem(screenPos);

        if (_selectedItem != null)
        {
            _selectedItem.SetSelected();
            _originGrid.ClearItem();
        }
    }

    public void OnInputDrag(Vector3 screenPos)
    {
        if (_selectedItem == null) return;

        if (!_isDragging && Vector3.Distance(screenPos, _initialPointer) > _dragThreshold)
            _isDragging = true;

        if (_isDragging)
        {
            Vector3 worldPos = _camera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;
            _selectedItem.transform.position = Vector3.Lerp(_selectedItem.transform.position, worldPos, Time.deltaTime * 20f);
        }
    }

    public void OnInputRelease(Vector3 screenPos)
    {
        if (_selectedItem == null) return;

        if (Vector3.Distance(screenPos, _initialPointer) < _dragThreshold)
        {
            _itemSelector.OnItemTapped(_selectedItem, _originGrid);
        }
        else
        {
            _mergeService.TryMergeOrPlace(_selectedItem, _originGrid, screenPos);
        }

        Reset();
    }

    private void Reset()
    {
        _selectedItem = null;
        _originGrid = null;
        _isDragging = false;
    }
}