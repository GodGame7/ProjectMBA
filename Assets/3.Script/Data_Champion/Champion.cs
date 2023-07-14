using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Champion", menuName = "CreateSO/Champion")]
public class Champion : ScriptableObject
{
    [Header("UI용 이미지")]
    public Sprite img;
    [Header("시작 프리펩, 시작 위치")]
    public GameObject prefab;
    public Vector3 startPos;
    [Header("기본 스탯")]
    public string champName;
    public float maxHp;
    public float maxMp;
    public float moveSpeed;
    [Header("기본 공격")]
    public float atk;
    public float atkRange;
    public float atkSpeed;
    [Header("스킬")]
    public Skill skillQ;
    public Skill skillW;
    public Skill skillE;
    public Skill skillR;
    public Skill skillD;
    public Skill skillF;

}
