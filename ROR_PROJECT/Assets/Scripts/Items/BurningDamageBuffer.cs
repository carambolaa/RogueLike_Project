using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningDamageBuffer : Item
{
    public float GetDamage()
    {
        return itemNumber * 3;
    }

    private void OnDestroy()
    {
        Debug.Log("minus damage");
    }
}
