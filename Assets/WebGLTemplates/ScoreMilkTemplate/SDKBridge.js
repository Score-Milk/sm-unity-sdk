// The Score Milk SDK Bridge is responsible for
// relaying frontend messages to the game

// Game to frontend
window.postMessage = (message, data) => {
	window.parent.postMessage({
		message,
		data: data ? JSON.stringify(data) : undefined
	}, "*");
}

// Frontend to game
window.addEventListener('load', async function () {
	window.addEventListener("message", (event) => {
		const { message, data } = event.data;

		if (typeof message === 'string') {
			window.unityInstance.SendMessage(
				'ScoreMilkManager',
				message,
				data ? JSON.stringify(data) : undefined
			)
		}
	});
});
