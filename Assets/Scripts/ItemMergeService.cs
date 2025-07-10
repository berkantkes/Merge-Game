using DG.Tweening;
using UnityEngine;

public class ItemMergeService : IItemMergeService
{
    private IGridRaycaster _raycaster;
    private MergeStrategy _mergeStrategy;
    private ObjectPoolManager _objectPool;
    private ItemDataHelper _dataHelper;
    private GridManager _gridManager;
    private ItemGenerator _itemGenerator;
    private UIManager _uiManager;

    public ItemMergeService(IGridRaycaster raycaster, MergeStrategy mergeStrategy, ObjectPoolManager pool, 
        ItemDataHelper dataHelper, GridManager gridManager, ItemGenerator generator, UIManager uiManager)
    {
        _raycaster = raycaster;
        _mergeStrategy = mergeStrategy;
        _objectPool = pool;
        _dataHelper = dataHelper;
        _gridManager = gridManager;
        _itemGenerator = generator;
        _uiManager = uiManager;
    }

    public void TryMergeOrPlace(ItemController selected, SingleGridController origin, Vector3 pointerScreen)
    {
        SingleGridController target = _raycaster.RaycastToGrid(pointerScreen);
        if (target == null)
        {
            CancelMove(selected, origin);
            return;
        }

        if (target.HasItem())
        {
            if (_mergeStrategy.CheckMerge(selected, target))
            {
                int nextLevel = selected.GetLevel() + 1;
                BoardItemFamilyType family = selected.GetBoardItemFamilyType();

                if (!_dataHelper.HasItemData(nextLevel, family))
                {
                    ChangeItems(selected, origin, target);
                    _uiManager.PlayCantMergeTextAnimation();
                    return;
                }

                _objectPool.Return(target.GetItem());
                _objectPool.Return(selected);
                origin.ClearItem();
                target.ClearItem();

                ItemData data = _dataHelper.GetItemData(nextLevel, family);

                ItemController newItem = _itemGenerator.CreateNewItem(
                    target.GetGridX(), target.GetGridY(),
                    data.Level, data, target.transform);

                newItem.PlayCreateItemAnimation();
                target.PlaceItem(newItem);
            }

            else
            {
                ChangeItems(selected, origin, target);
            }
        }
        else
        {
            origin.PlaceItem(selected);
            selected.GoOriginGrid(origin);
        }
    }

    private void ChangeItems(ItemController selected, SingleGridController origin, SingleGridController target)
    {
        origin.PlaceItem(target.GetItem());
        target.GetItem().GoOriginGrid(origin);
        target.PlaceItem(selected);
        selected.transform.position = target.transform.position;
        selected.PlayBumpAnimation();
    }

    private void CancelMove(ItemController selected, SingleGridController origin)
    {
        selected.GoOriginGrid(origin);
        origin.PlaceItem(selected);
    }
}
