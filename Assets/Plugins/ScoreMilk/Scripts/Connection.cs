using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScoreMilk{
public class Connection : Singleton<Connection>
{
        /// <summary>
        /// Received event that player pressed "play" button
        /// Game should go to real match scene and wait for start.
        /// This is before the players accept their transactions.
        /// Match starts after both players have accepted transaction and sent "EmitReady()"
        /// </summary>
        public static event EventHandler OnReceivedMatchmakingStart;
        /// <summary>
        /// Received event that player pressed "practice" button
        /// Game should go to a practice scene. NOT title screen.
        /// </summary>
        public static event EventHandler OnReceivedToPractice;
        /// <summary>
        /// Match was cancelled for any reason.
        /// Game should go back previous scene or title.
        /// </summary>
        public static event EventHandler OnReceivedCancelMatch;
        /// <summary>
        /// Received event both players are ready and accepted the required crypto transaction. 
        /// Game should continue and start match.
        /// </summary>
        public static event EventHandler<StartRealGameData> OnReceivedStart;
        /// <summary>
        /// Received event that wallet connected
        /// Receives wallet id as string
        /// </summary>
        public static event EventHandler<string> OnReceivedWalletConnected;
        /// <summary>
        /// Received event that wallet disconnected
        /// </summary>
        public static event EventHandler OnReceivedWalletDisconnected;

        /// <summary>
        /// Called when wallet connects
        /// NOT necessary to call in Unity
        /// </summary>
        public void walletConnectedCall(string walletAddress)
        {
            OnReceivedWalletConnected?.Invoke(this, walletAddress);
        }
        /// <summary>
        /// Called when wallet disconnects
        /// NOT necessary to call in Unity
        /// </summary>
        public void walletDisconnectedCall()
        {
            OnReceivedWalletDisconnected?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Received message that indicates game should go to practice mode
        /// NOT necessary to call. Use event instead.
        /// </summary>
        public void startPracticeGameCall(){
            OnReceivedToPractice?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Called when matchmaking starts
        /// NOT necessary to call in Unity
        /// </summary>
        public void getReadyCall()
        {
            OnReceivedMatchmakingStart?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Received message that indicates game can properly start
        /// NOT necessary to call. Use event instead.
        /// </summary>
        public void startRealGameCall(StartRealGameData userData) 
        {
            OnReceivedStart?.Invoke(this, userData);
        }
        /// <summary>
        /// Called when match is cancelled
        /// NOT necessary to call. Use event instead.
        /// </summary>
        public void quitToMenuCall()
        {
            OnReceivedCancelMatch?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Emits message to server that says player got points during match
        /// </summary>
        public static void EmitAddScore(int score)
        {
            NetworkManager.Instance.EmitAddScore(score);
        }
        /// <summary>
        /// Emits message to server that says the match ended. Accumulated points must be the same as added points
        /// points: total points acquired during match
        /// </summary>
        public static void EmitGameOver(int points)
        {
            NetworkManager.Instance.EmitGameOver(points);
        }
        /// <summary>
        /// Emits message to server that says player is ready to start match
        /// </summary>
        public static void EmitReady()
        {
            NetworkManager.Instance.EmitReady();
        }
}
}