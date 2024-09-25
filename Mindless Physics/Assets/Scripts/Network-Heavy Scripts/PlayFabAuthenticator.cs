using PlayFab;
using PlayFab.ClientModels;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using System;
using System.Collections;
using TMPro;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;

namespace NetworkHeavy
{
    public class PlayFabAuthenticator : MonoBehaviour {
    public TMP_Text photonID_Text;
    private string _playFabPlayerIdCache;
    private bool isBanned = false;
    public GameObject[] gameObjectsToDisableWhenBannedOrNotLoggedIn;

    //Run the entire thing on awake
    public IEnumerator Start() {
        AuthenticateWithPlayFab();
        yield return new WaitForSeconds(1.5f);
        // See if player is logged in and banned!
        photonID_Text.text = _playFabPlayerIdCache;
        if(!PlayFabClientAPI.IsClientLoggedIn() || isBanned){
            foreach(GameObject go in gameObjectsToDisableWhenBannedOrNotLoggedIn){
                go.SetActive(false);
            }
        }
    }

    public void CopyRoomCodeToClipboard() {
        TextEditor textEditor = new TextEditor();
        textEditor.text = _playFabPlayerIdCache;
        textEditor.SelectAll();
        textEditor.Copy();
    }

    /*
     * Step 1
     * We authenticate a current PlayFab user normally.
     * In this case we use the LoginWithCustomID API call for simplicity.
     * You can absolutely use any Login method you want.
     * We use PlayFabSettings.DeviceUniqueIdentifier as our custom ID.
     * We pass RequestPhotonToken as a callback to be our next step, if
     * authentication was successful.
     */
    private void AuthenticateWithPlayFab(){
        LogMessage("PlayFab authenticating using Custom ID...");

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CreateAccount = true,
            CustomId = PlayFabSettings.DeviceUniqueIdentifier
        }, RequestPhotonToken, OnPlayFabError);
    }

    /*
    * Step 2
    * We request a Photon authentication token from PlayFab.
    * This is a crucial step, because Photon uses different authentication tokens
    * than PlayFab. Thus, you cannot directly use PlayFab SessionTicket and
    * you need to explicitly request a token. This API call requires you to
    * pass a Photon App ID. The App ID may be hard coded, but in this example,
    * we are accessing it using convenient static field on PhotonNetwork class.
    * We pass in AuthenticateWithPhoton as a callback to be our next step, if
    * we have acquired the token successfully.
    */
    private void RequestPhotonToken(LoginResult obj) {
        LogMessage("PlayFab authenticated. Requesting photon token...");

        //We can player PlayFabId. This will come in handy during next step
        _playFabPlayerIdCache = obj.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
        }, AuthenticateWithPhoton, OnPlayFabError);
    }

    /*
     * Step 3
     * This is the final and the simplest step. We create a new AuthenticationValues instance.
     * This class describes how to authenticate a player inside the Photon environment.
     */
    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj) {
        LogMessage("Photon token acquired: " + obj.PhotonCustomAuthenticationToken + "  Authentication complete.");

        //We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
        var customAuth = new Photon.Realtime.AuthenticationValues { AuthType = CustomAuthenticationType.Custom };

        //We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID (!) and not username.
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);    // expected by PlayFab custom auth service

        //We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issues to your during previous step.
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        //We finally tell Photon to use this authentication parameters throughout the entire application.
        PhotonNetwork.AuthValues = customAuth;
    }

    private void OnPlayFabError(PlayFabError obj) {
        LogMessage(obj.GenerateErrorReport());
    }

    public void LogMessage(string message) {
        Debug.Log("PlayFab + Photon Example: " + message);
        if(message.Contains("banned", StringComparison.OrdinalIgnoreCase)){
            isBanned = true;
            Debug.Log("Yep, you're banned!");
        }
    }
    public void ChangeEmail(string email) {
        var request = new AddOrUpdateContactEmailRequest{
            EmailAddress = email,
        };
        PlayFabClientAPI.AddOrUpdateContactEmail(request, OnEmailSuccess, OnEmailError);
    }

    private void OnEmailSuccess(AddOrUpdateContactEmailResult result)
    {
        Debug.Log(result);
    }

    private void OnEmailError(PlayFabError error)
    {
        Debug.Log(error);
    }
}
}
