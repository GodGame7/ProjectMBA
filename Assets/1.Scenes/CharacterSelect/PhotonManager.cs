using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public Text StatusText;
    public Text NicknameText, RoomnameText;
    public InputField roomInput, NickNameInput;

    private void Awake()
    {
        
    }
    private void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Btn_Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        print("서버접속완료");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        NicknameText.text = PhotonNetwork.LocalPlayer.NickName;
    }
    public void Btn_Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("연결끊김");
    }
    public void Btn_JointLobby()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby(); print("로비접속완료");
    }
    public void Btn_CreateRoom() => 
        PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });
    public void Btn_JoinRoom() => 
        PhotonNetwork.JoinRoom(roomInput.text);
    public void Btn_JoinOrCreateRoom() => 
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
    public void Btn_JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void Btn_LeaveRoom() => PhotonNetwork.LeaveRoom();
    public override void OnCreatedRoom()
    {
        print("방만들기 완료");
        RoomnameText.text = PhotonNetwork.CurrentRoom.Name;
    }
    public override void OnJoinedRoom() => print("방참가 완료");
    public override void OnCreateRoomFailed(short returnCode, string message) => print("방만들기 실패");
    public override void OnJoinRoomFailed(short returnCode, string message) => print("방 참가 실패");
    public override void OnJoinRandomFailed(short returnCode, string message) => print("방 랜덤참가 실패");

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
}
