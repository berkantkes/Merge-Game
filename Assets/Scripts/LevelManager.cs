using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDataSO _levelDataSo;
    [SerializeField] private ItemController _itemController;

    private ObjectPoolManager _objectPoolManager;
    private ItemDataHelper _itemDataHelper;
    private int _initialPoolSize = 10;
    
    public void Initialize(ObjectPoolManager objectPoolManager, ItemDataHelper itemItemDataHelper, GridManager gridManager)
    {
        _objectPoolManager = objectPoolManager;
        _itemDataHelper = itemItemDataHelper;
        
        _objectPoolManager.CreatePool(_itemController, _initialPoolSize);
        
        foreach (var startingItem in _levelDataSo.StartingItems)
        {
            ItemController item = _objectPoolManager.Get<ItemController>(transform);
            
            ItemData itemData = _itemDataHelper.GetItemData(startingItem.Level, startingItem.BoardItemFamilyType);
            
            item.Initialize(startingItem.GridX, startingItem.GridY, startingItem.Level, itemData.Icon, itemData.ItemType,
                itemData.BoardItemFamilyType, gridManager);
        }
    }
}
