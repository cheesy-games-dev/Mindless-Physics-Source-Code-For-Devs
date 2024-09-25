using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float damage = 10f;
    public float destroyTime = 1f;
    public bool isDeadly = true; 
    private void Start(){
        Invoke(nameof(DestroyBullet), destroyTime);
    }
    void OnCollisionEnter(Collision collision){
        isDeadly = false;
    }
    public void DestroyBullet(){
        PhotonNetwork.Destroy(this.photonView.gameObject);
    }
}
