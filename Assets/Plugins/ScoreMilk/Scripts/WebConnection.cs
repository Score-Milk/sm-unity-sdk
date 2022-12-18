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

    public void Emit(string name, PlayerData data){
        StartCoroutine(GetText(url + "/" + name, data));
    }
    IEnumerator GetText(string names, PlayerData data) {
        UnityWebRequest www = UnityWebRequest.Put(names, JsonUtility.ToJson(data));
        www.SetRequestHeader( "Content-type", "application/json");
        www.SetRequestHeader( "Authorization", data.player_id);
        
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text //temp
            Debug.Log("got answer");
            Debug.Log(www.downloadHandler.text); 
        }
    }


}
}