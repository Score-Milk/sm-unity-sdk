// SCOREMILK COMMUNICATION BRIDGE v0.1

window.addEventListener('load', async function () {
	const env = await fetch('/env').then(data => data.json());

	// DEV ONLY, REMOVE THIS IN THE SDK
	window.gameLoaded = () => {
		window.parent.postMessage("gameLoaded", "*");

		window.unityInstance.SendMessage('NetworkManager', 'SetEnvs', JSON.stringify({
			API_URL: `${env.API_URL}/v2/matches`,
		}))
	};
	
	window.addEventListener("message", (event) => {
		const messageData = event.data;
		
		if (messageData.action == 'loadPractice'){
			if (window.unityInstance != null) {
				console.log("action= loadPractice");
				window.unityInstance.SendMessage('NetworkManager', 'toPracticeGame', JSON.stringify(messageData));
			}
		}

		if (messageData.action == 'loadScene'){
			console.log("action= loadScene");
	
			if (window.unityInstance != null) {
				console.log("action= loadScene");
				window.unityInstance.SendMessage('NetworkManager', 'startMatchmaking', JSON.stringify(messageData));
			}
		}

		if (messageData.action == 'canStartGame'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('NetworkManager', 'canStartGame');
			}
		}

		if (messageData.action == 'matchNotFound'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('NetworkManager', 'cancelMatch', JSON.stringify(messageData));
			}
		}

		if (messageData.action === 'connectedWallet'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('ScoreMilkManager', 'connectedWallet', JSON.stringify(messageData));
			}
		}

		if (messageData.action === 'disconnectedWallet'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('ScoreMilkManager', 'disconnectedWallet');
			}
		}
	});
});
