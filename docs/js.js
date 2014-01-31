var app = angular.module('App',['ngAnimate']);

function MainController($scope)
{
    $scope.profiles = [];
    
    _.each(["677515526", "641287676", "542059070", "574748744", "762012812", "100006381820882", "681217883", "589678208"], function(profile, index) {
        
        setTimeout(function() {
            $scope.$apply(function() {
                $scope.profiles.push(profile);
            });
            
        }, index * 1500);
        
    });}