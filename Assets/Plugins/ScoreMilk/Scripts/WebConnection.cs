/* 
    Backend communication module
    These methods send data to the Score Milk Backend
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace ScoreMilk {
public class WebConnection : Singleton<WebConnection>
{
    [Tooltip("URL to test in unity editor. Automatically set in web environment")]
    [SerializeField] private string url;

    // Initialization
    protected override void StartUp()
    {
        base.StartUp();
    }

    public void SetUrl(string _url) {
        url = _url;
    }

    public void Emit(string name, HttpRequestData data){
        StartCoroutine(SendHttpRequest(url + name, data));
    }

    IEnumerator SendHttpRequest(string names, HttpRequestData data) {
        EncryptedHttpRequestData encryptedData = new EncryptedHttpRequestData();
        encryptedData.data = ScoreMilk.Encryption.Encrypt(JsonUtility.ToJson(data));

        UnityWebRequest www = UnityWebRequest.Put(names, JsonUtility.ToJson(encryptedData));
        www.SetRequestHeader( "Content-type", "application/json");
        www.SetRequestHeader( "Authorization", data.player_id);
        www.SetRequestHeader("SM-APP", "SDK-0.2.4+");
        
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
    }
}
}

[Serializable]
public class EncryptedHttpRequestData {
    public string data;
}
