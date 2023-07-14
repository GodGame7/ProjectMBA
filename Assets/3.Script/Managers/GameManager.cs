using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;

    // 다른 스크립트에서 싱글턴 인스턴스에 접근하기 위한 프로퍼티
    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // 인스턴스가 이미 존재하는지 확인하고, 존재하지 않으면 현재 객체를 할당합니다.
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // 게임 매니저 초기화 로직
        InitializeGameManager();
    }
    #endregion
    private void InitializeGameManager()
    {
        // 게임 매니저 초기화 코드 작성
        // ...
    }
    AudioSource audioSource;
    [SerializeField] Texture2D[] cursorImgs;
    int count = 0;
    public float x;
    public float y;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        CursorTexture(0);
    }

    public void PlayAFX(AudioClip afx)
    {
        audioSource.PlayOneShot(afx);
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
