using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour
{
    [Header("���̾� ����")]
    [SerializeField] LayerMask layerUnit;
    [SerializeField] LayerMask layerGround;
    CommandMachine cm;
    [Tooltip("InputMode   0 = Idle / 1 = Targetting ")]
    public int inputMode = 0; // 0 = Idle, 1 = Attack, 2 = Skill
    [Header("��ų")]


    [Header("View ����")]
    View view;

    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
        cm = FindObjectOfType<CommandMachine>();
        view = FindObjectOfType<View>();
    }
    private void Update()
    {
        OnMouseClickMove(); //��Ŭ��
        OnMouseClickTargetting(); //��Ŭ��
        OnAttackButton();
        AttackInputMode();
    }


    #region MouseInputSystem
    // UpdateMethod
    public void OnMouseClickMove() //��Ŭ��
    {
        if (Input.GetMouseButton(1))
        {
            if (CheckUIClick())
            {
                Debug.Log("UI Move is Locked!");
                return;
            }
            // �̵� �޼���
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
            {
                cm.AddCommand(new MoveCommand(cm.receiver, hit.point));
            }
        }
    }
    public void OnMouseClickTargetting()
    {
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
            { view.SetTarget(hit.transform.GetComponent<Unit>()); } // Ÿ�� �������� ����
            else { view.ResetTarget(); } // Ÿ�� ���� null
        }
    }
    bool CheckUIClick()//Ŭ�� ��ġ�� UI ������ Ȯ��
    {
        // UI Ŭ�� ���� Ȯ��
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

    private void OnAttackButton()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //todo Ŀ�� �̹��� ���� �� Indicator ����, GameManager �α� �ý��ۿ� "��ǥ�� �������ּ���" ����
            inputMode = 1;
        }
    }
    public void AttackInputMode()
    {
        if (inputMode == 1)
        { 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
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
                        if (t_unit.GetTeam() == cm.receiver.GetTeam())
                        {
                            Debug.Log("Cannot Attack Ally");
                        }
                        else if (t_unit.GetTeam() != cm.receiver.GetTeam())
                        {
                            //todo Ŀ�� �̹��� �����ߴ� �� �� off�޼ҵ�
                            inputMode = 0;
                            cm.AddCommand(new AttackCommand(cm.receiver, t_unit));
                        }
                    }
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
                {
                    Vector3 t_pos = hit.point;
                    inputMode = 0;
                    cm.AddCommand(new AttackCommand(cm.receiver, t_pos));
                }
            }
        }
    }
}

