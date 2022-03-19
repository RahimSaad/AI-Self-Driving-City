using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class photonManager : MonoBehaviourPunCallbacks
{
    private const int maxCar = 20;
    public GameObject conBtn;
    
    void Start()
    {
        conBtn.SetActive(false);
        connectAndStart();
    }
        
    public void connectAndStart()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("SmartCity", new RoomOptions { MaxPlayers = maxCar }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(
            string.Format("x = {0}  ,  y = {1}  ,  z = {2}  ,  carModel = {3} , pos = {4}" 
            , btnHandler.instance.userData.carInfo.x
            , btnHandler.instance.userData.carInfo.y
            , btnHandler.instance.userData.carInfo.z
            , btnHandler.instance.userData.carModel
            , btnHandler.instance.userData.carInfo.pos)
            );

        PhotonNetwork.Instantiate(
               btnHandler.instance.userData.carModel
             , new Vector3(btnHandler.instance.userData.carInfo.x
                          , btnHandler.instance.userData.carInfo.y
                          , btnHandler.instance.userData.carInfo.z)
             , Quaternion.identity
        );

        //Debug.Log("loll");
        //PhotonNetwork.Instantiate(
        //       "Car_3"
        //     , new Vector3(125.1969f
        //                  , 3.293157e-06f
        //                  , -201.3321f)
        //     , Quaternion.identity
        //);

        conBtn.SetActive(true);
        GameManager.instance.turnCarView();
    }
    
    public override void OnLeftRoom()
    {
        
    }

}
