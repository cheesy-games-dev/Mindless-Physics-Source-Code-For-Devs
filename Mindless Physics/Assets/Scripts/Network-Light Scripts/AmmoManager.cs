using NetworkHeavy;
using Photon.Pun;
using UnityEngine;

public class AmmoManager : MonoBehaviour, IPunObservable
{
    public GameManager gameManager;
    public int lightAmmo;
    public int shotgunAmmo;
    public int mediumAmmo;
    public int heavyAmmo;
    public int miscAmmo;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(gameManager.infiniteAmmo) return;
        if(stream.IsWriting){
            stream.SendNext(lightAmmo);
            stream.SendNext(shotgunAmmo);
            stream.SendNext(mediumAmmo);
            stream.SendNext(heavyAmmo);
            stream.SendNext(miscAmmo);
        }
        else{
            lightAmmo = (int)stream.ReceiveNext();
            shotgunAmmo = (int)stream.ReceiveNext();
            mediumAmmo = (int)stream.ReceiveNext();
            heavyAmmo = (int)stream.ReceiveNext();
            miscAmmo = (int)stream.ReceiveNext();
        }
    }

    void Update(){
        if(gameManager == null) gameManager = FindAnyObjectByType<GameManager>();

        if(gameManager.infiniteAmmo){
            lightAmmo = 100;
            shotgunAmmo = 100;
            mediumAmmo = 100;
            heavyAmmo = 100;
            miscAmmo = 100;
        }
    }
}
