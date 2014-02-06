angular
	.module("app", ["ngAnimate"])

	.controller("DancerController", ["$scope", function($scope) {

		var viewModel = this;

		var hub = $.connection.danceHub;

		viewModel.move = function(axis, factor, e) {
			e.preventDefault();

			viewModel.dancer['Location' + axis] += factor * 25;
			viewModel.update(e);
		};

		viewModel.update = function(e) {
			e.preventDefault();

			viewModel.updating = true;

			hub.server.update(viewModel.dancer).done(function () {

				$scope.$apply(function() {
					viewModel.updating = false;
				});
			});
		};

		hub.client.update = function(dancer) {
			$scope.$apply(function () {
				viewModel.dancer = dancer;
			});
		};

		$.connection.hub.start().done(function () {

			hub.server.enter().done(function (dancer) {

				$scope.$apply(function () {
					viewModel.dancer = dancer;
					viewModel.connected = true;
				});
			});
		});

		return viewModel;
	}])

	.controller("DanceRingController", ["$scope", function ($scope) {

		var viewModel = this;

		viewModel.dancers = [];

		var dancerByIndex = function (dancer) {

			var index = _.findIndex(viewModel.dancers, function (existingDancer) {
				return existingDancer.Id === dancer.Id;
			});

			return index;
		};

		var hub = $.connection.danceHub;

		hub.client.add = function (dancer) {

			$scope.$apply(function () {
				viewModel.dancers.push(dancer);
			});
		};

		hub.client.remove = function (dancer) {

			var index = dancerByIndex(dancer);

			$scope.$apply(function () {
				viewModel.dancers.splice(index, 1);
			});
		};

		hub.client.update = function (dancer) {
			
			var index = dancerByIndex(dancer);

			$scope.$apply(function () {
				viewModel.dancers[index] = dancer;
			});
		};

		$.connection.hub.start().done(function () {

			hub.server.manage().done(function (dancers) {

				$scope.$apply(function () {
					viewModel.dancers = dancers;
					viewModel.connected = true;
				});
			});
		});

		return viewModel;
	}])
;