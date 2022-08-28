using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    private Dictionary<string, int> myInventory = new Dictionary<string, int>();
    private GameObject itemHolder;

    private void Start()
    {
        itemHolder = GameObject.Find("ItemHolder");
    }

    public void AddItem(string itemName)
    {
        CharacterManager.Instance.SetInventory(myInventory);
        if(myInventory.ContainsKey(itemName))
        {
            CharacterManager.Instance.AddItem(itemName);
            myInventory[itemName]++;
        }
        else
        {
            myInventory.Add(itemName, 1);
            itemHolder.AddComponent(Type.GetType(itemName));
        }
    }
}
