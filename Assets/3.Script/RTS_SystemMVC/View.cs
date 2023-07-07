using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    [Header("UI Object for SetActive")]
    public GameObject myUnitView;
    public GameObject minimap;
    public GameObject targetView;
    [Space]
    [Header("MyUnit View")]   
    public Image m_champImg;
    public Text m_champLevel;
    public Slider m_expSlider;
    public Slider m_hpSlider;
    public Slider m_mpSlider;
    public Text m_expTxt;
    public Text m_hpTxt;
    public Text m_mpTxt;
    [Space]
    [Header("Target View")]
    public Image t_champImg;
    public Text t_username;
    public Text t_champName;
    public Text t_lvlTxt;
    public Text t_atk;
    public Text t_as;
    public Text t_ar;
    public Text t_ms;
    public Text t_hpTxt;
    public Text t_mpTxt;

    [SerializeField]
    RTS_Cam.RTS_Camera cam;
    [SerializeField]
    Unit myUnit;
    [SerializeField]
    Unit targetUnit;

    private void Start()
    {
        cam = FindObjectOfType<RTS_Cam.RTS_Camera>();
    }
    private void Update()
    {
        
    }
    private void LateUpdate()
    {
        TargetInfoUpdate();
    }


    public void BtnInitMyUnit()
    {
        myUnit = targetUnit;
        targetUnit = null;
        InitMyUnit(myUnit);
    }
    public void InitMyUnit(Unit unit)
    {
        //todo : myUnit일 경우 카메라나 컨트롤러와 연동해야해요.
        cam.InitPlayer(unit);
        FindObjectOfType<CommandMachine>().Init(unit);
    }

    void TargetInfoUpdate()
    {
        if (targetUnit != null)
        {
            if (!targetView.activeSelf) targetView.SetActive(true);
            SetTargetUI(targetUnit);
        }
        else if (targetView.activeSelf) targetView.SetActive(false);
    }
    void SetTargetUI(Unit unit)
    {
        t_champImg.sprite = unit.img;
        t_username.text = unit.champName;
        t_champName.text = unit.champName;
        t_lvlTxt.text = $"Level : {unit.level}";
        t_atk.text = $"{unit.atk}";
        t_as.text = $"{unit.attackSpeed}";
        t_ms.text = $"{unit.moveSpeed}";
        t_ar.text = $"{unit.range}";
        t_hpTxt.text = $"{unit.curHp} / {unit.maxHp}";
        t_mpTxt.text = $"{unit.curMp} / {unit.maxMp}";
    }
    public void SetTarget(Unit unit)
    {
        targetUnit = unit;
    }
    public void ResetTarget()
    {
        targetUnit = null;
    }
}
