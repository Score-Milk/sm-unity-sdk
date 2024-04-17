/*
    Bridge communication module
    These are internal methods of the Score Milk SDK.
*/

using System;
using UnityEngine;

using OPS.AntiCheat.Field;

namespace ScoreMilk{
    public class NetworkManager : Singleton<NetworkManager>
    {
        private ProtectedString matchId = string.Empty;
        private ProtectedString userId = string.Empty;

        // Initialization
        protected override void StartUp()
        {
            EmitGameLoaded();
        }

        static void EmitGameLoaded()
        {
            Application.ExternalCall("gameLoaded");
        }

        /// <summary>
        /// Called after the frontend receives the gameLoaded call 
        /// </summary>
        void init(string jsonData)
        {
            InitData data = JsonUtility.FromJson<InitData>(jsonData);
            WebConnection.Instance.SetUrl(data.API_URL);
            ScoreMilk.Connection.Instance.initCall(data);
        }

        /// <summary>
        /// Called when the user logs in
        /// </summary>
        void login(string jsonData)
        {
            LoginData data = JsonUtility.FromJson<LoginData>(jsonData);
            NetworkManager.Instance.userId = data.userId;
            ScoreMilk.Connection.Instance.loginCall(data);
        }

        /// <summary>
        /// Called when the user logs out
        /// </summary>
        public void logout()
        {
            NetworkManager.Instance.userId = null;
            ScoreMilk.Connection.Instance.logoutCall();
        }

        /// <summary>
        /// Received when the user clicks the practice button
        /// </summary>
        void startPracticeGame(){
            Connection.Instance.startPracticeGameCall();
        }

        /// <summary>
        /// Received when the user finds an opponent
        /// </summary>
        void getReady(string json){
            GetReadyData data = new GetReadyData();
            try
            {
                data = JsonUtility.FromJson<GetReadyData>(json);
                NetworkManager.Instance.matchId = data.matchId;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            ScoreMilk.Connection.Instance.getReadyCall(data);
        }

        /// <summary>
        /// Received after both users pay the wage
        /// </summary>
        void startRealGame() 
        {
            Connection.Instance.startRealGameCall();
        }

        /// <summary>
        /// Called when match is cancelled
        /// </summary>
        void quitToMenu()
        {
            ScoreMilk.Connection.Instance.quitToMenuCall();
        }

        /// <summary>
        /// Tells the server that the game is ready to start
        /// TODO this method should be in the Connection class
        /// </summary>
        public void EmitReady()
        {
            HttpRequestData data = new HttpRequestData();
            data.match_room_id = NetworkManager.Instance.matchId;
            data.player_id = NetworkManager.Instance.userId;

            WebConnection.Instance.Emit("/matches/player-loaded-game", data);
        }

        /// <summary>
        /// Tells the backend that the match is finished
        /// points: total points acquired during match
        /// TODO this method should be in the Connection class
        /// </summary>
        public void EmitGameOver(int points)
        {
            HttpRequestData data = new HttpRequestData();
            data.match_room_id = NetworkManager.Instance.matchId;
            data.player_id = NetworkManager.Instance.userId;

            data.points = points.ToString();

            WebConnection.Instance.Emit("/matches/player-finished-game", data);
        }

        /// <summary>
        /// Tells the backend the user received points
        /// TODO this method should be in the Connection class
        /// </summary>
        public void EmitAddScore(int score)
        {
            HttpRequestData data = new HttpRequestData();
            data.match_room_id = NetworkManager.Instance.matchId;
            data.player_id = NetworkManager.Instance.userId;
            data.points = score.ToString();

            WebConnection.Instance.Emit("/matches/player-score-game", data);
        }
    }

    // We could remove this class and use the string
    // But this class will have more utility in the future
    // We will pass to the game a user-match token for authentication
    [Serializable]
    public class GetReadyData {
        public string matchId;
    }

    public class HttpRequestData {
        public string match_room_id;
        public string player_id;
        public string points;
    }

    public class LoginData {
        public string walletAddress;
        public string userId;
        public string name;
        public string username;
        public string avatar;
        public string bio;
    }

    public class InitData {
        public string ENVIRONMENT;
        public string API_URL;
    }
}