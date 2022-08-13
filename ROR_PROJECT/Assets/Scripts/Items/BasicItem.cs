using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicItem : MonoBehaviour
{
    private void Start()
    {
        PlayerMovement.instance.OnRecieveDamage += Cast;
    }

    private void Cast()
    {
        Debug.Log("Player got hit, heal!");
    }
}