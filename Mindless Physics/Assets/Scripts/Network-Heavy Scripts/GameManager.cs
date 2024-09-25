using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NetworkHeavy{
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum GameState{
        GameRunning,
        GamePaused,
        GameEnded
    }
    public enum GameMode{
            Sandbox,
            Deathmatch,
            Zombies,
            BattleRoyale
    }
    public GameState gameState = GameState.GameRunning;
    public GameMode gameMode;
    public string selectedMap;
    public MapManager selectedMapManager;
    public bool infiniteAmmo = false;
    
    void Awake(){
        // Initiate Photon View
        photonView.OwnershipTransfer = OwnershipOption.Takeover;
        photonView.TransferOwnership(PhotonNetwork.CurrentRoom.masterClientId);
        if(PhotonNetwork.IsMasterClient){
            photonView.RequestOwnership();
        }
        // End

        FindMapManager();
        DontDestroyOnLoad(gameObject);
    }

    public void AttemptJoinGame(){
        if(gameState == GameState.GameRunning){
            Invoke(nameof(StartGame), 5);
        }
    }

    public string playerGameObjectName = "Player (XR Rig)";

    [ContextMenu("Start Game")]
    public void StartGame(){
        int selectedPoint = Random.Range(0,  selectedMapManager.spawnPoints.Length);
        Transform selectedSpawnPoint = selectedMapManager.spawnPoints[selectedPoint];
        GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerGameObjectName, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
    }

    [ContextMenu("End Game")]
    public void EndGame(){
        if(PhotonNetwork.IsMasterClient){
            gameState = GameState.GameEnded;
        }
    }

    private bool endedGame = false;

    void Update(){
        FindMapManager();    
        if(PhotonNetwork.IsMasterClient){
            if(gameState == GameState.GameEnded){
                if(!endedGame){
                    PhotonNetwork.LoadLevel("MainMenu");
                    endedGame = true;
                }                
            }
        }
    }

    private bool isParentedToGameScene = false;

    void FindMapManager(){
        if(selectedMapManager == null){
            MapManager[] mapManagers = FindObjectsOfType<MapManager>();
            foreach(MapManager mapManager in mapManagers){
                if(mapManager.mapName == selectedMap){
                    selectedMapManager = mapManager;
                    mapManager.gameManager = this;
                    selectedMapManager.gameObject.SetActive(true);
                }
                else{
                    selectedMapManager.gameObject.SetActive(false);
                }
            }
        }
        else{
            if(!isParentedToGameScene){
                transform.SetParent(selectedMapManager.transform, false);
                if(transform.parent == selectedMapManager.transform){
                    transform.SetParent(null, false);
                    isParentedToGameScene = true;
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting){
            stream.SendNext(gameState);
            stream.SendNext(gameMode);
            stream.SendNext(selectedMap);
            stream.SendNext(infiniteAmmo);
        }
        else{
            this.gameState = (GameState)stream.ReceiveNext();
            this.gameMode = (GameMode)stream.ReceiveNext();
            this.selectedMap = (string)stream.ReceiveNext();
            this.infiniteAmmo = (bool)stream.ReceiveNext();
        }
    }
}
}
