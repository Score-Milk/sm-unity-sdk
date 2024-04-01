
/* 
    These are functions that the game should call
    To add score or tell the frontend that the game is ready
    Or that the game finished
*/
    
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScoreMilk{
public class Connection : Singleton<Connection>
{
    // Events
    // The game should subscribe to them
        /// <summary>
        /// Received event that player pressed "play" button
        /// Game should go to real match scene and wait for start.
        /// This is before the players accept their transactions.
        /// Match starts after both players have accepted transaction and sent "EmitReady()"
        /// </summary>
        public static event EventHandler<GetReadyData> OnReceivedGetReady;
        /// <summary>
        /// Received event that player pressed "practice" button
        /// Game should start a practice game.
        /// </summary>
        public static event EventHandler OnReceivedStartPracticeGame;
        /// <summary>
        /// Match was cancelled.
        /// Game should go back to the menu.
        /// </summary>
        public static event EventHandler OnReceivedQuitToMenu;
        /// <summary>
        /// Both players made the bets and are ready to play. 
        /// Game should start a real game.
        /// </summary>
        public static event EventHandler OnReceivedStartRealGame;
        /// <summary>
        /// Received event that wallet was connected
        /// Receives wallet id as string
        /// </summary>
        public static event EventHandler<LoginData> OnReceivedLogin;
        /// <summary>
        /// Received event that wallet was disconnected
        /// </summary>
        public static event EventHandler OnReceivedLogout;


    // Emit functions
    // The game should call them

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

    // Internal functions
    // The game should ignore these
        /// <summary>
        /// Called when wallet connects
        /// This is an internal function, the game should ignore it
        /// </summary>
        public void loginCall(LoginData data)
        {
            OnReceivedLogin?.Invoke(this, data);
        }
        /// <summary>
        /// Called when wallet disconnects
        /// This is an internal function, the game should ignore it
        /// </summary>
        public void logoutCall()
        {
            OnReceivedLogout?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Received message that indicates game should go to practice mode
        /// This is an internal function, the game should ignore it
        /// </summary>
        public void startPracticeGameCall(){
            OnReceivedStartPracticeGame?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Called when matchmaking starts
        /// This is an internal function, the game should ignore it
        /// </summary>
        public void getReadyCall(GetReadyData data)
        {
            OnReceivedGetReady?.Invoke(this, data);
        }
        /// <summary>
        /// Received message that indicates game can properly start
        /// This is an internal function, the game should ignore it
        /// </summary>
        public void startRealGameCall() 
        {
            OnReceivedStartRealGame?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Called when match is cancelled
        /// This is an internal function, the game should ignore it
        /// </summary>
        public void quitToMenuCall()
        {
            OnReceivedQuitToMenu?.Invoke(this, EventArgs.Empty);
        }
}
}