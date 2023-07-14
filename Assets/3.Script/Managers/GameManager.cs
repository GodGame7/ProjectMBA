using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;

    // �ٸ� ��ũ��Ʈ���� �̱��� �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // �ν��Ͻ��� �̹� �����ϴ��� Ȯ���ϰ�, �������� ������ ���� ��ü�� �Ҵ��մϴ�.
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // ���� �Ŵ��� �ʱ�ȭ ����
        InitializeGameManager();
    }
    #endregion
    private void InitializeGameManager()
    {
        // ���� �Ŵ��� �ʱ�ȭ �ڵ� �ۼ�
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
