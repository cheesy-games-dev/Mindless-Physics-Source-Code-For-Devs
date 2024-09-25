using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DevCanvas : MonoBehaviour
{
    public GameObject itemManager;

    void Start(){      
        AttemptStopCloningManagers attemptStopCloningManagers = FindAnyObjectByType<AttemptStopCloningManagers>();
        if(attemptStopCloningManagers.devCanvas != this){
            gameObject.SetActive(false);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update(){      
        if(SceneManager.GetActiveScene().name != "MainMenu"){
            itemManager.SetActive(false);
            return;
        }
        if(Keyboard.current.f3Key.wasPressedThisFrame){
            itemManager.SetActive(!itemManager.activeSelf);
        }
    }
}
