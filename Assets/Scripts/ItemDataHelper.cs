using System.Collections.Generic;
using UnityEngine;

public class ItemDataHelper : MonoBehaviour
{
    [SerializeField] private List<BaseBoardItemFamilySO> allFamilies;

    private Dictionary<(int, BoardItemFamilyType), ItemData> itemDataLookup 
        = new Dictionary<(int, BoardItemFamilyType), ItemData>();

    private void Awake()
    {
        BuildItemDataLookup();
    }

    private void BuildItemDataLookup()
    {
        foreach (var familySO in allFamilies)
        {
            for (int level = 1; level <= familySO.FamilyData.Count; level++)
            {
                var itemData = familySO.GetItemDataByLevel(level);
                if (itemData == null) continue;

                var key = (level, familySO.FamilyType);

                if (!itemDataLookup.ContainsKey(key))
                    itemDataLookup.Add(key, itemData);
            }
        }
    }

    public ItemData GetItemData(int level, BoardItemFamilyType familyType)
    {
        if (itemDataLookup.TryGetValue((level, familyType), out var itemData))
        {
            return itemData;
        }

        return null;
    }
}