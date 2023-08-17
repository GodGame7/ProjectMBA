using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    string roomName = string.Empty;
    int playerCount = 0;
    int maxPlayer = 4;
    [System.NonSerialized]
    public Text text_roomName;
    public Text text_players;
    private void Awake()
    {
        text_roomName = GetComponentInChildren<Text>();
    }
    public void UpdateInfo()
    {
        text_roomName.text = string.Format("{0}", roomName);
        text_players.text = string.Format("[ {0} / {1} ]", playerCount, maxPlayer);
    }
}
