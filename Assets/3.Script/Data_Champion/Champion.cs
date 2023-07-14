using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Champion", menuName = "CreateSO/Champion")]
public class Champion : ScriptableObject
{
    [Header("UI�� �̹���")]
    public Sprite img;
    [Header("���� ������, ���� ��ġ")]
    public GameObject prefab;
    public Vector3 startPos;
    [Header("�⺻ ����")]
    public string champName;
    public float maxHp;
    public float maxMp;
    public float moveSpeed;
    [Header("�⺻ ����")]
    public float atk;
    public float atkRange;
    public float atkSpeed;
    [Header("��ų")]
    public Skill skillQ;
    public Skill skillW;
    public Skill skillE;
    public Skill skillR;
    public Skill skillD;
    public Skill skillF;

}
