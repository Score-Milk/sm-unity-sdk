/* 
    Backend communication module
    These methods send data to the Score Milk Backend
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

using OPS.AntiCheat.Field;

namespace ScoreMilk {
public class BackendCommunication : Singleton<BackendCommunication>
{
    [Tooltip("URL to test in unity editor. Automatically set in web environment")]
    [SerializeField] private string url;
    private ProtectedString matchId = string.Empty;
    private ProtectedString userId = string.Empty;

    // Initialization
    protected override void StartUp()
    {
        base.StartUp();
    }

    public void SetUrl(string _url) {
        url = _url;
    }

    public void SetUserId(string _userId) {
        userId = _userId;
    }

    public void SetMatchId(string _matchId) {
        matchId = _matchId;
    }

    IEnumerator SendHttpRequest(string path, HttpRequestData data) {
        data.match_room_id = Instance.matchId;
        data.player_id = Instance.userId;

        EncryptedHttpRequestData encryptedData = new EncryptedHttpRequestData();
        encryptedData.data = ScoreMilk.Encryption.Encrypt(JsonUtility.ToJson(data));

        UnityWebRequest www = UnityWebRequest.Put(url + path, JsonUtility.ToJson(encryptedData));
        www.SetRequestHeader( "Content-type", "application/json");
        www.SetRequestHeader( "Authorization", data.player_id);
        www.SetRequestHeader("SM-APP", "SDK-0.2.4+");
        
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(System.Text.Encoding.Default.GetString(www.downloadHandler.data));
            Debug.Log(www.error);
        }
    }

        public void EmitLoaded()
        {
            StartCoroutine(SendHttpRequest("/matches/player-loaded-game", new HttpRequestData()));
        }

        public void EmitAddScore(int score)
        {
            HttpRequestData data = new HttpRequestData();
            data.points = score.ToString();

            StartCoroutine(SendHttpRequest("/matches/player-score-game", data));
        }

        public void EmitGameOver(int points)
        {
            HttpRequestData data = new HttpRequestData();
            data.points = points.ToString();

            StartCoroutine(SendHttpRequest("/matches/player-finished-game", data));
        }
    }
}

[Serializable]
public class EncryptedHttpRequestData {
    public string data;
}
