using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum InputType { Instant, Target, NonTarget }
public enum OutputType { None, Target, AoE }
public enum TargetType { None, Ally, Enemy }
public class Skill : ScriptableObject
{
    [Header("Indicator")]    
    public InputType inputType;    
    public OutputType outputType;    
    public TargetType targetType;
    public string skillName;
    public float[] dmgs;
    public int level = 1;
    public int maxLv = 5;
    public float cooldownTime;
    public float activateTime;
    public float durationTime;
    public float range;
    public float area;

    [Tooltip("0=none, 1=stun, 2=knock")]
    public int ccType;
    public float ccDurationTime;

    [Header("Audio Clip")]
    public AudioClip[] afxs;

    protected Unit myUnit;
    public virtual void Init(Unit unit)
    {
        myUnit = unit;
    }
    public virtual void Activate() { }
    public virtual void Activate(GameObject targetObj) { }
    public virtual void Activate(Unit targetUnit) { }
    public virtual void Activate(Vector3 targetPos) { }
    public virtual void Execute() { }
    public virtual void Execute(GameObject targetObj) { }
    public virtual void Execute(Unit targetUnit) { }
    public virtual void Execute(Vector3 targetPos) { }
    public virtual void Exit() { }
    public virtual void Exit(GameObject targetObj) { }
    public virtual void Exit(Unit targetUnit) { }
    public virtual void Exit(Vector3 targetPos) { }

}
