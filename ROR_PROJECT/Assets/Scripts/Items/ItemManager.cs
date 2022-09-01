using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    private GameObject itemHolder;

    private void Start()
    {
        itemHolder = GameObject.Find("ItemHolder");
        CharacterManager.Instance.OnAddItem += AddItem;
    }

    public void AddItem(string itemName)
    {
        itemHolder.AddComponent(Type.GetType(itemName));
    }
}
