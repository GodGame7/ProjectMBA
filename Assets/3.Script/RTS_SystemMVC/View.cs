using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    [Header("UI Object for SetActive")]
    public GameObject bottomUI;
    public GameObject minimap;
    public GameObject targetInfo;
    [Space]
    [Header("Bottom UI")]   
    public Image m_champImg;
    public Text m_champLevel;
    public Slider m_expSlider;
    public Slider m_hpSlider;
    public Slider m_mpSlider;
    public Text m_expTxt;
    public Text m_hpTxt;
    public Text m_mpTxt;
    [Space]
    [Header("Target Info")]
    public Image t_champImg;
    public Text t_username;
    public Text t_champName;
    public Text t_lvlTxt;
    public Text t_hpTxt;
    public Text t_mpTxt;

    Unit myUnit;
    Unit targetUnit;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }
    }


    private void LateUpdate()
    {
        TargetInfoUpdate();
    }
    void TargetInfoUpdate()
    {
        if (targetUnit != null)
        {
            if (!targetInfo.activeSelf) targetInfo.SetActive(true);
            SetTargetUI(targetUnit);
        }
        else targetInfo.SetActive(false);
    }
    void SetTargetUI(Unit unit)
    {
        t_champImg.sprite = unit.img;
        t_username.text = unit.champName;
        t_champName.text = unit.champName;
        t_lvlTxt.text = $"Level : {unit.level}";
        t_hpTxt.text = $"{unit.curHp} / {unit.maxHp}";
        //t_mpTxt.text = $"{unit.curHp} / {unit.maxHp}";
        //unitData¿¡ mp¾øÀ½ todo
    }
}
