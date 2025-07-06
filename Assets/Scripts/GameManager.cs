using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;

    private void Awake()
    {
        _gridManager.Initialize();
    }
}
