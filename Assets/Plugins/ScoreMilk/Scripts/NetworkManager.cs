using System;
using UnityEngine;
using Newtonsoft.Json;

using OPS.AntiCheat.Field;

namespace ScoreMilk{
    public class NetworkManager : Singleton<NetworkManager>
    {
        private ProtectedString match_room_id = string.Empty;
        private ProtectedString player_id = string.Empty;

        // Initialization
        protected override void StartUp()
        {
            EmitGameLoaded();
        }
        private void EmitGameLoaded()
        {
            Application.ExternalCall("gameLoaded");
        }
        void setUrl(string jsonData)
        {
            Envs envs = JsonConvert.DeserializeObject<Envs>(jsonData);
            WebConnection.Instance.SetUrl(envs.API_URL);
        }
        /// <summary>
        /// Called when wallet connects 
        /// NOT necessary to call in Unity
        /// </summary>
        public void walletConnected(string jsonData)
        {
            WalletConnectedData data = JsonConvert.DeserializeObject<WalletConnectedData>(jsonData);
            ScoreMilk.Connection.Instance.walletConnectedCall(data.walletAddress);
        }

        /// <summary>
        /// Called when wallet disconnects
        /// NOT necessary to call in Unity
        /// </summary>
        public void walletDisconnected()
        {
            ScoreMilk.Connection.Instance.walletDisconnectedCall();
        }
        /// <summary>
        /// Received message that indicates game should go to practice mode
        /// NOT necessary to call. Use event instead.
        /// </summary>
        void startPracticeGame(){
            Connection.Instance.startPracticeGameCall();
        }
        /// <summary>
        /// Received before a real match starts
        /// NOT necessary to call in Unity
        /// </summary>
        void getReady(string json){
            parseJson(json);
            ScoreMilk.Connection.Instance.getReadyCall();
        }
        /// <summary>
        /// Received message that indicates game can properly start
        /// NOT necessary to call. Use event instead.
        /// </summary>
        void startRealGame(string jsonData) 
        {
            StartRealGameData data = JsonConvert.DeserializeObject<StartRealGameData>(jsonData);
            Connection.Instance.startRealGameCall(data);
        }
        /// <summary>
        /// Called when match is cancelled
        /// NOT necessary to call.
        /// </summary>
        public void quitToMenu()
        {
            ScoreMilk.Connection.Instance.quitToMenuCall();
        }
        /// <summary>
        /// Emits message to server that says the match ended. Accumulated points must be the same as added points
        /// points: total points acquired during match
        /// </summary>
        public void EmitGameOver(int points)
        {
            Debug.Log("GAME OVER");

            PlayerData data = new PlayerData();
            data.match_room_id = NetworkManager.Instance.match_room_id;
            data.player_id = NetworkManager.Instance.player_id;

            data.points = points.ToString();

            WebConnection.Instance.Emit("/matches/player-finished-game", data);
        }

        /// <summary>
        /// Emits message to server that says player got points during match
        /// </summary>
        public void EmitAddScore(int score)
        {
            PlayerData data = new PlayerData();
            data.match_room_id = NetworkManager.Instance.match_room_id;
            data.player_id = NetworkManager.Instance.player_id;

            data.points = score.ToString();
            WebConnection.Instance.Emit("/matches/player-score-game", data);
        }
        
        /// <summary>
        /// Emits message to server that says player is ready to start match
        /// </summary>
        public void EmitReady()
        {
            Debug.Log("Emitindo EmitReady");
            PlayerData data = new PlayerData();
            data.match_room_id = NetworkManager.Instance.match_room_id;
            data.player_id = NetworkManager.Instance.player_id;
            data.points = "";

            //Application.ExternalCall("socket.emit", "EMIT_READY", new JSONObject(data));
            WebConnection.Instance.Emit("/matches/player-loaded-game", data);
        }
        void OnApplicationQuit()
        {
            OnlineApplicationQuit();
        }
        void OnlineApplicationQuit()
        {
            EmitGameOver(0);
            Debug.Log("Application ending after " + Time.time + " seconds");
        }
        void parseJson(string json){
            Debug.Log(json);
            try
            {
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                NetworkManager.Instance.match_room_id = playerData.match_room_id;
                NetworkManager.Instance.player_id = playerData.player_id;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }
    [Serializable]
    public class PlayerData {
        public string match_room_id;
        public string player_id;
        public string address;
        public string points;
    }
    
    [Serializable]
    public class WalletData {
        public string match_room_id;
        public string player_id;
        public string address;
        public string points;
    }
    
    public class StartRealGameData {
        public string name;
        public string username;
        public string avatar;
        public string bio;
    }

    class WalletConnectedData {
        public string walletAddress;
    }
    
    class Envs {
        public string API_URL;
    }
}