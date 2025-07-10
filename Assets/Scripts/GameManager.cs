using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private ObjectPoolManager _objectPoolManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private ItemDataHelper _itemDataHelper;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private ItemGenerator _itemGenerator;
    [SerializeField] private UIManager _uiManager;

    private void Awake()
    {
        _gridManager.Initialize();
        _itemGenerator.Initialize(_gridManager, _objectPoolManager, _itemDataHelper, _uiManager);
        _levelManager.Initialize(_objectPoolManager, _itemDataHelper, _gridManager, _itemGenerator);
        _inputManager.Initialize(_objectPoolManager, _itemDataHelper, _gridManager, _itemGenerator, _uiManager);
    }

    private void OnApplicationQuit()
    {
        List<ItemPlacementData> data = _gridManager.GetGridData();
        _levelManager.SaveCurrentLevel(data);
    }

}