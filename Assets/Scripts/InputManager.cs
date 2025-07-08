using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask gridLayerMask;

    private IInputHandler _inputHandler;

    public void Initialize(
        ObjectPoolManager objectPoolManager,
        ItemDataHelper itemDataHelper,
        GridManager gridManager,
        ItemGenerator itemGenerator)
    {
        var mergeStrategy = new MergeStrategy();
        var gridRaycaster = new GridRaycaster(mainCamera, gridLayerMask);
        var itemSelector = new ItemSelector(gridRaycaster, itemGenerator);
        var itemMergeService = new ItemMergeService(
            gridRaycaster, mergeStrategy, objectPoolManager, itemDataHelper, gridManager, itemGenerator);

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