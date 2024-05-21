using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("References")] 
    public PlayerBaseState currentState;
    public PlayerStateFactory States;
    private Rigidbody _rb;
    
    [Header("variables"),Space(5)]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumpPressed;
    public bool isFishing;
    public float gravity = 9.8f;
    [SerializeField] private LayerMask groundLayerMask;
    [Header("walking keybinds")] 
    [SerializeField] private KeyCode forward;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode back;
    [SerializeField] private KeyCode right;
    [SerializeField] private bool isMovementPressed;
    public bool inBoat;
    [Header("Jumping settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rayCastLength;

    [SerializeField] private Animator animator;

    public Collider currentZone;
    
    //getters and setters
    public PlayerBaseState CurrentState
    {
        get => currentState;
        set => currentState = value;
    }
    public Rigidbody Rb
    {
        get => _rb;
        set => _rb = value;
    }
    public bool IsJumpPressed
    {
        get => isJumpPressed;
        set => isJumpPressed = value;
    }
    public bool IsGrounded
    {
        get => isGrounded;
        set => isGrounded = value;
    }
    public float Gravity
    {
        get => gravity;
        set => gravity = value;
    }
    public float JumpForce
    {
        get => jumpForce;
        set => jumpForce = value;
    }
    public KeyCode Forward
    {
        get => forward;
        set => forward = value;
    }
    public KeyCode Left
    {
        get => left;
        set => left = value;
    }
    public KeyCode Back
    {
        get => back;
        set => back = value;
    }
    public KeyCode Right
    {
        get => right;
        set => right = value;
    }
    public bool IsMovementPressed
    {
        get => isMovementPressed;
        set => isMovementPressed = value;
    }

    public bool inTheAir;
    public AudioSource walkingSource;
    public bool canWalk;

    private void Awake()
    {
        //Get Rigidbody
        _rb = GetComponent<Rigidbody>();
        // setup state
        States = new PlayerStateFactory(this);
        currentState = States.Grounded();
        currentState.EnterState();
    }

    private void Update()
    {
        GameManager gameManager = GameManager.Instance;
        forward = gameManager.GetKey("Forward");
        left = gameManager.GetKey("Left");
        back = gameManager.GetKey("Back");
        right = gameManager.GetKey("Right");
        
        //Ground check   
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, rayCastLength, groundLayerMask) || Physics.Raycast(transform.position, new Vector3(0, -0.5f, 0.5f), out hit, rayCastLength * 1.5f, groundLayerMask) || Physics.Raycast(transform.position, new Vector3(0, 0.5f, -0.5f), out hit, rayCastLength * 1.5f, groundLayerMask);
        if (canWalk)
        {
            isJumpPressed = Input.GetKey(gameManager.GetKey("Jump")) && isGrounded;

            isMovementPressed = Input.GetKey(forward) || Input.GetKey(left) || Input.GetKey(back)
                                 || Input.GetKey(right);
        }

        inTheAir = !isGrounded;
        
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, rayCastLength, groundLayerMask);
        //Debug.Log(_isGrounded);
        
        
        currentState.UpdateStates();

        //character animator
        if (isFishing)
        {
            animator.SetInteger("State", 4);
            DisablePlayerMovement(true);
        }
        else if (inTheAir && !inBoat)
        {
            animator.SetInteger("State", 3);
        }
        else if (isMovementPressed && isGrounded)
        {
            animator.SetInteger("State", 2);
        }
        else if (!isMovementPressed)
        {
            animator.SetInteger("State", 1);
        }

        //audio steps
        if (IsMovementPressed && !inTheAir)
        {
            walkingSource.enabled = true;
        }
        else
        {
            walkingSource.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateStates();
    }

    public bool CanSetSpawnPoint(LayerMask mask)
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.8f, mask);
    }

    public void DisablePlayerMovement(bool isFishing)
    {
        if (isFishing)
        {
            canWalk = false;
            IsMovementPressed = false;
        }
        else 
        {
            IsMovementPressed = false;
            canWalk = false;
            animator.SetInteger("State", 1);
        }
    }

    public void EnablePlayerMovement()
    {
        canWalk = true;
    }
}