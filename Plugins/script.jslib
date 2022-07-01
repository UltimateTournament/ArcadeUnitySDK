mergeInto(LibraryManager.library, {

  Log: function (str) {
	console.log(UTF8ToString(str));
  },

  CloseGame: function () {
	console.log("closing game");
	if (window.self === window.parent) {
		// we're not in an iframe
		alert("Game Ended!");
	} else {
		window.parent.postMessage({"msg":"gameOver"}, '*');
	}
	window.close();
  },

  StoreSettings: function (settings) {
	if (window.self === window.parent) {
		// we're not in an iframe
		window.localStorage.setItem("game-settings", JSON.stringify(settings))
	} else {
		window.parent.postMessage({msg:"storeSettings", reason:UTF8ToString(settings)}, '*');
	}
	window.close();
  },

  ReportErrorAndCloseGame: function (reason) {
	console.log("closing game");
	if (window.self === window.parent) {
		// we're not in an iframe
		alert("Game ended with error!\n", reason);
	} else {
		window.parent.postMessage({msg:"gameError", reason:UTF8ToString(reason)}, '*');
	}
	window.close();
  },

  IsSecure: function () {
	return window.location.protocol === "https:";
  },

  Hostname: function () {
	var returnStr = window.location.hostname;
	var bufferSize = lengthBytesUTF8(returnStr) + 1;
	var buffer = _malloc(bufferSize);
	stringToUTF8(returnStr, buffer, bufferSize);
	return buffer;
  },

  Token: function () {
	var returnStr = window.uaPlayerToken || "";
	if (window.self === window.parent) {
		// we're not in an iframe
		returnStr = "dummy-token";
	}
	if (!window.uaPlayerTokenInit) {
		window.uaPlayerTokenInit = true;
		window.uaPlayerToken = "";
		window.onmessage = (e) => {
			if (e.data.token) {
				window.uaPlayerToken = e.data.token;
			}
		}
		window.top.postMessage({ msg: 'requestToken' }, '*')
	}
	var bufferSize = lengthBytesUTF8(returnStr) + 1;
	var buffer = _malloc(bufferSize);
	stringToUTF8(returnStr, buffer, bufferSize);
	return buffer;
  },

  Settings: function () {
	var returnStr = window.uaPlayerSettings || "{}";
	if (window.self === window.parent) {
		// we're not in an iframe
		returnStr = window.localStorage.getItem("game-settings") ?? "{}";
	}
	if (!window.uaPlayerSettingsInit) {
		window.uaPlayerSettingsInit = true;
		window.uaPlayerSettings = "";
		window.onmessage = (e) => {
			if (e.data.msg === 'settings') {
				window.uaPlayerSettings = e.data.settings;
			}
		}
		window.top.postMessage({ msg: 'requestSettings' }, '*')
	}
	var bufferSize = lengthBytesUTF8(returnStr) + 1;
	var buffer = _malloc(bufferSize);
	stringToUTF8(returnStr, buffer, bufferSize);
	return buffer;
  },

  BaseApiServerName: function () {
	var returnStr = "ultimatearcade.io";
	if (/staging/.test(window.location.hostname)) {
		returnStr = "staging." + returnStr;
	}
	var bufferSize = lengthBytesUTF8(returnStr) + 1;
	var buffer = _malloc(bufferSize);
	stringToUTF8(returnStr, buffer, bufferSize);
	return buffer;
  },

  Port: function () {
	return window.location.port * 1;
  }

});
