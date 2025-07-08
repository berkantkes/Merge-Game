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
    private ItemGenerator _itemGenerator;

    private string SavePath => Application.persistentDataPath + "/saved_level.json";

    public void Initialize(ObjectPoolManager objectPoolManager, ItemDataHelper itemItemDataHelper, GridManager gridManager,
        ItemGenerator itemGenerator)
    {
        _itemGenerator = itemGenerator;
        _objectPoolManager = objectPoolManager;
        _itemDataHelper = itemItemDataHelper;

        _objectPoolManager.CreatePool(_itemController, _initialPoolSize);

        List<ItemPlacementData> itemsToLoad;

        itemsToLoad = _levelDataSo.StartingItems;
        // if (File.Exists(SavePath))
        // {
        //     string json = File.ReadAllText(SavePath);
        //     ItemPlacementDataList dataList = JsonUtility.FromJson<ItemPlacementDataList>(json);
        //     itemsToLoad = dataList.Items;
        // }
        // else
        // {
        //     itemsToLoad = _levelDataSo.StartingItems;
        // }

        foreach (var itemData in itemsToLoad)
        {
            ItemData itemInfo = _itemDataHelper.GetItemData(itemData.Level, itemData.BoardItemFamilyType);
            
            Debug.Log("Init3");
            Debug.Log(_itemGenerator);
            Debug.Log("Init4");
            Debug.Log(itemData);
            Debug.Log(itemInfo);
            Debug.Log(transform);
            _itemGenerator.CreateNewItem(
                itemData.GridX, 
                itemData.GridY, 
                itemData.Level,
                itemInfo,
                transform
                );
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
