window.gameLoaded = () => {
	window.parent.postMessage({
		message: 'gameLoaded',
		data: {
			version: '0.2.0',
			platform: 'Unity'
		}
	}, "*");
};
window.enableFullScreen = () => {
	window.unityInstance.SetFullscreen(1);
};

window.disableFullScreen = () => {
	window.unityInstance.SetFullscreen(0);
};


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
