angular
.module("app", ["ngAnimate"])
.controller("TestController", ["$scope", function ($scope) {

	var test = this;
	test.model = [];

	// Declare a proxy to reference the hub. 
	var chat = $.connection.chatHub;

	// Create a function that the hub can call to broadcast messages.
	chat.client.broadcastMessage = function (profileId, message) {

		$scope.$apply(function() {
			test.model.push({ profileId: profileId, message: message });
		});
	};

	// Start the connection.
	$.connection.hub.start().done(function () {
		test.ready = true;
	});

	test.send = function(e) {
		e.preventDefault();

		chat.server.send(test.value);

		test.value = '';
	};

	return test;
}]);