using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected int itemNumber;

    protected virtual void Start()
    {
        itemNumber = 1;
        ItemNumberUpdate();
        CharacterManager.Instance.OnPlusItem += AddOne;
        CharacterManager.Instance.OnMinusItem += MinusOne;
    }

    protected void AddOne(string itemName)
    {
        if(itemName == this.GetType().ToString())
        {
            itemNumber++;
            ItemNumberUpdate();
        }
    }

    protected void MinusOne(string itemName)
    {
        if(itemName == this.GetType().ToString())
        {
            if(itemNumber > 1)
            {
                itemNumber--;
                ItemNumberUpdate();
                return;
            }
            if(itemNumber <= 1)
            {
                Destroy(this);
            }
        }
    }

    protected virtual void ItemNumberUpdate()
    {

    }
}
