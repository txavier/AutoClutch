(function () {
    'use strict';

    angular
        .module('app')
        .config(config);

    config.$inject = ['$routeProvider', '$locationProvider', '$httpProvider'];

    function config($routeProvider, $locationProvider, $httpProvider) {
        // Configure the security token bearer interceptor.
        $httpProvider.interceptors.push('authInterceptorService');

        $routeProvider
            .when('/dashboard', {
                templateUrl: 'app/user/dashboard.html',
                controller: 'DashboardController',
                controllerAs: 'vm'
            })
            .when('/update-action-figure/:actionFigureId', {
                templateUrl: 'app/action-figures/update-action-figure.html',
                controller: 'UpdateActionFigureController',
                controllerAs: 'vm'
            })
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
                templateUrl: '/app/home/home.html',
                controller: 'HomeController',
                controllerAs: 'vm'
            })
            .otherwise({ redirectTo: 'home' });

    }

})();