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
		window.parent.postMessage({"msg":"closeGame"}, '*');
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
	var returnStr = prompt("Enter player token", "");
	var bufferSize = lengthBytesUTF8(returnStr) + 1;
	var buffer = _malloc(bufferSize);
	stringToUTF8(returnStr, buffer, bufferSize);
	return buffer;
  },

  BaseApiServerName: function () {
	console.log("TODO get server name from iframe parent");
	var returnStr = "staging.ultimatearcade.io";
	var bufferSize = lengthBytesUTF8(returnStr) + 1;
	var buffer = _malloc(bufferSize);
	stringToUTF8(returnStr, buffer, bufferSize);
	return buffer;
  },

  Port: function () {
	return window.location.port * 1;
  }

});
