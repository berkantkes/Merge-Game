using UnityEngine;

public class SingleGridController : MonoBehaviour
{
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;
    
    private ItemController _currentItem;

    public bool HasItem() => _currentItem != null;
    public ItemController GetItem() => _currentItem;

    public void PlaceItem(ItemController item)
    {
        _currentItem = item;
    }

    public void ClearItem()
    {
        _currentItem = null;
    }

    public int GetGridX()
    {
        return gridX;
    }
    public int GetGridY()
    {
        return gridY;
    }
}