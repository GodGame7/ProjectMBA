using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{
    public Image champIcon;
    public Image info1;
    public Image info2;
    public Text userName;
    public Champion selected;
    private void Start()
    {
        Clear();
    }
    public void Clear()
    {
        userName.text = string.Empty;
    }
    public void SetName(string name)
    {
        userName.text = name;
    }
    public void SetIcon(Champion selectedChamp)
    {
        selected = selectedChamp;
        champIcon.sprite = selectedChamp.img;
    }
}
