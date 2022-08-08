using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    //Player control references
    private Rigidbody m_Rigidbody;
    private PlayerInputSystem m_InputSystem;

    [SerializeField]
    private float groundMultiplier;
    [SerializeField]
    private float airMultiplier;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float horizontalInput;
    [SerializeField]
    private float verticalInput;
    [SerializeField]
    private Vector3 moveDirection;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        m_InputSystem = new PlayerInputSystem();
        m_InputSystem.Player.Jump.performed += Jump;
    }

    private void Update()
    {
        MyInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnEnable()
    {
        m_InputSystem.Player.Enable();
    }

    private void OnDisable()
    {
        m_InputSystem.Player.Disable();
    }

    private void MyInput()
    {
        Vector2 tempVec2 = m_InputSystem.Player.Move.ReadValue<Vector2>();
        horizontalInput = tempVec2.x;
        verticalInput = tempVec2.y;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
    }

    private void Move()
    {
        //Debug.Log(m_InputSystem.Player.Move.ReadValue<Vector2>());
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        m_Rigidbody.AddForce(moveDirection.normalized * moveSpeed * groundMultiplier, ForceMode.Force);
    }
}
