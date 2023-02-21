using System;
using UnityEngine;
using Newtonsoft.Json;

using OPS.AntiCheat.Field;

namespace ScoreMilk{
    public class NetworkManager : Singleton<NetworkManager>
    {
        private ProtectedString wallet_id = string.Empty;
        private ProtectedString match_room_id = string.Empty;
        private ProtectedString address = string.Empty;

        // used so GameOver isn't sent too much
        private bool isGameOver = false;

        // Initialization
        protected override void StartUp()
        {
            EmitGameLoaded();
        }

        private void EmitGameLoaded()
        {
            Debug.Log("Emitindo gameLoaded");
            Application.ExternalCall("gameLoaded");
        }
        
        /// <summary>
        /// Received message that indicates game can properly start
        /// NOT necessary to call. Use event instead.
        /// </summary>
        void canStartGame(string jsonData) 
        {
            var userData = JsonConvert.DeserializeObject<UserData>(jsonData);
            Connection.Instance.canStartGameCall(userData);
            Debug.Log("Recebido canStartGame");
        }
        /// <summary>
        /// Received message that indicates game should go to practice mode
        /// NOT necessary to call. Use event instead.
        /// </summary>
        void toPracticeGame(){
            Connection.Instance.toPracticeGameCall();
            Debug.Log("Recebido toPractice");
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
            data.address = NetworkManager.Instance.address;
            data.player_id = NetworkManager.Instance.address;

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
            data.address = NetworkManager.Instance.address;
            data.player_id = NetworkManager.Instance.address;

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
            data.address = NetworkManager.Instance.address;
            data.player_id = NetworkManager.Instance.address;
            data.points = "";

            //Application.ExternalCall("socket.emit", "EMIT_READY", new JSONObject(data));
            WebConnection.Instance.Emit("/matches/player-loaded-game", data);
        }
        /// <summary>
        /// Called when match is cancelled
        /// NOT necessary to call.
        /// </summary>
        public void cancelMatch(string json)
        {
            Debug.Log("recebido cancelMatch");
            ScoreMilk.Connection.Instance.cancelMatchCall(json);
        }

        /// <summary>
        /// Called when match is cancelled 
        /// NOT necessary to call in Unity
        /// </summary>
        public void matchNotFound(string json)
        {
            Debug.Log("recebido matchNotFound");
            ScoreMilk.Connection.Instance.matchNotFoundCall(json);
        }

        /// <summary>
        /// Called when matchmaking starts
        /// NOT necessary to call in Unity
        /// </summary>
        void startMatchmaking(string json){
            Debug.Log("recebido startMatchmaking");
            parseJson(json);
            ScoreMilk.Connection.Instance.startMatchmakingCall(json);
        }

        /// <summary>
        /// Called when wallet connects 
        /// NOT necessary to call in Unity
        /// </summary>
        public void walletConnected(string wallet)
        {
            Debug.Log("recebido walletConnected");
            wallet_id = wallet;
            ScoreMilk.Connection.Instance.walletConnectedCall(wallet);
        }

        /// <summary>
        /// Called when wallet disconnects
        /// NOT necessary to call in Unity
        /// </summary>
        public void walletDisconnected()
        {
            wallet_id = "";
            Debug.Log("recebido matchNotFound");
            ScoreMilk.Connection.Instance.walletDisconnectedCall();
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
        void SetEnvs(string jsonData)
        {
            var envs = JsonConvert.DeserializeObject<Envs>(jsonData);
            WebConnection.Instance.SetUrl(envs.API_URL);
        }
        void parseJson(string json){
            Debug.Log(json);
            try
            {
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                NetworkManager.Instance.match_room_id = playerData.match_room_id;
                NetworkManager.Instance.address = playerData.address;
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
    
    class UserData {
        public string name;
        public string username;
        public string avatar;
        public string bio;
    }
    
    class Envs {
        public string API_URL;
    }
}