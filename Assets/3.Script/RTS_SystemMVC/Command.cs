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

public class SkillCommand : ICommand
{
    Unit myUnit;
    Vector3 t_pos;
    Unit t_unit;
    Skill skill;
    int index;
    public SkillCommand(Unit receiver, Skill skill, int index)
    {
        myUnit = receiver;
        this.skill = skill;
        t_unit = null;
        t_pos = myUnit.transform.position;
        this.index = index;
    }
    public SkillCommand(Unit receiver, Skill skill, int index, Vector3 t_pos)
    {
        myUnit = receiver;
        this.skill = skill;
        this.t_pos = t_pos;
        t_unit = null;
        this.index = index;
    }
    public SkillCommand(Unit receiver, Skill skill, int index, Unit t_unit)
    {
        myUnit = receiver;
        this.skill = skill;
        this.t_unit = t_unit;
        t_pos = myUnit.transform.position;
        this.index = index;
    }
    public void Execute()
    {
        ExecuteSkill();
    }
    void ExecuteSkill()
    {
        if (skill != null)
        {
            switch (skill.inputType)
            {
                case InputType.Instant: myUnit.state_skillmove.Init(skill, index); myUnit.SetState(myUnit.state_skillmove); break;
                case InputType.Target: myUnit.state_skillmove.Init(skill, index, t_unit); myUnit.SetState(myUnit.state_skillmove); break;
                case InputType.NonTarget: myUnit.state_skillmove.Init(skill, index, t_pos); myUnit.SetState(myUnit.state_skillmove); break;
            }
        }
    }
}

public class PerformCommand : ICommand
{
    Unit myUnit;
    float time;
    Skill skill;
    public PerformCommand(Unit receiver, float time, Skill skill)
    {
        myUnit = receiver;
        this.time = time;
        this.skill = skill;
    }
    public void Execute()
    {
        myUnit.state_perform.Init(time, skill);
        myUnit.SetState(myUnit.state_perform);
    }
}