using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDataSO _levelDataSo;
    [SerializeField] private ItemController _itemController;

    private ObjectPoolManager _objectPoolManager;
    private ItemDataHelper _itemDataHelper;
    private int _initialPoolSize = 10;

    private string SavePath => Application.persistentDataPath + "/saved_level.json";

    public void Initialize(ObjectPoolManager objectPoolManager, ItemDataHelper itemItemDataHelper, GridManager gridManager)
    {
        _objectPoolManager = objectPoolManager;
        _itemDataHelper = itemItemDataHelper;

        _objectPoolManager.CreatePool(_itemController, _initialPoolSize);

        List<ItemPlacementData> itemsToLoad;

        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            ItemPlacementDataList dataList = JsonUtility.FromJson<ItemPlacementDataList>(json);
            itemsToLoad = dataList.Items;
            Debug.Log("Loaded level from saved JSON.");
        }
        else
        {
            itemsToLoad = _levelDataSo.StartingItems;
            Debug.Log("Loaded level from LevelDataSO.");
        }

        foreach (var itemData in itemsToLoad)
        {
            ItemController item = _objectPoolManager.Get<ItemController>(transform);

            ItemData itemInfo = _itemDataHelper.GetItemData(itemData.Level, itemData.BoardItemFamilyType);

            item.Initialize(itemData.GridX, itemData.GridY, itemData.Level, itemInfo.Icon, itemInfo.ItemType,
                itemInfo.BoardItemFamilyType, gridManager);
        }
    }

    public void SaveCurrentLevel(List<ItemPlacementData> data)
    {
        ItemPlacementDataList wrapper = new ItemPlacementDataList { Items = data };
        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(SavePath, json);
    }
}

[System.Serializable]
public class ItemPlacementDataList
{
    public List<ItemPlacementData> Items;
}
