using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update


    [SerializeField]
    Button findMatchBtn;

    [SerializeField]
    GameObject searchingPanel;

    void Start()
    {
        searchingPanel.SetActive(false);
        findMatchBtn.gameObject.SetActive(false);


        PhotonNetwork.ConnectUsingSettings();
        findMatchBtn.onClick.AddListener(FindMatch);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We are Connected to Photon! on" + PhotonNetwork.CloudRegion + "Server");
        PhotonNetwork.AutomaticallySyncScene = true;
        findMatchBtn.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindMatch()
    {
        searchingPanel.SetActive(true);
        findMatchBtn.gameObject.SetActive(false);

        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Searching for a Game");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Could not Find Room - Creating a Room");
        MakeRoom();
    }

    void MakeRoom()
    {
        int randomRoomName = Random.Range(0, 5000);
        RoomOptions roomOptions =
            new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 2
            };
        PhotonNetwork.CreateRoom("RoomaName_" + randomRoomName, roomOptions);
        Debug.Log("Room Created, waiting for another player");
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + "/2 Starting Game");
            PhotonNetwork.LoadLevel(1);
        }
    }
}
