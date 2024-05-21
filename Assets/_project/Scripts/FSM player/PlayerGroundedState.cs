using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{

    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory) 
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
    
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
        if (Ctx.IsJumpPressed)
        {
            SwitchState(Factory.Jump());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(Ctx.IsMovementPressed ? Factory.Walk() : Factory.Idle());
    }
}
