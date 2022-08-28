using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IItem : MonoBehaviour
{
    [SerializeField] protected int itemNumber;

    protected virtual void Start()
    {
        CharacterManager.Instance.OnAddItem += AddOne;
    }

    protected void AddOne(string itemName)
    {
        Debug.Log(this.GetType().ToString());
        if(itemName == this.GetType().ToString())
        {
            itemNumber++;
        }
    }

}
