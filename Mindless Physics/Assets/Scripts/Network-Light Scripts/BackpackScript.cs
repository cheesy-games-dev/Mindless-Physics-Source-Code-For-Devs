using Photon.Pun;
using TMPro;
using UnityEngine;

public class BackpackScript : MonoBehaviour
{
    public PhotonView selectedPrefab;
    public AmmoManager ammoManager;
    public TMP_Text[] ammoTexts;
    public Transform bulletSpawn;
    public TMP_Text spawnGunText;
    public Transform originParent;

    private void Update() {
        transform.SetParent(originParent, true);
        spawnGunText.text = selectedPrefab.name;

        // ammo
        if (!ammoManager.gameManager.infiniteAmmo) {
            string templateAmmoString = "Ammo: ";
            ammoTexts[0].text = "Light " + templateAmmoString + ammoManager.lightAmmo;
            ammoTexts[1].text = "Shotgun " + templateAmmoString + ammoManager.shotgunAmmo;
            ammoTexts[2].text = "Medium " + templateAmmoString + ammoManager.mediumAmmo;
            ammoTexts[3].text = "Heavy " + templateAmmoString + ammoManager.heavyAmmo;
            ammoTexts[4].text = "Misc " + templateAmmoString + ammoManager.miscAmmo;
        }
    }

    public void SetSelectedPrefab(PhotonView newPrefab) {
        selectedPrefab = newPrefab;
    }

    public void SpawnPrefab() {
        PhotonNetwork.Instantiate(selectedPrefab.name, bulletSpawn.position, Quaternion.identity);
    }
}
