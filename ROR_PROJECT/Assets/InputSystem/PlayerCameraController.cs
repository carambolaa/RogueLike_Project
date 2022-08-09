using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    private float sensX;
    [SerializeField]
    private float sensY;

    private float xRotation;
    private float yRotation;

    private PlayerInputSystem m_InputSystem;
    private Transform player;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_InputSystem = new PlayerInputSystem();
        player = GameObject.Find("Orientation").transform;
    }

    private void OnEnable()
    {
        m_InputSystem.Player.Enable();
    }

    private void OnDisable()
    {
        m_InputSystem.Player.Disable();
    }

    private void Update()
    {
        CameraRotate();
    }

    private void CameraRotate()
    {
        Vector2 tempMouseVec = m_InputSystem.Player.Look.ReadValue<Vector2>();
        float mouseX = tempMouseVec.x * Time.deltaTime * sensX;
        float mouseY = tempMouseVec.y * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        player.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
