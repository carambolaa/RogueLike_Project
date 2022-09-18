using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    private GameObject itemHolder;
    [SerializeField]
    private Transform inventory_UI;
    [SerializeField]
    private GameObject itemPrefab;
    private Dictionary<string, int> m_Inventory = new Dictionary<string, int>();

    private void Start()
    {
        inventory_UI = GameObject.Find("InventoryBackground").transform;
        itemHolder = GameObject.Find("ItemHolder");

        CharacterManager.Instance.OnAddItem += AddItem;
        CharacterManager.Instance.OnAddItem += RefreshItemUI;
        CharacterManager.Instance.OnPlusItem += RefreshItemUI;
        CharacterManager.Instance.OnMinusItem += RefreshItemUI;
    }

    public void AddItem(string itemName)
    {
        itemHolder.AddComponent(Type.GetType(itemName));
    }

    private void RefreshItemUI(string itemName)
    {
        foreach(Transform child in inventory_UI.transform)
        {
            Destroy(child.gameObject);
        }

        m_Inventory = CharacterManager.Instance.GetInventoryDictionary();

        foreach (KeyValuePair<string, int> itemInfo in m_Inventory)
        {
            Debug.Log(itemPrefab);
            if(itemPrefab != null)
            {
                Instantiate(itemPrefab, inventory_UI);
            }
        }
    }
}
