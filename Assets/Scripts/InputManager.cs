using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask gridLayerMask;

    private ItemController _selectedItem;
    private SingleGridController _originGrid;
    private MergeStrategy _mergeStrategy;
    private ObjectPoolManager _objectPoolManager;
    private ItemDataHelper _itemDataHelper;
    private GridManager _gridManager;
    private ItemGenerator _itemGenerator;
    private Vector3 _initialPointerPosition;
    
    private const float dragThreshold = 10f;
    private bool _isDraggingStarted = false;
    
    public void Initialize(ObjectPoolManager objectPoolManager, ItemDataHelper itemDataHelper, GridManager gridManager,
        ItemGenerator itemGenerator)
    {
        _mergeStrategy = new MergeStrategy();
        _objectPoolManager = objectPoolManager;
        _itemDataHelper = itemDataHelper;
        _gridManager = gridManager;
        _itemGenerator = itemGenerator;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnInputStart();
        }

        if (Input.GetMouseButton(0) && _selectedItem != null)
        {
            float dragDistance = Vector3.Distance(Input.mousePosition, _initialPointerPosition);

            if (!_isDraggingStarted && dragDistance > dragThreshold)
            {
                _isDraggingStarted = true;
                SnapToPointer(); 
            }

            if (_isDraggingStarted)
            {
                DragSelectedItem();
            }
        }

        if (Input.GetMouseButtonUp(0) && _selectedItem != null)
        {
            OnInputRelease();
        }
    }

    void OnInputStart()
    {
        _initialPointerPosition = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, gridLayerMask))
        {
            var grid = hit.collider.GetComponent<SingleGridController>();
            if (grid != null && grid.HasItem())
            {
                _originGrid = grid;
                _selectedItem = grid.GetItem();
                grid.ClearItem();
            }
        }
    }

    void DragSelectedItem()
    {
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f; 
        _selectedItem.transform.position = worldPos;
    }
    
    void OnItemTapped(ItemController item)
    {
        if (item.GetItemType() == ItemType.Generator)
        {
            _itemGenerator.CreateNewItem();
            _originGrid.PlaceItem(item);
        }
    }
    void HandleDragRelease()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, gridLayerMask))
        {
            var targetGrid = hit.collider.GetComponent<SingleGridController>();
            if (targetGrid.GetItem() != null)
            {
                var targetItem = targetGrid.GetItem();
                
                if (_mergeStrategy.CheckMerge(_selectedItem, targetGrid))
                {
                    _objectPoolManager.Return(targetGrid.GetItem());
                    _objectPoolManager.Return(_selectedItem);
                    _originGrid.ClearItem();
                    targetGrid.ClearItem();
                    ItemController item = _objectPoolManager.Get<ItemController>(targetGrid.transform);
                    ItemData itemData = _itemDataHelper.GetItemData(_selectedItem.GetLevel() + 1, _selectedItem.GetBoardItemFamilyType());
                    item.Initialize(targetGrid.GetGridX(), targetGrid.GetGridY(), _selectedItem.GetLevel() + 1,
                        itemData.Icon, itemData.ItemType,
                        itemData.BoardItemFamilyType, _gridManager);
                    
                    return;
                }
                else
                {
                    _originGrid.PlaceItem(targetItem);
                    targetItem.transform.position = _originGrid.transform.position;

                    targetGrid.PlaceItem(_selectedItem);
                    _selectedItem.transform.position = targetGrid.transform.position;

                    ResetState();
                    return;
                }
            }
        }

        _selectedItem.transform.position = _originGrid.transform.position;
        _originGrid.PlaceItem(_selectedItem);
        ResetState();
    }

    void OnInputRelease()
    {
        float dragDistance = Vector3.Distance(Input.mousePosition, _initialPointerPosition);

        if (dragDistance < dragThreshold)
        {
            OnItemTapped(_selectedItem);
        }
        else
        {
            HandleDragRelease();
        }

        
    }
    void SnapToPointer()
    {
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        _selectedItem.transform.position = worldPos;
    }


    void ResetState()
    {
        _selectedItem = null;
        _originGrid = null;
        _isDraggingStarted = false;
    }

}
