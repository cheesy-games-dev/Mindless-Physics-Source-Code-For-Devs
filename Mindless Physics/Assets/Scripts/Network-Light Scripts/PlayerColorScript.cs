using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColorScript : MonoBehaviour
{
    public Color universalPlayerColor;
    public Vector3 universalPlayerColorDebug;
    public float multiplier = 0.1f;
    private string selectedColor;
    public Graphic[] graphics;
    public TMP_Text redText;
    public TMP_Text greenText;
    public TMP_Text blueText;
    void Start(){
        AttemptStopCloningManagers attemptStopCloningManagers = FindAnyObjectByType<AttemptStopCloningManagers>();
        if(attemptStopCloningManagers.playerColorScript != this){
            gameObject.SetActive(false);
        }
        transform.SetParent(null, false);
        DontDestroyOnLoad(this.gameObject);
        if(!(PlayerPrefs.HasKey("r") || PlayerPrefs.HasKey("g") || PlayerPrefs.HasKey("b"))){
            PlayerPrefs.SetInt("r", Random.Range(0, 10));
            PlayerPrefs.SetInt("g", Random.Range(0, 10));
            PlayerPrefs.SetInt("b", Random.Range(0, 10));
        }
    }
    void Update()
    {
        universalPlayerColor.r = PlayerPrefs.GetInt("r") * (multiplier);
        universalPlayerColor.g = PlayerPrefs.GetInt("g") * (multiplier);
        universalPlayerColor.b = PlayerPrefs.GetInt("b") * (multiplier);
        universalPlayerColor.a = 255;
        universalPlayerColorDebug = new Vector3(universalPlayerColor.r, universalPlayerColor.g, universalPlayerColor.b);
        redText.text = "Red: " + universalPlayerColor.r.ToString();
        redText.color = new Color(universalPlayerColor.r + 5, 0 , 0);
        greenText.text = "Green: " + universalPlayerColor.g.ToString();
        greenText.color = new Color(0, universalPlayerColor.g + 5 , 0);
        blueText.text = "Blue: " + universalPlayerColor.b.ToString();
        blueText.color = new Color(0, 0 , universalPlayerColor.b + 5);
        foreach(Graphic g in graphics){
            g.color = universalPlayerColor;
        }
    }
    
    public void ChangeSelectedColor(string specificColor){
        selectedColor = specificColor;
    }

    public void ChangeColorValue(int addedNumber){
        if((PlayerPrefs.GetInt(selectedColor) >= 10 && addedNumber > 0)|| (PlayerPrefs.GetInt(selectedColor) <= 0 && addedNumber < 0)){
            return;
        }
        PlayerPrefs.SetInt(selectedColor, PlayerPrefs.GetInt(selectedColor) + addedNumber);
        PlayerPrefs.SetInt(selectedColor, Mathf.Clamp(PlayerPrefs.GetInt(selectedColor), 0, 10));
        Debug.Log(selectedColor + ": " + PlayerPrefs.GetInt(selectedColor));
    }
}
