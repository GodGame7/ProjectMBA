using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    RTS_Cam.RTS_Camera cam;
    public int team = 0; // 0 = blue, 1 = red, 2 = neutral;
    [SerializeField] GameObject marker;
    [Header("자동세팅 컴포넌트")]
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] public Animator anim;
    //
    [Header("데이터 받기")]
    [SerializeField] Champion unitData;
    public Sprite img;
    public float maxHp;
    public float curHp;
    public float maxMp;
    public float curMp;
    public float atk;
    public float range;
    public float moveSpeed;
    public int level;
    public string champName;
    public float attackSpeed;
    public bool myUnit = false;
    public bool canReceive() { return true; }
    //
    [SerializeField] IState cur_state;
    [Space]
    [Header("State Machine")]
    public IdleState state_idle;
    public AttackState state_attack;
    public MoveState state_move;
    void InitState()
    {
        state_idle = new IdleState(this);
        state_attack = new AttackState(this);
        state_move = new MoveState(this);
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

    #region 외부 접근 메소드 from StateMachine
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
    #endregion


    #region Init메소드
    public void InitMyUnit()
    {
        //todo : myUnit일 경우 카메라나 컨트롤러와 연동해야해요.
        cam.InitPlayer(this);
        FindObjectOfType<CommandMachine>().Init(this);
    }
    public void Init_status()
    {
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
}
