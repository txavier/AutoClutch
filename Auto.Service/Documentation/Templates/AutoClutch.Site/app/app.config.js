(function () {
    'use strict';

    angular
    .module('app')
    .config(config);

    function config($routeProvider, $locationProvider, cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;

        $routeProvider
            .when('/about', {
                templateUrl: 'app/about/about.html',
                controller: 'AboutController',
                controllerAs: 'vm'
            })
            .when('/users', {
                templateUrl: 'app/users/users.html',
                controller: 'UsersController',
                controllerAs: 'vm'
            })
            .when('/add-user', {
                templateUrl: 'app/users/add-user.html',
                controller: 'AddUserController',
                controllerAs: 'vm'
            })
            .when('/update-user/:userId', {
                templateUrl: 'app/users/update-user.html',
                controller: 'UpdateUserController',
                controllerAs: 'vm'
            })
            .when('/user-action-logs', {
                templateUrl: 'app/user-action-logs/user-action-logs.html',
                controller: 'UserActionLogsController',
                controllerAs: 'vm'
            })
            .when('/history/:typeFullName/:id', {
                templateUrl: 'app/histories/history.html',
                controller: 'historyController',
                controllerAs: 'vm'
            })
            .when('/histories', {
                templateUrl: 'app/histories/histories.html',
                controller: 'historiesController',
                controllerAs: 'vm'
            })
            .when('/home', {
                templateUrl: 'app/home/home.html',
                controller: 'HomeController',
                controllerAs: 'vm'
            })
            .otherwise({ redirectTo: 'home' });
    }

    angular
        .module('app')
        .config(['angularBingMapsProvider', function (angularBingMapsProvider) {
            angularBingMapsProvider.setDefaultMapOptions({
                credentials: 'Aud9zLnF-UWZGKBR77bVTv3uSEIXmMj1DONIXZBxG59xHNa8HwAys28-rC3_D3SQ',
                enableClickableLogo: false
            });
            angularBingMapsProvider.bindCenterRealtime(false);
        }]);

})();