using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelData")]
public class LevelDataSO : ScriptableObject
{
    public List<ItemPlacementData> StartingItems;
}