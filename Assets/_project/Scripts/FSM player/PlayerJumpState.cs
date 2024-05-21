using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    

    public override void EnterState()
    {
        Jump();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsGrounded && !Ctx.IsJumpPressed)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(Ctx.IsMovementPressed ? Factory.Walk() : Factory.Idle());
    }

    void Jump()
    {
        // Apply an upward force for jumping
        Ctx.Rb.AddForce(Vector3.up * Ctx.JumpForce, ForceMode.Impulse);
    }
}
