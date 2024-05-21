using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    private Vector3 _movement;
    private const float Speed = 5f;
    private float switchStateDelay = 0.25f;
    private float switchStateTimer;

    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        switchStateTimer = switchStateDelay;
    }

    public override void EnterState()
    {
        // Update animation
    }

    public override void UpdateState()
    {
        // Get input from the player using key bindings
        float verticalInput = Input.GetKey(Ctx.Forward) ? 1f : Input.GetKey(Ctx.Back) ? -1f : 0f;
        float horizontalInput = Input.GetKey(Ctx.Right) ? 1f : Input.GetKey(Ctx.Left) ? -1f : 0f;

        _movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        // Debug.Log($"Vertical input = {_movement.z} & horizontal input = {_movement.x}.");

        switchStateTimer -= Time.deltaTime;

        if (switchStateTimer <= 0f)
        {
            CheckSwitchStates();
        }
    }

    public override void FixedUpdateState()
    {
        MovePlayer(_movement);
    }

    public override void ExitState()
    {
        // Reset the timer when exiting the state
        switchStateTimer = switchStateDelay;
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMovementPressed) SwitchState(Factory.Idle());
    }

    public override void InitializeSubState()
    {
    }

    private void MovePlayer(Vector3 direction)
    {
        Vector3 newPosition = Ctx.transform.position + direction * Speed * Time.fixedDeltaTime;
        Ctx.Rb.MovePosition(newPosition);
        
        //rotation
        if (direction.magnitude > 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            Ctx.transform.rotation = Quaternion.Slerp(Ctx.transform.rotation, toRotation, Time.fixedDeltaTime * 10f);
        }
    }
}
