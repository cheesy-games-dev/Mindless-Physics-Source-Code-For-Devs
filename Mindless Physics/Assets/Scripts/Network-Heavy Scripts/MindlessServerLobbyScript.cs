using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using Photon.Voice.PUN;

namespace NetworkHeavy {
    public class MindlessServerLobbyScript : MonoBehaviourPunCallbacks
    {
        // Lobby Settings
        public bool isOnline = false;
        public string selectedMap = "City";
        public enum GameMode{
            Sandbox,
            Deathmatch,
            Zombies,
            BattleRoyale
        }
        public GameMode gameMode = GameMode.Sandbox;
        public bool isPrivate = false;
        public bool infiniteAmmo = false;

        // Variables
        public TMP_Dropdown mapDropdown;
        public TMP_Dropdown masterClientMapDropdown;
        public TMP_Text lobbyText;
        public TMP_Dropdown.OptionData[] maps;
        public GameObject masterClientCommands;
        public GameObject GameSettingsObject;
        public GameObject playerGameObject;
        public Transform radioTransform;

        void Awake(){
            foreach(TMP_Dropdown.OptionData map in maps) {
                mapDropdown.options.Add(map);
                masterClientMapDropdown.options.Add(map);
            }
            PhotonNetwork.SendRate = 20;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.EnableCloseConnection = true;
            isOnline = false;
            selectedMap = "City";
            gameMode = GameMode.Sandbox;
            
        }

        void Start(){
            if(PhotonNetwork.InRoom){
                PhotonNetwork.Instantiate(voiceObject.name, radioTransform.position, radioTransform.rotation);
            }
        }

        public void SwitchGameMode(int newGameModeValue) {
            GameMode newGameMode = GameMode.Sandbox;
            if(newGameModeValue == 1){
                newGameMode = GameMode.Deathmatch;
            }
            else if(newGameModeValue == 2){
                newGameMode = GameMode.Zombies;
            }
            else if (newGameModeValue == 3){
                newGameMode = GameMode.BattleRoyale;
            }
            gameMode = newGameMode;
        }

        public void InfinteAmmo(bool value){
            if(PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient){
                infiniteAmmo = value;
            }
        }

        public void ToggleLine(bool line){
            if(!line) PhotonNetwork.LeaveLobby();
            PhotonNetwork.LeaveRoom();
            isOnline = line;
            PhotonNetwork.OfflineMode = !isOnline;
            if (line){
                Debug.Log("Connecting To Master Server");
                PhotonNetwork.ConnectUsingSettings();  
            }
            else{
                Debug.Log("Disconnected From Master Server");
                PhotonNetwork.Disconnect();
            }
        }

        public void TogglePrivacy(bool privacy){
            isPrivate = privacy;
            if(PhotonNetwork.InRoom) {
                if(PhotonNetwork.IsMasterClient){
                    PhotonNetwork.CurrentRoom.IsVisible = !isPrivate;
                    Debug.Log("Room Privacy is " + !PhotonNetwork.CurrentRoom.IsVisible);
                }
            }
        }

        void Update(){
            selectedMap = mapDropdown.options[mapDropdown.value].text;
            if(PhotonNetwork.InRoom){
                lobbyText.text = "Lobby: " + PhotonNetwork.CurrentRoom.Name;
                if(PhotonNetwork.IsMasterClient){
                    masterClientCommands.SetActive(true);
                }
                else{
                    masterClientCommands.SetActive(false);
                    GameSettingsObject.SetActive(false);
                }
            }
            else{
                masterClientCommands.SetActive(false);
                GameSettingsObject.SetActive(false);
                lobbyText.text = "Lobby: " + "None";
            }            
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Master Server");
            PhotonNetwork.JoinLobby();  
        }

        public void CreateRoom(){
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.EnableCloseConnection = true;
            PhotonNetwork.OfflineMode = !isOnline;

            // Room Properties
            RoomOptions roomOptions = new RoomOptions();
            ExitGames.Client.Photon.Hashtable RoomCustomProps = new ExitGames.Client.Photon.Hashtable();
            RoomCustomProps.Add("h", PlayerPrefs.GetString("username"));
            RoomCustomProps.Add("n", PlayerPrefs.GetString("username") + "'s Game");
            RoomCustomProps.Add("g", gameMode);
            roomOptions.IsVisible = !isPrivate;
            Debug.Log("Privacy Mode: " + !roomOptions.IsVisible);
            roomOptions.CustomRoomProperties = RoomCustomProps;
            roomOptions.CustomRoomPropertiesForLobby = new string[3] { "h", "n", "g"};
            roomOptions.MaxPlayers = 4;
            roomOptions.BroadcastPropsChangeToAll = true;
            Debug.Log("Max Players: " + roomOptions.MaxPlayers);
            TryCreateRoom(roomOptions);
            Debug.Log("Attempting To Create Room!");
        }
        
        public GameObject gameManagerObject;

        public void StartGame(){
            if(!PhotonNetwork.IsMasterClient) return;
            // Create GameManager
            GameManager gameManager = PhotonNetwork.Instantiate(gameManagerObject.name, Vector3.zero, Quaternion.identity).gameObject.AddComponent<GameManager>();          
            
            // GameManager Variables
            gameManager.gameMode = (GameManager.GameMode)gameMode;
            gameManager.selectedMap = selectedMap;
            gameManager.infiniteAmmo = infiniteAmmo;
            gameManager.AttemptJoinGame();
            
            PhotonNetwork.LoadLevel("Game");
        }
        public void JoinRoom(string code){
            PhotonNetwork.JoinRoom(code);
        }

        public void LeaveRoom(){
            PhotonNetwork.LeaveRoom();
            Debug.Log("Attempting To Leave Room!");
        }

        void TryCreateRoom(RoomOptions roomOptions){
            if(PhotonNetwork.OfflineMode){
                PhotonNetwork.CreateRoom("Offline", roomOptions, TypedLobby.Default);
                return;
            }
            PhotonNetwork.CreateRoom(Random.Range(100000, 999998 + Random.Range(0, 1)).ToString(), roomOptions, TypedLobby.Default);
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log("Joined Lobby!");
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            Debug.Log("Created Room!");
        }
        public PhotonVoiceView voiceObject;
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Joined Room! " + PhotonNetwork.CurrentRoom.Name);
            PhotonNetwork.Instantiate(voiceObject.name, radioTransform.position, radioTransform.rotation);
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            Debug.Log("Left Room!");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
        }
    }
}
