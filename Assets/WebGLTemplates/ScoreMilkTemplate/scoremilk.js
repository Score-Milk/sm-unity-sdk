// SCOREMILK COMMUNICATION BRIDGE v0.1

window.gameLoaded = () => {
	window.parent.postMessage("gameLoaded", "*");
};

window.addEventListener('load', async function () {
	window.addEventListener("message", (event) => {
		const messageData = event.data;

		if (messageData.action == 'setEnvs'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('ScoreMilkManager', 'SetEnvs', JSON.stringify(messageData.envs));
			}
		}
		
		if (messageData.action == 'loadPractice'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('ScoreMilkManager', 'toPracticeGame');
			}
		}

		if (messageData.action == 'loadScene'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('ScoreMilkManager', 'startMatchmaking', JSON.stringify(messageData));
			}
		}

		if (messageData.action == 'canStartGame'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('ScoreMilkManager', 'canStartGame');
			}
		}

		if (messageData.action == 'matchNotFound'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('ScoreMilkManager', 'matchNotFound', JSON.stringify(messageData));
			}
		}

		if (messageData.action === 'connectedWallet'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('ScoreMilkManager', 'walletConnected', JSON.stringify(messageData));
			}
		}

		if (messageData.action === 'disconnectedWallet'){
			if (window.unityInstance != null) {
				window.unityInstance.SendMessage('ScoreMilkManager', 'walletDisconnected');
			}
		}
	});
});
