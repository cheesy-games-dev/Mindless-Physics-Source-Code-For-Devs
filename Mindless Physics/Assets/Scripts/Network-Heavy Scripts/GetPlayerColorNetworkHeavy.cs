using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NetworkHeavy{
public class GetPlayerColorNetworkHeavy : MonoBehaviourPunCallbacks
{
    public Player player;
    public MeshRenderer meshRenderer;
    public SkinnedMeshRenderer skinnedMeshRenderer;

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            player = newPlayer;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            player = otherPlayer;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            player = PhotonNetwork.LocalPlayer;
        }

        public PhotonView myView;
        void Start()
    {   
        this.myView = GetComponentInParent<PhotonView>();
        meshRenderer = GetComponent<MeshRenderer>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if(this.myView.IsMine){
            player = myView.Owner;
        }
    }

    void Update()
    {
        if(player == null) return;
        Color playerColor = new Color((float)player.CustomProperties["r"], (float)player.CustomProperties["g"], (float)player.CustomProperties["b"], 255);
        if(meshRenderer != null) {
            meshRenderer.material.color = playerColor;
        }
        else if (skinnedMeshRenderer != null) {
            skinnedMeshRenderer.material.color = playerColor;
        }
    }
}
}

