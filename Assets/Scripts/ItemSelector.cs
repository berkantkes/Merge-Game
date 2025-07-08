using UnityEngine;

public class ItemSelector : IItemSelector
{
    private readonly IGridRaycaster _raycaster;
    private ItemGenerator _itemGenerator;

    public ItemSelector(IGridRaycaster raycaster, 
        ItemGenerator itemGenerator)
    {
        _raycaster = raycaster;
        _itemGenerator = itemGenerator;
    }

    public (ItemController, SingleGridController) TrySelectItem(Vector3 screenPos)
    {
        SingleGridController grid = _raycaster.RaycastToGrid(screenPos);
        if (grid != null && grid.HasItem())
            return (grid.GetItem(), grid);

        return (null, null);
    }

    public void OnItemTapped(ItemController item, SingleGridController originGrid)
    {
        if (item.GetItemType() != ItemType.Generator) return;

        originGrid.PlaceItem(item);
        item.GoOriginGrid(originGrid);
        _itemGenerator.CreateNewItemGenerator(originGrid);
    }
}