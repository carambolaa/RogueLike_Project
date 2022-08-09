using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform cameraPosition;

    private void Awake()
    {
        cameraPosition = GameObject.Find("CameraPos").transform;
    }
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
