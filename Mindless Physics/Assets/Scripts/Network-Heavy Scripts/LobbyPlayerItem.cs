using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkHeavy {
public class LobbyPlayerItem : MonoBehaviourPunCallbacks
{
    public Image playerColor;
    public Button kickButton;
    public TMP_Text playerName;
    public PlayerColorScript playerColorScript;
    ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
    Player player;
    void Awake(){
        playerColorScript = FindAnyObjectByType<PlayerColorScript>();
        kickButton.gameObject.SetActive(false);
    }
    public void SetPlayerInfo(Player _player){        
        playerName.text = _player.NickName;
        player = _player;
        UpdatePlayerItem(player);
    }

    void Update()
    {
        if(!props.ContainsKey("r")){
            props.Add("r", playerColorScript.universalPlayerColor.r);
        }
        if(!props.ContainsKey("g")){
            props.Add("g", playerColorScript.universalPlayerColor.g);
        }
        if(!props.ContainsKey("b")){
            props.Add("b", playerColorScript.universalPlayerColor.b);
        }
        if((float)props["r"] != playerColorScript.universalPlayerColor.r){
            props["r"] = playerColorScript.universalPlayerColor.r;
        }
        if((float)props["g"] != playerColorScript.universalPlayerColor.g){
            props["g"] = playerColorScript.universalPlayerColor.g;
        }
        if((float)props["b"] != playerColorScript.universalPlayerColor.b){
            props["b"] = playerColorScript.universalPlayerColor.b;
        }
        playerName.text = player.NickName;
        if(player.IsLocal){
            PhotonNetwork.SetPlayerCustomProperties(props);
        }
        if(PhotonNetwork.IsMasterClient && !PhotonNetwork.OfflineMode){
            kickButton.gameObject.SetActive(true);
            kickButton.onClick.AddListener(CloseConnection);
        }
        else{
            kickButton.gameObject.SetActive(false);
            kickButton.onClick.RemoveListener(CloseConnection);
            kickButton.onClick.RemoveAllListeners();
        }
    }

    private void CloseConnection(){
        PhotonNetwork.CloseConnection(player);
    }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
            if (player == targetPlayer){
                UpdatePlayerItem(targetPlayer);
            }
        }

        public void UpdatePlayerItem(Player targetPlayer)
        {
            float red = 0;
            if(targetPlayer.CustomProperties.ContainsKey("r")){
                red = (float)targetPlayer.CustomProperties["r"];
                props["c"] = (float)targetPlayer.CustomProperties["r"];
            }
            float green = 0;
             if(targetPlayer.CustomProperties.ContainsKey("g")){
                green = (float)targetPlayer.CustomProperties["g"];
                props["c"] = (float)targetPlayer.CustomProperties["g"];
            }
            float blue = 0;
             if(targetPlayer.CustomProperties.ContainsKey("b")){
                blue = (float)targetPlayer.CustomProperties["b"];
                props["c"] = (float)targetPlayer.CustomProperties["b"];
            }

            playerColor.color = new Color(red, green, blue, 255);
        }
    }
}
