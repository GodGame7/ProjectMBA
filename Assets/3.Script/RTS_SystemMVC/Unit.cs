using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    RTS_Cam.RTS_Camera cam;
    [Header("��Ʈ��ũ ����")]
    public bool myUnit = false;
    public int team = 0; // 0 = blue, 1 = red, 2 = neutral;
    [Header("�ڵ����� ������Ʈ")]
    [SerializeField] GameObject marker;
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] public Animator anim;
    [Header("������ �ޱ�")]
    [SerializeField] Champion unitData;
    public Sprite img;
    public float maxHp;
    private float _curHp;
    public float curHp { get { return _curHp; } set { _curHp = value; } }
    public float maxMp;
    public float curMp;
    public float atk;
    public float range;
    private float _moveSpeed;
    public float moveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; SetMoveSpeed(); } }
    public int level;
    public string champName;
    private float _attackSpeed;
    public float attackSpeed { 
        get { return _attackSpeed; } 
        set { if (_attackSpeed != value) { 
                _attackSpeed = value; SetAttackSpeed(); } } }
    [Header("���� ���� ����")]
    public float attackCoolTime;
    public float currentAttackCoolTime;
    public bool isAttacking = false;
    [Header("���� ���� ����")]
    public float reviveTime;
    public bool isAlive;
    public bool isStopT;
    public bool canReceive() { if (cur_state == state_die || isStopT) return false; else return true; }
    //todo ����� ���� ���� ���� �Ǵ� �޼ҵ� �ʿ�.
    public IState cur_state;
    [Space]
    [Header("State Machine")]
    public IdleState state_idle;
    public AttackState state_attack;
    public MoveState state_move;
    public DieState state_die;
    void InitState()
    {
        state_idle = new IdleState(this);
        state_attack = new AttackState(this);
        state_move = new MoveState(this);
        state_die = new DieState(this);
        cur_state = state_idle;
    }
    #region Unity CallBack Method
    private void Awake()
    {
        cam = FindObjectOfType<RTS_Cam.RTS_Camera>();
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        Init_status();
        InitState();
    }
    #region StateMachine.Update~LateUpdate 
    private void Update()
    {
        cur_state.Update();
        if(currentAttackCoolTime <= attackCoolTime) currentAttackCoolTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        cur_state.FixedUpdate();
    }
    private void LateUpdate()
    {
        cur_state.LateUpdate();
    }
    #endregion
    #endregion

    #region �ܺ� ���� �޼ҵ� from StateMachine
    public void SetState(IState state)
    {
        cur_state.Exit();
        cur_state = state;
        cur_state.Enter();
    }
    public int GetTeam()
    {
        return team;
    }
    public void Stop()
    {
        navAgent.SetDestination(transform.position);
    }
    public void MoveTo(Vector3 t_pos)
    {
        navAgent.SetDestination(t_pos);
    }
    public void OnDamage(Unit attackUnit, float dmg)
    {
        if (isAlive) { curHp -= dmg; }
        
        if (curHp <= 0)
        {
            Debug.Log($"Team : {attackUnit.team}�� {attackUnit.champName}�� Team : {this.team}�� {this.champName}�� �׿����ϴ�.");
            curHp = 0;
            reviveTime = level * 5f;
            isAlive = false;
            SetState(state_die);
        }
    }
    public void Death()
    {
        navAgent.enabled = false;
        anim.Play("Death");
    }
    public void Revival()
    {
        curHp = maxHp;
        curMp = maxMp;
        isAlive = true;
        navAgent.enabled = true;
        SetState(state_idle);
    }
    #endregion

    #region Init�޼ҵ�
    public void InitMyUnit()
    {
        //todo : myUnit�� ��� ī�޶� ��Ʈ�ѷ��� �����ؾ��ؿ�.
        cam.InitPlayer(this);
        FindObjectOfType<CommandMachine>().Init(this);
    }
    public void Init_status()
    {
        isAlive = true;
        img = unitData.img;
        maxHp = unitData.maxHp;
        curHp = unitData.maxHp;
        maxMp = unitData.maxMp;
        curMp = unitData.maxMp;
        atk = unitData.atk;
        attackSpeed = unitData.atkSpeed;
        range = unitData.atkRange;
        moveSpeed = unitData.moveSpeed;
        level = 1;
        champName = unitData.champName;
    }
    #endregion



    void SetAttackSpeed()
    {
        //���� ��Ÿ�� ���
        attackCoolTime = 1f / _attackSpeed;
        currentAttackCoolTime = attackCoolTime;
        //���ݼӵ��� 1���� ������ �ִϸ��̼� ������ ����ϱ� ���ؼ� ��� ����, �ƴϸ� �⺻�ӵ� 1�� ���
        if (_attackSpeed > 1) anim.SetFloat("AttackSpeed", _attackSpeed);
        else anim.SetFloat("AttackSpeed", 1);
    }
    void SetMoveSpeed()
    {
        navAgent.speed = _moveSpeed / 40f;
    }
}
