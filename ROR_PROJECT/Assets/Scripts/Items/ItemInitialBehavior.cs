using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInitialBehavior : MonoBehaviour
{
    void Start()
    {
        string name = this.gameObject.name;
        this.gameObject.name = name.Remove(name.Length - 7);
        GetComponent<Rigidbody>().AddForce(Vector3.up * 13, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(new Vector3(1, 0, 0) * 100, ForceMode.Impulse);
    }
}
