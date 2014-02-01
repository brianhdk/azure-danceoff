angular
	.module("app", ["ngAnimate"])

	.controller("DancerController", ["$scope", function($scope) {

		var viewModel = this;

		var hub = $.connection.danceHub;

		hub.client.update = function (ring) {

			$scope.$apply(function () {
				viewModel.ring = ring;
			});
		};

		viewModel.updateStatus = function(e) {
			e.preventDefault();

			viewModel.updating = true;

			viewModel.dancer.Status = viewModel.statusText;

			hub.server.update(viewModel.dancer).done(function () {

				$scope.$apply(function() {
					viewModel.updating = false;
					viewModel.statusText = '';
				});
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
;