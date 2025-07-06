using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ObjectPoolManager _objectPoolManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private ItemDataHelper _itemDataHelper;

    private void Awake()
    {
        _gridManager.Initialize();
        _levelManager.Initialize(_objectPoolManager, _itemDataHelper, _gridManager);
    }
}
