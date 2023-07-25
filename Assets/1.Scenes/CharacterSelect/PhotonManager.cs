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
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
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
            print("닉네임 입력 바람, 2~8글자");
            return;
        }
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        print("서버접속완료");      
        PhotonNetwork.JoinLobby();
    }
    public void Btn_Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("연결끊김");
        print(cause);
        StartGame();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby(); print("로비접속완료");
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
        print("방만들기 완료");
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
        roomMakeLog.text = "방만들기실패" + message;
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        roomMakeLog.text = "방참가실패" + message;
    }
    public override void OnJoinRandomFailed(short returnCode, string message) => roomMakeLog.text = "방참가실패" + message;
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다.</color>");
    }

    void RoomRenewal()
    {
        for (int i = 0; i < ChatText.Length; i++)
        {
            ChatText[i].text = string.Empty;
        }
    }




    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);
            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있나요? : " + PhotonNetwork.InLobby);
            print("연결됐나요? : " + PhotonNetwork.IsConnected);
        }
    }

    [PunRPC] // RPC는 플레이어가 속해있는 모든 방 인원에게 전달
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
