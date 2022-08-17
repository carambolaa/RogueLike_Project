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

    [Header("PlayerValue")]
    protected int HP;
    protected int gold;
    protected int XP;

    //Events
    public event Action OnRecieveDamage;
    public event Action<Transform> OnDealDamage;

    public static PlayerMovement instance { get; private set; }

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
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.freezeRotation = true;

        m_Orientation = GameObject.Find("Orientation").transform;
        m_Camera = GameObject.Find("Main Camera").transform;

        m_InputSystem = new PlayerInputSystem();
        m_InputSystem.Player.Jump.performed += Jump;
        m_InputSystem.Player.E.performed += Interact;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.groundMultiplier = 9;
        this.airMultiplier = 3;
        this.moveSpeed = 6;
        this.runSpeed = 9;
        this.jumpForce = 9;
        this.jumpCooldown = 0.25f;
        this.groundDrag = 9;
    }

    protected virtual void Update()
    {
        MyInput();
        GroundCheck();
        DragControl();
        SpeedLimiter();
    }

    protected virtual void FixedUpdate()
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

    private void damageDealt(Transform enemy)
    {
        Debug.Log(enemy.position);
        OnDealDamage?.Invoke(enemy);
    }

    public void RecieveDamage(int damage)
    {
        this.HP -= damage;
        OnRecieveDamage?.Invoke();
    }

    private void Interact(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if(Physics.Raycast(m_Camera.transform.position, m_Camera.forward, out hit, 10f))
        {
            if(hit.transform.tag == "item")
            {
                Type comp = Type.GetType(hit.transform.name);
                if (!gameObject.GetComponent(comp) && comp != null)
                {
                    gameObject.AddComponent(comp);
                }
                Destroy(hit.transform.gameObject);
            }
        }
    }

    private void RemoveCode()
    {
        Destroy(GetComponent<BasicItem>());
    }

    protected void UpdateHP(int HP)
    {
        this.HP += HP;
    }

    public int GetHP()
    {
        return this.HP;
    }

    protected void UpdateGold(int Gold)
    {
        this.gold += Gold;
    }

    public int GetGold()
    {
        return this.gold;
    }

    protected virtual void MyInput()
    {
        Vector2 tempVec2 = m_InputSystem.Player.Move.ReadValue<Vector2>();
        horizontalInput = tempVec2.x;
        verticalInput = tempVec2.y;
    }

    protected virtual void GroundCheck()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + groundCheckOffset, ground))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    protected virtual void DragControl()
    {
        if(grounded)
        {
            m_Rigidbody.drag = groundDrag;
        }
        else if(!grounded)
        {
            m_Rigidbody.drag = airDrag;
        }
    }

    protected virtual void ResetJump()
    {
        readyToJump = true;
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
            JumpCalculation();
            Invoke("ResetJump", jumpCooldown);
        }
    }

    protected virtual void Move()
    {
        moveDirection = m_Orientation.forward * verticalInput + m_Orientation.right * horizontalInput;

        if(grounded)
        {
            m_Rigidbody.AddForce(moveDirection.normalized * moveSpeed * groundMultiplier, ForceMode.Force);
        }
        else if(!grounded)
        {
            m_Rigidbody.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }
    }

    protected virtual void SpeedLimiter()
    {
        Vector3 tempVel = new Vector3(m_Rigidbody.velocity.x, 0f, m_Rigidbody.velocity.z);
        float targetSpeed = isRunning ? runSpeed : moveSpeed;
        if(tempVel.magnitude > targetSpeed)
        {
            Vector3 limitedVel = tempVel.normalized * targetSpeed;
            m_Rigidbody.velocity = new Vector3(limitedVel.x, m_Rigidbody.velocity.y, limitedVel.z);
        }
    }
}
