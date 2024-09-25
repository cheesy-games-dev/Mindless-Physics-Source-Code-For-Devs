using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptStopCloningManagers : MonoBehaviour
{
    public PlayerColorScript playerColorScript;
    public DevCanvas devCanvas;

    void Awake()
    {
        FindObjects();
    }

    void LateUpdate(){
        FindObjects();
    }

    void FindObjects(){
        playerColorScript = FindAnyObjectByType<PlayerColorScript>();
        devCanvas = FindAnyObjectByType<DevCanvas>();
        PlayerColorScript[] playerColorScripts = FindObjectsOfType<PlayerColorScript>();
        foreach(var pCS in playerColorScripts){
            if(!pCS.gameObject.activeSelf){
                Destroy(pCS.gameObject);
            }
        }
        DevCanvas[] devCanvases = FindObjectsOfType<DevCanvas>();
        foreach(var dC in devCanvases){
            if(!dC.gameObject.activeSelf){
                Destroy(dC.gameObject);
            }
        }
    }
}
