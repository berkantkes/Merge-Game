using DG.Tweening;
using UnityEngine;

public class ItemMergeService : IItemMergeService
{
    private readonly IGridRaycaster _raycaster;
    private readonly MergeStrategy _mergeStrategy;
    private readonly ObjectPoolManager _objectPool;
    private readonly ItemDataHelper _dataHelper;
    private readonly GridManager _gridManager;
    private readonly ItemGenerator _itemGenerator;

    public ItemMergeService(IGridRaycaster raycaster, MergeStrategy mergeStrategy,
        ObjectPoolManager pool, ItemDataHelper dataHelper,
        GridManager gridManager, ItemGenerator generator)
    {
        _raycaster = raycaster;
        _mergeStrategy = mergeStrategy;
        _objectPool = pool;
        _dataHelper = dataHelper;
        _gridManager = gridManager;
        _itemGenerator = generator;
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
                _objectPool.Return(target.GetItem());
                _objectPool.Return(selected);
                origin.ClearItem();
                target.ClearItem();

                ItemData data = _dataHelper.GetItemData(
                    selected.GetLevel() + 1,
                    selected.GetBoardItemFamilyType());

                ItemController newItem = _itemGenerator.CreateNewItem(
                    target.GetGridX(), target.GetGridY(),
                    data.Level, data, target.transform);

                newItem.PlayCreateItemAnimation();
                target.PlaceItem(newItem);
            }
            else
            {
                origin.PlaceItem(target.GetItem());
                target.GetItem().GoOriginGrid(origin);
                target.PlaceItem(selected);
                selected.transform.position = target.transform.position;
            }
        }
        else
        {
            origin.PlaceItem(selected);
            selected.GoOriginGrid(origin);
        }
    }

    private void CancelMove(ItemController selected, SingleGridController origin)
    {
        selected.GoOriginGrid(origin);
        origin.PlaceItem(selected);
    }
}
