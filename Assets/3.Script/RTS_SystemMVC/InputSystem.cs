using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour
{
    [Header("레이어 세팅")]
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
        OnMouseClickMove(); //우클릭
        OnMouseClickTargetting(); //좌클릭
    }


    #region MouseInputSystem
    // UpdateMethod
    public void OnMouseClickMove() //우클릭
    {
        if (Input.GetMouseButton(1))
        {
            if (CheckUIClick())
            {
                return;
            }
            // 이동 메서드
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
            { } // 타겟 유닛으로 설정
            else { } // 타겟 유닛 null
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
}

