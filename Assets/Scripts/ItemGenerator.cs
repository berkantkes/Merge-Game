using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private ItemGeneratorSO _itemGeneratorSo;
    
    private GridManager _gridManager;
    private ObjectPoolManager _objectPoolManager;
    private ItemDataHelper _itemDataHelper;

    public void Initialize(GridManager gridManager, ObjectPoolManager objectPoolManager, ItemDataHelper itemDataHelper)
    {
        _gridManager = gridManager;
        _objectPoolManager = objectPoolManager;
        _itemDataHelper = itemDataHelper;
    }
    
    public void CreateNewItem(SingleGridController generatorGrid)
    {
        List<SingleGridController> emptyGrids = _gridManager.GetEmptyGrids();
        if (emptyGrids == null || emptyGrids.Count == 0)
        {
            return;
        }

        SingleGridController targetGrid = emptyGrids[Random.Range(0, emptyGrids.Count)];

        GeneratorData generatorData = _itemGeneratorSo.GeneratorData
            .OrderByDescending(d => d.GeneratorLevel)
            .FirstOrDefault();
        
        if (generatorData._GenerateItemsDatas == null || generatorData._GenerateItemsDatas.Count == 0)
        {
            return;
        }

        float totalWeight = generatorData._GenerateItemsDatas.Sum(i => i.Possibility);
        float randomValue = Random.Range(0f, totalWeight);

        float current = 0f;
        GenerateItemsData selectedData = new GenerateItemsData();

        foreach (var data in generatorData._GenerateItemsDatas)
        {
            current += data.Possibility;
            if (randomValue <= current)
            {
                selectedData = data;
                break;
            }
        }

        ItemData itemData = _itemDataHelper.GetItemData(selectedData.GenerateItemLevel,
            selectedData.BoardItemFamilyType
        );

        ItemController item = _objectPoolManager.Get<ItemController>(targetGrid.transform);

        item.Initialize(
            targetGrid.GetGridX(),
            targetGrid.GetGridY(),
            selectedData.GenerateItemLevel,
            itemData.Icon,
            itemData.ItemType,
            itemData.BoardItemFamilyType,
            _gridManager
        );

        targetGrid.PlaceItem(item);

        item.PlayCreateItemAnimation(generatorGrid);
    }
}
