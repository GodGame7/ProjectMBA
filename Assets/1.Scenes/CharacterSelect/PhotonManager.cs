using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public Text StatusText, LobbyInfoText;
    public GameObject PanelConnect, PanelRoom, PanelLobby;

    [Header("Connect")]
    public Text NicknameText;
    public InputField NickNameInput;
    [Header("Lobby")]
    public InputField roomInput;
    public Text roomMakeLog;
    [Header("Room(Chat & Select)")]
    public Text roomNameText;
    public Text message;
    public Text[] ChatText;
    public GameObject[] Team1Users;
    public GameObject[] Team2Users;

    [Header("ETC")]
    public PhotonView PV;

    private void Start()
    {
        StartGame();
    }
    private void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "�κ� / " + PhotonNetwork.CountOfPlayers + "����";
    }

    void StartGame()
    {
        PanelConnect.SetActive(true);
        PanelLobby.SetActive(false);
        PanelRoom.SetActive(false);
    }
    public void Btn_Connect()
    {
        if (NickNameInput.text == string.Empty || NickNameInput.text.Length < 2)
        {
            print("�г��� �Է� �ٶ�, 2~8����");
            return;
        }
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        print("�������ӿϷ�");      
        PhotonNetwork.JoinLobby();
    }
    public void Btn_Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("�������");
        print(cause);
        StartGame();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby(); print("�κ����ӿϷ�");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        NicknameText.text = PhotonNetwork.LocalPlayer.NickName;
        roomMakeLog.text = string.Empty;
        PanelConnect.SetActive(false);
        PanelLobby.SetActive(true);
        PanelRoom.SetActive(false);
    }
    public void Btn_CreateRoom() => 
        PhotonNetwork.CreateRoom(roomInput.text == "" ? "Room" + Random.Range(0, 100) : roomInput.text, new RoomOptions { MaxPlayers = 4 });
    public void Btn_JoinRoom() => 
        PhotonNetwork.JoinRoom(roomInput.text);
    public void Btn_JoinOrCreateRoom() => 
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
    public void Btn_JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void Btn_LeaveRoom() => PhotonNetwork.LeaveRoom();
    public override void OnCreatedRoom()
    {
        print("�游��� �Ϸ�");
    }
    public override void OnJoinedRoom()
    {
        PanelConnect.SetActive(false);
        PanelLobby.SetActive(false);
        PanelRoom.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        RoomRenewal();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        roomMakeLog.text = "�游������" + message;
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        roomMakeLog.text = "����������" + message;
    }
    public override void OnJoinRandomFailed(short returnCode, string message) => roomMakeLog.text = "����������" + message;
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�.</color>");
    }

    void RoomRenewal()
    {
        for (int i = 0; i < ChatText.Length; i++)
        {
            ChatText[i].text = string.Empty;
        }
    }




    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            print("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);
            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }
            print(playerStr);
        }
        else
        {
            print("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            print("�� ���� : " + PhotonNetwork.CountOfRooms);
            print("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            print("�κ� �ֳ���? : " + PhotonNetwork.InLobby);
            print("����Ƴ���? : " + PhotonNetwork.IsConnected);
        }
    }

    [PunRPC] // RPC�� �÷��̾ �����ִ� ��� �� �ο����� ����
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
        {
            if (ChatText[i].text == string.Empty)
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        }
        if (!isInput)
        {
            for (int i = 1; i < ChatText.Length; i++)
            {
                ChatText[i - 1].text = ChatText[i].text;
                ChatText[ChatText.Length - 1].text = msg;
            }
        }
    }
}
