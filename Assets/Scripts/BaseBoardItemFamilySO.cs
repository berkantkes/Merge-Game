using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBoardItemFamilySO : ScriptableObject
{
    public List<ItemData> FamilyData;
    public BoardItemFamilyType FamilyType;

    public ItemData GetItemDataByLevel(int level)
    {
        int index = level - 1;
        if (index >= 0 && index < FamilyData.Count)
        {
            return FamilyData[index];
        }

        return null;
    }
}