using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Texture2D[] cursorImgs;
    int count = 0;
    public float x;
    public float y;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void CursorTexture(int index)
    {
        switch (index)
        {
            case 0: Cursor.SetCursor(cursorImgs[0], new Vector2(x ,y), CursorMode.Auto); return;
            case 1: Cursor.SetCursor(cursorImgs[1], new Vector2(x, y), CursorMode.Auto); return;
            case 2: return;
            case 3: return;
        }
    }
    public void CursorChangeBtn()
    {
        CursorTexture(count%3);
        count++;
    }
}
