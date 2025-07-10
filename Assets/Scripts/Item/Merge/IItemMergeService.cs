using UnityEngine;

public interface IItemMergeService
{
    void TryMergeOrPlace(ItemController selectedItem, SingleGridController originGrid, Vector3 pointerPosition);
}