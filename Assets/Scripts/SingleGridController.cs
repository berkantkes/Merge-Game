using UnityEngine;

public class SingleGridController : MonoBehaviour
{
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;

    public int GetGridX()
    {
        return gridX;
    }
    public int GetGridY()
    {
        return gridY;
    }
}
