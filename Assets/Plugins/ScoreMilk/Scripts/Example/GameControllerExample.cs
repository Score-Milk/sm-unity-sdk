using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScoreMilk.Example{
// An example of how a game controller should work in tandem to ScoreMilk
public class GameControllerExample : MonoBehaviour
{
# region Subscribing events
    // Subscribes events on start of Application
    private void OnEnable() {
        ScoreMilk.Connection.OnReceivedMatchmakingStart += OnMatchmaking;
        ScoreMilk.Connection.OnReceivedToPractice += OnReceivedPractice;
        ScoreMilk.Connection.OnReceivedCancelMatch += OnMatchNotFound;
        ScoreMilk.Connection.OnReceivedStart += OnReceivedStart;
    }
    // Unsubscribes events on end of Application
    private void OnDisable() {
        ScoreMilk.Connection.OnReceivedMatchmakingStart -= OnMatchmaking;
        ScoreMilk.Connection.OnReceivedToPractice -= OnReceivedPractice;
        ScoreMilk.Connection.OnReceivedCancelMatch -= OnMatchNotFound;
        ScoreMilk.Connection.OnReceivedStart -= OnReceivedStart;
    }
# endregion

# region Emissions

    // Call this to register points during online match
    public void EmitAddScore(int points){
        ScoreMilk.Connection.EmitAddScore(points);
    }
    // Call this to register final pontuation at the end of match
    public void EmitGameOver(int points){
        ScoreMilk.Connection.EmitGameOver(points);
    }
    // Call this to give OK message to server to start match
    public void EmitReady(){
        ScoreMilk.Connection.EmitReady();
    }
#endregion

# region Events

    /// Called when player pressed "play" button
    /// Probably put here functions to change game scene to online game scene
    private void OnMatchmaking(object sender, EventArgs e){
        print("Matchmaking stuff here");
        var manager = GameObject.FindWithTag("GameManager");
        if (manager != null){
            manager.SendMessage("StartMatch");
        }
    }
    /// Called when player pressed "practice" button
    /// Probably put here functions to change game scene to practice game scene
    private void OnReceivedPractice(object sender, EventArgs e){
        print("OnReceivedPractice stuff here");
        SceneManager.LoadScene("PracticeGame");
    }
    /// Called when match has not started properly
    /// Probably put here function to return to title scene
    private void OnMatchNotFound(object sender, EventArgs e){
        print("MatchNotFound stuff here");
        SceneManager.LoadScene("Title");
    }
    /// Called when match has started properly
    /// Probably put here function to start online game
    private void OnReceivedStart(object sender, EventArgs e){
        print("ReceivedStart stuff here");
        SceneManager.LoadScene("OnlineGame");
    }
#endregion
}

}