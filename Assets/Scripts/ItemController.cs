using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private int _level;
    private BoardItemFamilyType _boardItemFamilyType;
    private ItemType _itemType;

    public void Initialize(int level, Sprite icon, ItemType itemType, BoardItemFamilyType boardItemFamilyType)
    {
        _level = level;
        _spriteRenderer.sprite = icon;
        _itemType = itemType;
        _boardItemFamilyType = boardItemFamilyType;
    }

}
