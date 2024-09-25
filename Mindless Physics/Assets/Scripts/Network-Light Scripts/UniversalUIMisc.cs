using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalUIMisc : MonoBehaviour
{
    public void OpenLink(string linkString){
        Application.OpenURL(linkString);
    }
}
