using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [SerializeField]
    protected Image abilityIcon;
    protected enum KeyBind { E, R, Shift, M1, M2}
    [SerializeField]
    protected KeyBind key;
    protected PlayerInputSystem m_InputSystem;
    protected InputAction m_PlayerAction;
    protected float cooldown;
    protected bool isCooling;
    protected float damage;


    protected virtual void Awake()
    {
        m_InputSystem = new PlayerInputSystem();
        switch (key)
        {
            case KeyBind.E:
               m_PlayerAction = m_InputSystem.Player.E;
                break;
            case KeyBind.R:
                m_PlayerAction = m_InputSystem.Player.R;
                break;
            case KeyBind.Shift:
                m_PlayerAction = m_InputSystem.Player.Shift;
                break;
            case KeyBind.M1:
                m_PlayerAction = m_InputSystem.Player.M1;
                break;
            case KeyBind.M2:
                m_PlayerAction = m_InputSystem.Player.M2;
                break;
        }

    }

    protected virtual void OnEnable()
    {
        m_InputSystem.Player.Enable();
    }

    protected virtual void OnDisable()
    {
        m_InputSystem.Player.Disable();
    }

    protected virtual void Ability_UI(float elapsed)
    {
        abilityIcon.fillAmount = elapsed;
    }
}
