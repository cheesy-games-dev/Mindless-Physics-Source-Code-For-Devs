using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using NetworkHeavy;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof (PhotonRigidbodyView))]
public class NetworkGrabbableObject : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable, IPunObservable
{
    [Header("Networked Values")]
    public PhotonView photonView;
    public PhotonRigidbodyView photonRigidbodyView;
    public bool isGrabbed = false;
    public Transform defaultParent;
    public MapManager mapManager;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting){
            stream.SendNext((bool)isGrabbed);
        }
        else{
            isGrabbed = (bool)stream.ReceiveNext();
        }
    }
    void Start(){
        movementType = MovementType.VelocityTracking;
        photonRigidbodyView = GetComponent<PhotonRigidbodyView>();
        if(photonView == null){
            photonView = GetComponent<PhotonView>();
        }
        if(photonView == null){
            photonView = GetComponentInParent<PhotonView>();
        }
        if(photonView == null){
            photonView = this.gameObject.AddComponent<PhotonView>();
        }
        if(photonView != null){
            photonView.OwnershipTransfer = OwnershipOption.Takeover;
            photonRigidbodyView.m_TeleportEnabled = true;
            photonRigidbodyView.m_SynchronizeVelocity = true;
            photonRigidbodyView.m_SynchronizeAngularVelocity = true;
        }
        isGrabbed = false;  
        attachEaseInTime = 0.05f;
    }

    void Update(){
        if(mapManager == null) mapManager = FindAnyObjectByType<GameManager>().selectedMapManager;
        GetComponent<Rigidbody>().useGravity = !isGrabbed;
        if(!isGrabbed){
            if(defaultParent == null)
                transform.SetParent(mapManager.mainObjectsParent, true);
            else
                transform.SetParent(defaultParent, true);
        }
    }

    ActivateEventArgs activateEventArgs;    
    DeactivateEventArgs deactivateEventArgs;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if(isGrabbed && photonView.Owner != PhotonNetwork.LocalPlayer) return;
        isGrabbed = true;
        photonView.RequestOwnership();
        foreach (Collider collider in colliders) {
            collider.enabled = false;
        }
        base.OnSelectEntering(args);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        isGrabbed = false;
        foreach (Collider collider in colliders) {
            collider.enabled = true;
        }
        base.OnSelectExited(args);
    }
    protected override void OnActivated(ActivateEventArgs args)
    {
        if(isGrabbed && photonView.Owner != PhotonNetwork.LocalPlayer) return;
        photonView.RPC(nameof(ActivateRPC), RpcTarget.OthersBuffered);
        activateEventArgs = args;
        base.OnActivated(args);
    }

    [PunRPC]
    void ActivateRPC(){
        activated.Invoke(activateEventArgs);
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        photonView.RPC(nameof(DeactivateRPC), RpcTarget.OthersBuffered, args);
        deactivateEventArgs = args;
        base.OnDeactivated(args);
    }

    [PunRPC]
    void DeactivateRPC( ){
        deactivated.Invoke(deactivateEventArgs);
    }
}