using UnityEngine;

[System.Serializable]
public class ItemPlacementData
{
    public int GridX;
    public int GridY;
    public BoardItemFamilyType BoardItemFamilyType;
    public int Level;

    public ItemPlacementData(int gridX, int gridY, BoardItemFamilyType boardItemFamilyType, int level)
    {
        GridX = gridX;
        GridY = gridY;
        BoardItemFamilyType = boardItemFamilyType;
        Level = level;
    }
}