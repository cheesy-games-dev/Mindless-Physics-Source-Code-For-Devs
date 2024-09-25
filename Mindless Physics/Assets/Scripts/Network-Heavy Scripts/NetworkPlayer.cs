using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


namespace NetworkHeavy
{
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonRigidbodyView))]
    public class NetworkPlayer : MonoBehaviourPunCallbacks
    {
        [NonSerialized] public PhotonView view;
        private PhotonRigidbodyView rigidbodyView;
        public GameObject clientSide;
        public GameObject serverSide;
        public GameObject colliders;
        public UnityEngine.Object[] deleteForPeasants;
        public UnityEngine.Object[] deleteForServer;
        public GameObject[] disableForClient;
        public Camera streamerCamera;

        void Start()
        {
            view = photonView;
            rigidbodyView = GetComponent<PhotonRigidbodyView>();
            clientSide.SetActive(true);
            serverSide.SetActive(true);
            colliders.SetActive(true);
            SetViewProperties();
            SetRigidbodyViewProperties();
            CustomizeControls();
        }

        void SetViewProperties(){
            view.OwnershipTransfer = OwnershipOption.Fixed;
            view.Synchronization = ViewSynchronization.UnreliableOnChange;
            view.observableSearch = PhotonView.ObservableSearch.AutoFindAll;
        }

        void SetRigidbodyViewProperties(){
            rigidbodyView.m_SynchronizeAngularVelocity = true;
            rigidbodyView.m_SynchronizeVelocity = true;
            rigidbodyView.m_TeleportEnabled = true;
            rigidbodyView.m_TeleportIfDistanceGreaterThan = 3f;
        }

        void Update()
        {
            CustomizeControls();
            if (view.IsMine){
                clientSide.SetActive(true);
                serverSide.SetActive(true);
                colliders.SetActive(true);
                foreach(var gO in disableForClient){
                    if(gO.GetComponent<MeshFilter>() != null){
                        gO.layer = 7;
                        return;
                    }
                    else if(gO.GetComponent<SkinnedMeshRenderer>() != null){
                        gO.layer = 7;
                    }
                    else{
                        gO.SetActive(false);
                    }
                }
                if(PhotonNetwork.IsMasterClient){
                    foreach(var classObject in deleteForPeasants){
                        Destroy(classObject);
                    }
                }
            }
            else{
                Destroy(streamerCamera.gameObject);
                clientSide.SetActive(false);
                serverSide.SetActive(true);
                colliders.SetActive(false);
                foreach(var classObject in deleteForServer){
                    Destroy(classObject);
                }
                if(FindAnyObjectByType<IKTargetFollowVRRig>() != null){
                    Destroy(FindAnyObjectByType<IKTargetFollowVRRig>());
                }
            }
        }

        public XRDirectInteractor[] xrDirectInteractors;

        private void CustomizeControls() {
            foreach (XRDirectInteractor directInteractor in xrDirectInteractors) {
                directInteractor.selectActionTrigger = (XRBaseInputInteractor.InputTriggerType)PlayerPrefs.GetInt("grabMode") + 1;
            }
        }

}
}
