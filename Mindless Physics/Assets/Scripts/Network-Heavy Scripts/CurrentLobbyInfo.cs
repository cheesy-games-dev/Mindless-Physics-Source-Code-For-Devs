using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

namespace NetworkHeavy{
    public class CurrentLobbyInfo : MonoBehaviourPunCallbacks
    {   
        public List<LobbyPlayerItem> playerItemsList = new List<LobbyPlayerItem>();
        public LobbyPlayerItem playerItemPrefab;
        public Transform playerItemParent;
        public float TimerToUpdatePlayerList = 5f;
        private bool TimeToUpdatePlayerList = true;

         void Awake(){
             TimeToUpdatePlayerList = true;
         }

         void Start(){
            if(PhotonNetwork.InRoom){
                UpdatePlayerList();
            }
         }

         public void Update(){
             if(PhotonNetwork.InRoom){
                 if(TimeToUpdatePlayerList){
                         TimeToUpdatePlayerList = false;
                         Invoke(nameof(UpdatePlayerList), TimerToUpdatePlayerList);
                }
             }
         }

         public override void OnJoinedRoom()
         {
             base.OnJoinedRoom();
             Debug.Log("Joined Room! " + PhotonNetwork.CurrentRoom.Name);
             UpdatePlayerList();
         }

         public override void OnLeftRoom()
         {
             base.OnLeftRoom();
             Debug.Log("Left Room!");
             UpdatePlayerList();
         }

         public override void OnPlayerEnteredRoom(Player newPlayer)
         {
             base.OnPlayerEnteredRoom(newPlayer);
             UpdatePlayerList();
         }

         public override void OnPlayerLeftRoom(Player otherPlayer)
         {
             base.OnPlayerLeftRoom(otherPlayer);
             UpdatePlayerList();
         }

         void UpdatePlayerList()
         {
             TimeToUpdatePlayerList = true;
             foreach (LobbyPlayerItem item in playerItemsList){
                 Destroy(item.gameObject);
             }
             playerItemsList.Clear();

             if(PhotonNetwork.CurrentRoom == null){
                 return;
             }

             foreach(KeyValuePair<int, Player> Player in PhotonNetwork.CurrentRoom.Players){
                 LobbyPlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
                 newPlayerItem.SetPlayerInfo(Player.Value);
                 playerItemsList.Add(newPlayerItem);
             }
         }
    }
}

