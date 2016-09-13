(function () {
    'use strict';

    angular
        .module('app')
        .config(config);

    function config($routeProvider, $locationProvider) {
        $routeProvider
            .when('/view-action-figure/:actionFigureId', {
                templateUrl: 'app/action-figures/view-action-figure.html',
                controller: 'ViewActionFigureController',
                controllerAs: 'vm'
            })
            .when('/add-action-figure', {
                templateUrl: 'app/action-figures/add-action-figure.html',
                controller: 'AddActionFigureController',
                controllerAs: 'vm'
            })
            .when('/home', {
                templateUrl: 'app/home/home.html',
                controller: 'homeController',
                controllerAs: 'vm'
            })
            .otherwise({ redirectTo: 'home' });
    }

})();