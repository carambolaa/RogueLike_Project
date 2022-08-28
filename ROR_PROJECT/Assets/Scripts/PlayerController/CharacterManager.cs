using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }
    private Dictionary<string, int> m_Inventory = new Dictionary<string, int>();

    //Events
    public event Action OnRecieveDamage;
    public event Action<Transform, float> OnDealDamage;
    public event Action<string> OnAddItem;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddItem(string itemName)
    {
        OnAddItem?.Invoke(itemName);
    }

    public void SetInventory(Dictionary<string, int> inventory)
    {
        m_Inventory = inventory;
    }

    public float CalculateItemDamageBuff(Transform target, float currentDamage)
    {
        if(m_Inventory.ContainsKey("Crowbar") && target.GetComponent<Enemy>().GetHealPercentage() >= 0.9f)
        {
            currentDamage *= m_Inventory["Crowbar"] * 1.75f;
        }
        return currentDamage;
    }

    public void DealDamage(Transform target, float playerBaseDamage, float damageMultiplier)
    {
        //check buff item
        var currentDamage = playerBaseDamage * damageMultiplier;
        //calculate damage.deal damage
        target.GetComponent<Enemy>().RevieveDamage(CalculateItemDamageBuff(target, currentDamage));
        OnDealDamage?.Invoke(target, currentDamage);
    }

    public void RecieveDamage(int damage)
    {
        //this.HP -= damage;
        OnRecieveDamage?.Invoke();
    }
}
