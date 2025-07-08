using UnityEngine;

public class GridRaycaster : IGridRaycaster
{
    private readonly Camera _camera;
    private readonly LayerMask _layerMask;

    public GridRaycaster(Camera camera, LayerMask layerMask)
    {
        _camera = camera;
        _layerMask = layerMask;
    }

    public SingleGridController RaycastToGrid(Vector3 screenPos)
    {
        Ray ray = _camera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _layerMask))
        {
            return hit.collider.GetComponent<SingleGridController>();
        }

        return null;
    }
}