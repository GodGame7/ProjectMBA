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
    public int inputMode = 0; // 0 = Idle, 1 = Command

    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
        cm = FindObjectOfType<CommandMachine>();
    }
    private void Update()
    {
        OnMouseClickMove(); //��Ŭ��
        OnMouseClickTargetting(); //��Ŭ��
    }


    #region MouseInputSystem
    // UpdateMethod
    public void OnMouseClickMove() //��Ŭ��
    {
        if (Input.GetMouseButton(1))
        {
            if (CheckUIClick())
            {
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
            { } // Ÿ�� �������� ����
            else { } // Ÿ�� ���� null
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
}

