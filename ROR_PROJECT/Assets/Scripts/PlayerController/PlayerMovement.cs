using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : MonoBehaviour
{ 
    /// <summary>
    /// Virutal Sript which contains basic movements of every character.
    /// Create unique character's script and inherite this script.
    /// </summary>

    [Header("Movement")]
    //Player control references
    protected Rigidbody m_Rigidbody;
    protected PlayerInputSystem m_InputSystem;
    protected Transform m_Orientation;
    protected Transform m_Camera;

    private enum MovementState
    {
        Walking,
        Running,
        Air

    }
    private MovementState m_State;

    [Header("Ground Check")]
    [SerializeField]
    protected float playerHeight;
    [SerializeField]
    protected float groundCheckOffset;
    [SerializeField]
    protected LayerMask ground;
    [SerializeField]
    protected bool grounded;

    [Header("Settings")]
    [SerializeField]
    protected float groundMultiplier;
    [SerializeField]
    protected float airMultiplier;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private bool isSprinting;
    [SerializeField]
    protected float runSpeed;
    [SerializeField]
    protected float jumpForce;
    [SerializeField]
    protected float jumpCooldown;
    [SerializeField]
    protected float jumpCount;
    [SerializeField]
    protected float jumpTimes;
    [SerializeField]
    protected float groundDrag;
    [SerializeField]
    protected float airDrag;
    [SerializeField]
    private float maxSlopeAngle;
    private RaycastHit slopHit;
    private bool exitingSlope;
    RaycastHit hit;

    [Header("Debug")]
    [SerializeField]
    protected bool isRunning;
    [SerializeField]
    protected bool readyToJump;
    [SerializeField]
    protected float horizontalInput;
    [SerializeField]
    protected float verticalInput;
    [SerializeField]
    protected Vector3 moveDirection;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.freezeRotation = true;

        m_Orientation = GameObject.Find("Orientation").transform;
        m_Camera = GameObject.Find("Main Camera").transform;

        m_InputSystem = new PlayerInputSystem();
        m_InputSystem.Player.Jump.performed += Jump;
        m_InputSystem.Player.Sprint.performed += Sprint;
        m_InputSystem.Player.E.performed += Interact;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected virtual void Update()
    {
        GroundCheck();
        MyInput();
        SpeedLimiter();
        StateHandler();
        DragControl();
        //Debug.Log(m_Rigidbody.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void OnEnable()
    {
        m_InputSystem.Player.Enable();
    }

    protected virtual void OnDisable()
    {
        m_InputSystem.Player.Disable();
    }

    private void Interact(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if(Physics.Raycast(m_Camera.transform.position, m_Camera.forward, out hit, 3f))
        {
            if(hit.transform.tag == "lootBox")
            {
                hit.transform.GetComponent<SpawnItem>()?.Spawn();
                return;
            }
            if(hit.transform.tag == "item")
            {
                var target = hit.transform.gameObject;
                CharacterManager.Instance.AddItem(target.name);
                Destroy(target);
                return;
            }
        }
    }

    protected virtual void MyInput()
    {
        Vector2 tempVec2 = m_InputSystem.Player.Move.ReadValue<Vector2>();
        horizontalInput = tempVec2.x;
        verticalInput = tempVec2.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (Physics.SphereCast(transform.position, 1f, Vector3.down, out hit, playerHeight * 0.5f + groundCheckOffset, ground))
        {
            Gizmos.DrawWireSphere(hit.transform.position, 0.5f);
        }
    }

    protected virtual void GroundCheck()
    {
        if(Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hit, playerHeight * 0.25f + groundCheckOffset, ground))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        m_Rigidbody.useGravity = !OnSlope();
    }

    protected virtual void DragControl()
    {
        if (grounded)
        {
            m_Rigidbody.drag = groundDrag;
        }
        else if(!grounded)
        {
            m_Rigidbody.drag = 0;
        }
    }

    private void StateHandler()
    {
        if(isSprinting && grounded)
        {
            m_State = MovementState.Running;
            moveSpeed = runSpeed;
        }
        else if(grounded)
        {
            m_State = MovementState.Walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            m_State = MovementState.Air;
        }
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        if(isSprinting)
        {
            isSprinting = false;
        }
        else
        {
            isSprinting = true;
        }
    }

    protected virtual void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    protected virtual void JumpCalculation()
    {
        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, 0f, m_Rigidbody.velocity.z);
        m_Rigidbody.AddForce(this.transform.up * jumpForce, ForceMode.Impulse);
    }

    protected virtual void Jump(InputAction.CallbackContext context)
    {
        if(readyToJump && grounded)
        {
            readyToJump = false;
            exitingSlope = true;
            JumpCalculation();
            Invoke("ResetJump", jumpCooldown);
        }
    }

    protected virtual void Move()
    {
        moveDirection = m_Orientation.forward * verticalInput + m_Orientation.right * horizontalInput;

        if(OnSlope() && !exitingSlope)
        {
            m_Rigidbody.AddForce(GetSlopeMoveDirection() * moveSpeed * 20, ForceMode.Force);
            Vector3 tempVel = new Vector3(m_Rigidbody.velocity.x, 0f, m_Rigidbody.velocity.z);
            if (m_Rigidbody.velocity.y > 0)
            {
                m_Rigidbody.AddForce(Vector3.down * 80, ForceMode.Force);
            }
        }
        else if(grounded)
        {
            m_Rigidbody.AddForce(moveDirection.normalized * moveSpeed * groundMultiplier, ForceMode.Force);
        }
        else if(!grounded)
        {
            m_Rigidbody.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }
        
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopHit.normal).normalized;
    }

    protected virtual void SpeedLimiter()
    {
        if(OnSlope() && !exitingSlope)
        {
            if(m_Rigidbody.velocity.magnitude > moveSpeed)
            {
                m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 tempVel = new Vector3(m_Rigidbody.velocity.x, 0f, m_Rigidbody.velocity.z);

            if (tempVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = tempVel.normalized * moveSpeed;
                m_Rigidbody.velocity = new Vector3(limitedVel.x, m_Rigidbody.velocity.y, limitedVel.z);
            }
        }
    }
}
