using System.Collections;
using System.Collections.Generic;
using NetworkHeavy;
using UnityEngine;

public class RagdollMode : MonoBehaviour
{
    public bool startRagdoll = false;
    public Transform ragdollArmature;
    public Rigidbody[] allBodyParts;
    public Collider beforeRagdollCollider;
    public GameObject[] disableWhenRagdoll;

    void Awake(){
        if(beforeRagdollCollider != null){
            beforeRagdollCollider.enabled = true;
            beforeRagdollCollider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            beforeRagdollCollider.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
        allBodyParts = ragdollArmature.GetComponentsInChildren<Rigidbody>();
        foreach(GameObject gO in disableWhenRagdoll){
            gO.SetActive(true);
        }
        foreach(Rigidbody b in allBodyParts){
            b.mass = 0.1f;
            b.isKinematic = true;
            b.useGravity = false;
            b.GetComponent<Collider>().enabled = false;
        } 
        if(startRagdoll){
            Ragdoll();
        }
        bool isServerPlayer = GetComponentInParent<NetworkPlayer>() != null && !GetComponentInParent<NetworkPlayer>().photonView.IsMine;
        if(isServerPlayer){
            foreach(Rigidbody b in allBodyParts){
                b.mass = 0.1f;
                b.isKinematic = true;
                b.useGravity = false;
                b.GetComponent<Collider>().enabled = true;
            } 
            if(beforeRagdollCollider != null){
                beforeRagdollCollider.enabled = false;
            }
        }
    }

    [ContextMenu("Ragdoll")]
    public void Ragdoll(){
        if(beforeRagdollCollider != null){
            beforeRagdollCollider.enabled = false;
            beforeRagdollCollider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            beforeRagdollCollider.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
        allBodyParts = ragdollArmature.GetComponentsInChildren<Rigidbody>();
        foreach(GameObject gO in disableWhenRagdoll){
            gO.SetActive(false);
        }
        foreach(Rigidbody b in allBodyParts){
            b.mass = 0.1f;
            b.isKinematic = false;
            b.useGravity = true;
            b.GetComponent<Collider>().enabled = true;
        } 
    }
}
