# Score Milk Unity SDK
Hello and Welcome to the ScoreMilk platform!
With this SDK you can integrate your Unity games to the Score Milk matchmaking system.

# Quick Start
 ### 1) Select "WebGL" as game Platform.
- Go to: File > Build Settings > Platform
- Select "WebGL"
Note: If you don’t find this option or it is disabled, please install WebGL build support package in Unity Hub.

![If you don’t find this option or it is disabled, please install WebGL build support package in Unity Hub](TutorialImages/tutorial1.png)

 ### 2) Set WebGL Template to ScoreMilkTemplate
- Go to: Edit > Project Settings > Player > WebGL > Resolution and Presentation
- Select "ScoreMilkTemplate"

![ScoreMilk Template](TutorialImages/tutorial2.png)

 ### 3) Check "Run in Background" option
- Go to: Edit > Project Settings > Player > WebGL > Resolution and Presentation, if you aren't already
- Check in "Run in Background"

![Check in "Run in Background"](TutorialImages/tutorial3.png)

 ### 4) Drag ScoreMilkManager Prefab to first scene
- Go to folder: Assets/Plugins/ScoreMilk/Prefabs
- Select and Drag "ScoreMilkManager" prefab to scene
- Prefab can be in every scene, it is an singleton that automatically removed copies

![ScoreMilk Manager Prefab](TutorialImages/tutorial4.png)

 ### 5) Subscribe GameObjects to ScoreMilk Events
- You Subscribe events in a GameObject as shown below:
```
private void OnEnable() {
    ScoreMilk.GameInterface.OnGetReady += OnMatchmaking;
}

private void OnDisable() {
    ScoreMilk.GameInterface.OnGetReady -= OnMatchmaking;
}
```
- You have to Subscribe/Unsubscribe to four events:
    - `ScoreMilk.GameInterface.OnInit` - Received as soon as the game calls the `gameLoaded` function.
    - `ScoreMilk.GameInterface.OnGetReady` - Player pressed "play" button. Game should go to real match scene and wait for start.
    - `ScoreMilk.GameInterface.OnStartPracticeGame` - Player pressed "practice" button. Game should go to a practice scene. NOT title screen.
    - `ScoreMilk.GameInterface.OnQuitToMenu` - Match was cancelled for any reason. Game should go back previous scene or title.
    - `ScoreMilk.GameInterface.OnStartRealGame` - Both players are ready and accepted the required crypto transaction. Game should continue and start match.

 - These events are optional:
    - `ScoreMilk.GameInterface.OnLogin` - User logged in. Sends user data in the event
    - `ScoreMilk.GameInterface.OnLogout` - User logged out

 ### 6) Emit messages to server according to game
- You emit messages like this: `ScoreMilk.GameInterface.EmitAddScore(points);`

- You have to emit three events to the backend:
    - `ScoreMilk.GameInterface.EmitReady()`: When the game is ready to start a real match. After emitting this, just wait. Do not start the match yet.
    - `ScoreMilk.GameInterface.EmitAddScore(int points)`: When player scored `points`. `points` can be positive or negative.
    - `ScoreMilk.GameInterface.EmitGameOver(int points)`: When game has ended. `points` emitted at GameOver must be the same as the sum of all `points` emitted previously

- You have to send two messages to the frontend:
    - `ScoreMilk.GameInterface.MessagePractice()`: Call this function whenever the game goes to practice mode.
    - `ScoreMilk.GameInterface.MessageIdle()`: Call this function whenever the game exits practice mode.

Notes:

Location of code varies heavily accordingly with game

Points can be positive or negative. Negative points decrease final value.

Points emitted at GameOver must be the same as the sum of all points emitted previously.

The information flow should go as follows:

![Information Flow](TutorialImages/informationFlow.png)

 ### 7) You are done!

# Example

See `GameControllerExample.cs` script for a usage example and basic explanation

# Community and Support
You can meet other Score Milk users and get support on our [Discord](https://discord.gg/N2mEknPs)!