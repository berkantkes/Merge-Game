using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private ItemGeneratorSO _itemGeneratorSo;
    
    private GridManager _gridManager;

    public void Initialize(GridManager gridManager)
    {
        _gridManager = gridManager;
    }
    
    public void CreateNewItem()
    {
        List<SingleGridController> emptyGrids = _gridManager.GetEmptyGrids();
        if (emptyGrids == null || emptyGrids.Count == 0)
        {
            Debug.LogWarning("No empty grids available.");
            return;
        }

        SingleGridController targetGrid = emptyGrids[Random.Range(0, emptyGrids.Count)];

        GeneratorData generatorData = _itemGeneratorSo.GeneratorData
            .OrderByDescending(d => d.GeneratorLevel)
            .FirstOrDefault();
        
        if (generatorData._GenerateItemsDatas == null || generatorData._GenerateItemsDatas.Count == 0)
        {
            Debug.LogWarning("No items to generate in SO.");
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

        ItemData itemData = FindObjectOfType<ItemDataHelper>().GetItemData(
            selectedData.GenerateItemLevel,
            selectedData.BoardItemFamilyType
        );

        var poolManager = FindObjectOfType<ObjectPoolManager>();
        ItemController item = poolManager.Get<ItemController>(targetGrid.transform);

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
    }
}
