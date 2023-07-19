using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rush", menuName = "Skill/Warrior/Rush")]
public class Rush : Skill
{
    [Header("Rush Skill Setting")]
    public float speed;
    Unit t_unit;


}
