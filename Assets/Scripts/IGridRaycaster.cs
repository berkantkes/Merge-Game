using UnityEngine;

public interface IGridRaycaster
{
    SingleGridController RaycastToGrid(Vector3 screenPosition);
}