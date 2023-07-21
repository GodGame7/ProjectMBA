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
        print("�������ӿϷ�");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        NicknameText.text = PhotonNetwork.LocalPlayer.NickName;
    }
    public void Btn_Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("�������");
    }
    public void Btn_JointLobby()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby(); print("�κ����ӿϷ�");
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
        print("�游��� �Ϸ�");
        RoomnameText.text = PhotonNetwork.CurrentRoom.Name;
    }
    public override void OnJoinedRoom() => print("������ �Ϸ�");
    public override void OnCreateRoomFailed(short returnCode, string message) => print("�游��� ����");
    public override void OnJoinRoomFailed(short returnCode, string message) => print("�� ���� ����");
    public override void OnJoinRandomFailed(short returnCode, string message) => print("�� �������� ����");

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
}
