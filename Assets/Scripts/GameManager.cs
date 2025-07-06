using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ObjectPoolManager _objectPoolManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private ItemDataHelper _itemDataHelper;
    [SerializeField] private InputManager _inputManager;

    private void Awake()
    {
        _gridManager.Initialize();
        _levelManager.Initialize(_objectPoolManager, _itemDataHelper, _gridManager);
        _inputManager.Initialize(_objectPoolManager, _itemDataHelper, _gridManager);
    }
}
