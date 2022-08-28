using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            Debug.Log("-10");
            other.GetComponent<CharacterManager>()?.RecieveDamage(10);
        }
    }
}