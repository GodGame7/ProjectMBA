using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour
{
    [Header("키 세팅")]
    public KeyCode q;
    public KeyCode w;
    public KeyCode e;
    public KeyCode r;
    public KeyCode d;
    public KeyCode f;
    Unit myUnit;
    [Header("레이어 세팅")]
    [SerializeField] LayerMask layerUnit;
    [SerializeField] LayerMask layerGround;
    CommandMachine cm;
    [Tooltip("InputMode   0 = Idle / 1 = Targetting ")]
    public int inputMode = 0; // 0 = Idle, 1 = Attack, 2 = SkillTarget, 3 = NonTarget
    [Header("스킬")]
    SkillMachine sm;
    Skill cur_skill;
    int cur_skillIndex;
    Indicator indicator;
    [Header("View 연동")]
    View view;
    [SerializeField]ParticleSystem clickParticle;

    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
        view = FindObjectOfType<View>();
    }
    public void Init(Unit myUnit)
    {
        this.myUnit = myUnit;
        sm = myUnit.sm;
        cm = myUnit.cm;
    }
    private void Start()
    {
        //todo indicator = cm.receiver.GetComponentInChildren<Indicator>();
    }
    private void Update()
    {
        OnMouseClickMove(); //우클릭
        OnMouseClickTargetting(); //좌클릭
        OnAttackButton();
        AttackInputMode();
        StopInput();
        SkillKeyInput();
        TargetInputMode();
        NonTargetInputMode();
        if(particleTime < 1f) particleTime += Time.deltaTime;
    }


    #region MouseInputSystem
    float particleTime = 1f;
    void ClickParticlePlay(Vector3 hitPoint)
    {
        hitPoint.y = 0.11f;
        clickParticle.transform.position = hitPoint;
        if (particleTime >= 0.1f) { clickParticle.Play(); particleTime = 0; }
    }
    // UpdateMethod
    public void OnMouseClickMove() //우클릭
    {
        if (Input.GetMouseButton(1))
        {
            if (CheckUIClick())
            {
                Debug.Log("UI Move is Locked!");
                return;
            }
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
            {
                ClickParticlePlay(hit.point);
                cm.AddCommand(new MoveCommand(cm.receiver, hit.point));
            }
        }
    }
    public void OnMouseClickTargetting()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { view.ResetTarget(); }
        if (Input.GetMouseButtonDown(0))
        {
            if (CheckUIClick())
            {
                return;
            }
            else
            {
                LeftClickTargetting();
            }
        }
    }
    private void LeftClickTargetting()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
        {
            if (!hit.transform.GetComponent<Unit>().myUnit)
            { view.SetTarget(hit.transform.GetComponent<Unit>()); return; } // 타겟 유닛으로 설정
        }
    }
    bool CheckUIClick()//클릭 위치가 UI 위인지 확인
    {
        // UI 클릭 여부 확인
        bool isUIHit = false;
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        if (results != null)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    isUIHit = true;
                    break;
                }
            }
        }
        return isUIHit;
    }
    #endregion
    public void AttackInputMode()
    {
        if (inputMode == 1)
        { 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.CursorTexture(0);
                inputMode = 0;
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                GameManager.Instance.CursorTexture(0);
                inputMode = 0;
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
                {
                    Unit t_unit;
                    if (hit.transform.TryGetComponent(out t_unit))
                    {
                        if (t_unit.isAlive)
                        {
                            if (t_unit.GetTeam() == cm.receiver.GetTeam())
                            {
                                Debug.Log("Cannot Attack Ally");
                            }
                            else if (t_unit.GetTeam() != cm.receiver.GetTeam())
                            {
                                //todo 커서 이미지 변경했던 것 등 off메소드
                                GameManager.Instance.CursorTexture(0);
                                inputMode = 0;
                                cm.AddCommand(new AttackCommand(cm.receiver, t_unit));
                                return;
                            }
                        }
                    }
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
                {
                    Vector3 t_pos = hit.point;
                    GameManager.Instance.CursorTexture(0);
                    inputMode = 0;
                    ClickParticlePlay(hit.point);
                    cm.AddCommand(new AttackCommand(cm.receiver, t_pos));
                }
            }
        }
    }
    public void TargetInputMode()
    {
        //타게팅입력모드 index = 2
        if (inputMode == 2)
        {
            //cancle 메소드
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.CursorTexture(0);
                inputMode = 0;
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                GameManager.Instance.CursorTexture(0);
                inputMode = 0;
                return;
            }
            //좌클릭 입력 시 스킬 발동
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
                {
                    Unit t_unit;
                    if (hit.transform.TryGetComponent(out t_unit))
                    {
                        if (t_unit.isAlive)
                        {
                            if (t_unit.GetTeam() == cm.receiver.GetTeam())
                            {
                                Debug.Log("Cannot Attack Ally");
                            }
                            else if (t_unit.GetTeam() != cm.receiver.GetTeam())
                            {
                                //todo 커서 이미지 변경했던 것 등 off메소드
                                GameManager.Instance.CursorTexture(0);
                                inputMode = 0;
                                cm.AddCommand(new SkillCommand(cm.receiver, cur_skill, cur_skillIndex, t_unit));
                                return;
                            }
                        }
                    }
                }
                else { Debug.Log("유효하지 않은 목표입니다"); }
            }
        }
    }
    public void NonTargetInputMode()
    {
        //논타게팅입력모드 index = 3
        if (inputMode == 3)
        {
            //cancle 메소드
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.CursorTexture(0);
                inputMode = 0;
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                GameManager.Instance.CursorTexture(0);
                inputMode = 0;
                return;
            }
            //좌클릭 입력 시 스킬 발동
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
                {
                    GameManager.Instance.CursorTexture(0);
                    inputMode = 0;
                    cm.AddCommand(new SkillCommand(cm.receiver, cur_skill, cur_skillIndex, hit.point));
                    return;
                }
                else { Debug.Log("유효하지 않은 목표입니다"); }
            }
        }
    }


    void StopInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            cm.AddCommand(new StopCommand(cm.receiver));
        }
    }
    void OnAttackButton()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //todo 커서 이미지 변경 등 Indicator 설정, GameManager 로그 시스템에 "목표를 지정해주세요" 설정
            GameManager.Instance.CursorTexture(1);
            inputMode = 1;
        }
    }
    void SkillKeyInput()
    {
        if (Input.GetKeyDown(q))
        {
            int inputKey = 0;
            if (!sm.isSkillReady(inputKey))
            {
                //todo 쿨타임일 경우 처리
                Debug.Log("SkillState is Cooldown");
                return;
            }
            switch (sm.skills[inputKey].inputType)
            {
                case InputType.Instant:
                    cm.AddCommand(new SkillCommand(cm.receiver, sm.skills[inputKey], inputKey)); break;
                case InputType.Target:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 2;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
                case InputType.NonTarget:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 3;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
            }
        }
        if (Input.GetKeyDown(w))
        {
            int inputKey = 1;
            if (!sm.isSkillReady(inputKey))
            {
                //todo 쿨타임일 경우 처리
                Debug.Log("SkillState is Cooldown");
                return;
            }
            switch (sm.skills[inputKey].inputType)
            {
                case InputType.Instant:
                    cm.AddCommand(new SkillCommand(cm.receiver, sm.skills[inputKey], inputKey)); break;
                case InputType.Target:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 2;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
                case InputType.NonTarget:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 3;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
            }
        }
        if (Input.GetKeyDown(e))
        {
            int inputKey = 2;
            if (!sm.isSkillReady(inputKey))
            {
                //todo 쿨타임일 경우 처리
                Debug.Log("SkillState is Cooldown");
                return;
            }
            switch (sm.skills[inputKey].inputType)
            {
                case InputType.Instant:
                    cm.AddCommand(new SkillCommand(cm.receiver, sm.skills[inputKey], inputKey)); break;
                case InputType.Target:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 2;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
                case InputType.NonTarget:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 3;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
            }
        }
        if (Input.GetKeyDown(r))
        {
            int inputKey = 3;
            if (!sm.isSkillReady(inputKey))
            {
                //todo 쿨타임일 경우 처리
                Debug.Log("SkillState is Cooldown");
                return;
            }
            switch (sm.skills[inputKey].inputType)
            {
                case InputType.Instant:
                    cm.AddCommand(new SkillCommand(cm.receiver, sm.skills[inputKey], inputKey)); break;
                case InputType.Target:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 2;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
                case InputType.NonTarget:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 3;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
            }
        }
        if (Input.GetKeyDown(d))
        {
            int inputKey = 4;
            if (!sm.isSkillReady(inputKey))
            {
                //todo 쿨타임일 경우 처리
                Debug.Log("SkillState is Cooldown");
                return;
            }
            switch (sm.skills[inputKey].inputType)
            {
                case InputType.Instant:
                    cm.AddCommand(new SkillCommand(cm.receiver, sm.skills[inputKey], inputKey)); break;
                case InputType.Target:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 2;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
                case InputType.NonTarget:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 3;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
            }
        }
        if (Input.GetKeyDown(f))
        {
            int inputKey = 5;
            if (!sm.isSkillReady(inputKey))
            {
                //todo 쿨타임일 경우 처리
                Debug.Log("SkillState is Cooldown");
                return;
            }
            switch (sm.skills[inputKey].inputType)
            {
                case InputType.Instant:
                    cm.AddCommand(new SkillCommand(cm.receiver, sm.skills[inputKey], inputKey)); break;
                case InputType.Target:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 2;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
                case InputType.NonTarget:
                    GameManager.Instance.CursorTexture(1);
                    inputMode = 3;
                    cur_skill = sm.skills[inputKey];
                    cur_skillIndex = inputKey; break;
            }
        }
    }
}

