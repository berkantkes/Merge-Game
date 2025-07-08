using UnityEngine;

public interface IItemSelector
{
    (ItemController, SingleGridController) TrySelectItem(Vector3 pointerScreenPosition);
    void OnItemTapped(ItemController item, SingleGridController originGrid);
}