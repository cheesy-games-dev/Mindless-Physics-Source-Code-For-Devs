using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkHeavy{
public class MapManager : MonoBehaviour
{
    public string mapName = "City";
    public GameManager gameManager;
    public Transform mainObjectsParent;
    public Transform[] spawnPoints;
    public Transform[] zombieSpawnPoints;
    public GameObject gameObjectsDeathmatch;
    public GameObject gameObjectsZombies;
    public GameObject gameObjectsBattleRoyale;

    void LateUpdate(){
        if(gameManager != null){
            gameObjectsDeathmatch.SetActive(gameManager.gameMode == GameManager.GameMode.Deathmatch);
            gameObjectsZombies.SetActive(gameManager.gameMode == GameManager.GameMode.Zombies);
            gameObjectsBattleRoyale.SetActive(gameManager.gameMode == GameManager.GameMode.BattleRoyale);
        }
        else{
            //gameManager = FindAnyObjectByType<GameManager>();
        }
    }
}
}
