using System;
using NetworkHeavy;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunScript : MonoBehaviourPunCallbacks
{
    public AmmoManager ammoManager;
    NetworkGrabbableObject networkGrabbableObject;
    public Transform bulletSpawn;
    public Rigidbody bulletObject;
    [Range(0, 100)]
    public float damage = 10f;
    public float bulletDestroyTime = 3f;
    public float bulletForce = 50f;
    public bool fire = false;
    public int myAmmo = 0;
    public enum AmmoType{
        light,
        shotgun,
        medium,
        heavy,
        misc,
    }

    public AmmoType ammoType = AmmoType.light;
    public float recoilForce = 30;
    public float fireRate = 2f;
    public int bulletsToShoot = 1;
    public ParticleSystem muzzleFlash;
    public AudioSource audioSource;
    public AudioClip shootAudioClip;

    void Start()
    {
        fire = false;
        networkGrabbableObject = GetComponent<NetworkGrabbableObject>();
        networkGrabbableObject.activated.AddListener(StartFire);
        networkGrabbableObject.deactivated.AddListener(StopFire);
        audioSource = GetComponent<AudioSource>();
    }

    private void StartFire(ActivateEventArgs arg0)
    {
        Debug.Log("Started to Shoot");
        fire = true;
        if(myAmmo <= 0) return;
        if(isAuto) return;
        Shoot();
        photonView.RPC(methodName: nameof(Shoot), RpcTarget.OthersBuffered);
    }

    private void StopFire(DeactivateEventArgs arg0)
    {
        Debug.Log("Stopped Shooting");
        fire = false;
    }
    public bool isAuto = false;
    private float nextTimeToFire = 0f;
    void Update()
    {
        if(!photonView.IsMine) return;
        ManageAmmo(0);
        if(myAmmo <= 0) return;
        if(!isAuto) return;
        if(fire && Time.time >= nextTimeToFire){
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            photonView.RPC(methodName: nameof(Shoot), RpcTarget.OthersBuffered);
        }
    }

    [PunRPC]
    private void Shoot()
    {
        ManageAmmo(removeBullet: 1);
        muzzleFlash.Play();
        audioSource.PlayOneShot(shootAudioClip);
        if(!photonView.IsMine) return;
        for(int i = 0; i < bulletsToShoot; i++){
            Bullet spawnedBullet = PhotonNetwork.Instantiate(bulletObject.name, bulletSpawn.position, bulletSpawn.rotation).GetComponent<Bullet>();
            spawnedBullet.damage = this.damage;
            spawnedBullet.destroyTime = this.bulletDestroyTime;
            if (damage <= 0) {
                spawnedBullet.damage = 0;
                spawnedBullet.isDeadly = false;
            }
            spawnedBullet.GetComponent<Rigidbody>().linearVelocity = bulletSpawn.forward * bulletForce;
            GetComponent<Rigidbody>().rotation = Quaternion.Euler(-recoilForce + transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    private void ManageAmmo(int removeBullet)
    {
        // Ammo Manager
        NetworkPlayer[] networkPlayers = FindObjectsOfType<NetworkPlayer>();
        foreach (NetworkPlayer networkPlayer in networkPlayers){
            if(networkPlayer.photonView == this.photonView){
                ammoManager = GetComponent<AmmoManager>();
            }
        }
        if(ammoManager == null) return;
        if(ammoType == AmmoType.light){
            myAmmo = ammoManager.lightAmmo;
            ammoManager.lightAmmo -= removeBullet;
        }
        if(ammoType == AmmoType.shotgun){
            myAmmo = ammoManager.shotgunAmmo;
            ammoManager.shotgunAmmo -= removeBullet;
        }
        if(ammoType == AmmoType.medium){
            myAmmo = ammoManager.mediumAmmo;
            ammoManager.mediumAmmo -= removeBullet;
        }
        if(ammoType == AmmoType.heavy){
            myAmmo = ammoManager.heavyAmmo;
            ammoManager.heavyAmmo -= removeBullet;
        }
        if(ammoType == AmmoType.misc){
            myAmmo = ammoManager.miscAmmo;
            ammoManager.miscAmmo -= removeBullet;
        }
    }
}
