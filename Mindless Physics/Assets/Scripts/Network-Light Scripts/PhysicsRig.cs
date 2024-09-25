using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhysicsRig : MonoBehaviour, IPunObservable
{   
    public Transform playerHead;

    public CapsuleCollider bodyCollider;

    public float bodyHeightMax = 2f;
    public float bodyHeightMin = 0.5f;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting){
            stream.SendNext((float)bodyCollider.height);
        }
        else{
            bodyCollider.height = (float)bodyHeightMax;
        }
    }

    void FixedUpdate()
    {
        // Set Body
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height / 2, playerHead.localPosition.z);

    }
}
