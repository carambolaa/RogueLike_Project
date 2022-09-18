using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAt : MonoBehaviour
{
    private Transform mainCamera;

    private void Start()
    {
        mainCamera = Camera.main.transform;
    }

    private void Update()
    {
        if (gameObject.activeSelf == true)
        {
            this.transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
            float currentSize = 0.003f * Vector3.Distance(mainCamera.position, this.transform.position) / 6;
            this.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
        }
    }
}
