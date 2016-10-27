(function () {
    'use strict';

    angular
    .module('app')
    .config(config);
    
    config.$inject = ['$routeProvider', '$locationProvider', 'cfpLoadingBarProvider'];

    function config($routeProvider, $locationProvider, cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;

        $routeProvider
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
            //.when('/:sectionName/contracts/:contractNumber', {
            //    templateUrl: 'app/contracts/contract.html',
            //    controller: 'ContractController',
            //    controllerAs: 'vm',
            //    //access: {
            //    //    loginRequired: true,
            //    //    requiredPermissions: ['admin', 'sameSection', 'owner'],
            //    //    permissionCheckType: 'AtLeastOne'
            //    //}
            //})
            .when('/home', {
                templateUrl: 'app/home/home.html',
                controller: 'HomeController',
                controllerAs: 'vm'
            })
            .otherwise({ redirectTo: 'home' });
    }
})();