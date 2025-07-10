using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask gridLayerMask;

    private IInputHandler _inputHandler;

    public void Initialize(ObjectPoolManager objectPoolManager, ItemDataHelper itemDataHelper, GridManager gridManager,
        ItemGenerator itemGenerator, UIManager uiManager)
    {
        MergeStrategy mergeStrategy = new MergeStrategy();
        IGridRaycaster gridRaycaster = new GridRaycaster(mainCamera, gridLayerMask);
        IItemSelector itemSelector = new ItemSelector(gridRaycaster, itemGenerator);
        IItemMergeService itemMergeService = new ItemMergeService(gridRaycaster, mergeStrategy, objectPoolManager, 
            itemDataHelper, gridManager, itemGenerator, uiManager);

        _inputHandler = new ItemDragHandler(mainCamera, itemMergeService, itemSelector);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _inputHandler.OnInputStart(Input.mousePosition);

        if (Input.GetMouseButton(0))
            _inputHandler.OnInputDrag(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
            _inputHandler.OnInputRelease(Input.mousePosition);
    }
}