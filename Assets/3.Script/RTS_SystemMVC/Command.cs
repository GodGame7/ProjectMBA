using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void Execute();
}

public class MoveCommand : ICommand
{
    Unit myUnit;
    Vector3 t_pos;
    public MoveCommand(Unit receiver, Vector3 t_pos)
    {
        myUnit = receiver;
        this.t_pos = t_pos;
    }
    public void Execute()
    {
        myUnit.state_move.Init(t_pos);
        myUnit.SetState(myUnit.state_move);
    }
}

public class AttackCommand : ICommand
{
    Unit myUnit;
    Vector3 t_pos;
    Unit t_unit;
    public AttackCommand(Unit receiver, Vector3 t_pos)
    {
        myUnit = receiver;
        this.t_pos = t_pos;
        t_unit = null;
    }
    public AttackCommand(Unit receiver, Unit t_unit)
    {
        myUnit = receiver;
        this.t_unit = t_unit;
        t_pos = myUnit.transform.position;
    }
    public void Execute()
    {
        if (t_unit != null) {
            myUnit.state_attack.Init(t_unit, 1);
            myUnit.SetState(myUnit.state_attack);
        }
        else { 
            myUnit.state_attack.Init(t_pos);
            myUnit.SetState(myUnit.state_attack);
        }
    }
}
public class StopCommand : ICommand
{
    Unit myUnit;
    public StopCommand(Unit receiver)
    {
        myUnit = receiver;
    }
    public void Execute()
    {
        myUnit.Stop();
        myUnit.SetState(myUnit.state_idle);
    }
}