/*
    Bridge communication module
    These are internal methods of the Score Milk SDK.
*/

using System;
using UnityEngine;

namespace ScoreMilk{
    public class BridgeCommunication : Singleton<BridgeCommunication>
    {
        // Initialization
        protected override void StartUp()
        {
            emitGameLoaded();
        }

        // Tells the frontend that the game is loaded and sends the SDK version
        static void emitGameLoaded()
        {
            PostBridgeMessageData data = new PostBridgeMessageData();
            data.message = "gameLoaded";
            data.version = "0.2.5";
            data.platform = "Unity";

            Application.ExternalCall("postBridgeMessage", JsonUtility.ToJson(data));		
        }

        // Tells the frontend that the game is idle and can start matches
        public static void emitIdle()
        {
            PostBridgeMessageData data = new PostBridgeMessageData();
            data.message = "stateIdle";

            Application.ExternalCall("postBridgeMessage", JsonUtility.ToJson(data));	
        }

        // Tells the frontend that the game is in a practice match
        public static void emitPracticing()
        {
            PostBridgeMessageData data = new PostBridgeMessageData();
            data.message = "statePractice";

            Application.ExternalCall("postBridgeMessage", JsonUtility.ToJson(data));		
        }

        /// <summary>
        /// Called after the frontend receives the gameLoaded call 
        /// </summary>
        void init(string jsonData)
        {
            InitData data = JsonUtility.FromJson<InitData>(jsonData);
            BackendCommunication.Instance.SetUrl(data.API_URL);
            ScoreMilk.GameInterface.Instance.initCall(data);
        }

        /// <summary>
        /// Called when the user logs in
        /// </summary>
        void login(string jsonData)
        {
            LoginData data = JsonUtility.FromJson<LoginData>(jsonData);
            BackendCommunication.Instance.SetUserId(data.userId);
            ScoreMilk.GameInterface.Instance.loginCall(data);
        }

        /// <summary>
        /// Called when the user logs out
        /// </summary>
        void logout()
        {
            BackendCommunication.Instance.SetUserId(null);
            ScoreMilk.GameInterface.Instance.logoutCall();
        }

        /// <summary>
        /// Received when the user clicks the practice button
        /// </summary>
        void startPracticeGame(){
            GameInterface.Instance.startPracticeGameCall();
        }

        /// <summary>
        /// Received when the user finds an opponent
        /// </summary>
        void getReady(string json){
            GetReadyData data = new GetReadyData();
            try
            {
                data = JsonUtility.FromJson<GetReadyData>(json);
                BackendCommunication.Instance.SetUserId(data.matchId);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            ScoreMilk.GameInterface.Instance.getReadyCall(data);
        }

        /// <summary>
        /// Received after both users pay the wage
        /// </summary>
        void startRealGame() 
        {
            GameInterface.Instance.startRealGameCall();
        }

        /// <summary>
        /// Called when match is cancelled
        /// </summary>
        void quitToMenu()
        {
            ScoreMilk.GameInterface.Instance.quitToMenuCall();
        }
    }

    // We could remove this class and use the string
    // But this class will have more utility in the future
    // We will pass to the game a user-match token for authentication
    [Serializable]
    public class GetReadyData {
        public string matchId;
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

    class PostBridgeMessageData {
        public string message;
        public string version;
        public string platform;
    }
}
