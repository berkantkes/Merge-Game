using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemGeneratorSO", menuName = "Items/ItemGeneratorSO")]
public class ItemGeneratorSO : ScriptableObject
{
    public List<GeneratorData> GeneratorData;
}

[System.Serializable]
public class GeneratorData
{
    public int GeneratorLevel;
    public List<GenerateItemsData> _GenerateItemsDatas;
}

[System.Serializable]
public struct GenerateItemsData
{
    public BoardItemFamilyType BoardItemFamilyType;
    public int GenerateItemLevel;
    public float Possibility;
}