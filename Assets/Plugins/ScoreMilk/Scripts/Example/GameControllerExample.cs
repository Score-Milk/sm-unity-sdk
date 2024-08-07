using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScoreMilk.Example{
// An example of how a game controller should work in tandem to ScoreMilk
public class GameControllerExample : MonoBehaviour
{
# region Subscribing events
    // Subscribes to events on start of Application
    private void OnEnable() {
        ScoreMilk.GameInterface.OnGetReady += GetReady;
        ScoreMilk.GameInterface.OnStartPracticeGame += StartPracticeGame;
        ScoreMilk.GameInterface.OnQuitToMenu += QuitToMenu;
        ScoreMilk.GameInterface.OnStartRealGame += StartRealGame;
        ScoreMilk.GameInterface.OnInit += Init;
        ScoreMilk.GameInterface.OnLogin += Login;
        ScoreMilk.GameInterface.OnLogout += Logout;
    }
    // Unsubscribes to events on end of Application
    private void OnDisable() {
        ScoreMilk.GameInterface.OnGetReady -= GetReady;
        ScoreMilk.GameInterface.OnStartPracticeGame -= StartPracticeGame;
        ScoreMilk.GameInterface.OnQuitToMenu -= QuitToMenu;
        ScoreMilk.GameInterface.OnStartRealGame -= StartRealGame;
        ScoreMilk.GameInterface.OnInit -= Init;
        ScoreMilk.GameInterface.OnLogin -= Login;
        ScoreMilk.GameInterface.OnLogout -= Logout;
    }
# endregion

# region Emissions

    // Call this to register points during online match
    public void EmitAddScore(int points){
        ScoreMilk.GameInterface.EmitAddScore(points);
    }
    // Call this to register final pontuation at the end of match
    public void EmitGameOver(int points){
        ScoreMilk.GameInterface.EmitGameOver(points);
    }
    // Call this to give OK message to server to start match
    public void EmitReady(){
        ScoreMilk.GameInterface.EmitReady();
    }
#endregion

# region Events

    private void Init(object sender, InitData e) { 
        print(e.ENVIRONMENT);
    }

    /// Called when player pressed "play" button
    /// Game prepares itself for the match start and waits
    /// Do not start the match, only prepare for it
    private void GetReady(object sender, GetReadyData e){
        print("GetReady stuff here");
        var manager = GameObject.FindWithTag("GameManager");
        if (manager != null){
            manager.SendMessage("GetReady");
        }
    }
    /// Called when player pressed "practice" button
    private void StartPracticeGame(object sender, EventArgs e){
        print("StartPracticeGame stuff here");
        SceneManager.LoadScene("PracticeGame");
    }
    /// Called when an error occurs
    private void QuitToMenu(object sender, EventArgs e){
        print("QuitToMenu stuff here");
        SceneManager.LoadScene("Menu");
    }
    /// Called when match has started properly
    /// Starts the match
    private void StartRealGame(object sender, EventArgs e){
        print("StartRealGame stuff here");
        SceneManager.LoadScene("RealGame");
    }
    /// Called when the user connects a wallet
    private void Login(object sender, LoginData e){
        print("Login stuff here");
    }
    /// Called when the user disconnects the wallet
    private void Logout(object sender, EventArgs e){
        print("Logout stuff here");
        SceneManager.LoadScene("RealGame");
    }
#endregion
}

}