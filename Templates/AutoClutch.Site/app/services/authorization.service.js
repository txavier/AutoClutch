(function () {
    'use strict';

    angular
        .module('app')
        .factory('authorizationService', authorizationService);

    authorizationService.$inject = ['$rootScope', '$location', '$window', 'authenticationService', 'dataService', 'toaster'];

    function authorizationService($rootScope, $location, $window, authenticationService, dataService, toaster) {
        var loggedInUser = null;

        var service = {
            initialize: initialize,
            authorize: authorize,
            paramsToKeyValue: paramsToKeyValue,
            paramsToStringKeyValue: paramsToStringKeyValue
        };

        return service;

        function paramsToKeyValue(params) {
            var parameters = [];

            angular.forEach(params, function (value, key) {
                this.push({ key: key, value: value });
            }, parameters);

            return parameters;
        }

        function paramsToStringKeyValue(params) {
            var parameters = '';

            var index = 0;

            angular.forEach(params, function (value, key) {
                // If this is part of a list of parameters
                // then add a comma.
                if (index > 0) {
                    parameters += '&'
                }

                parameters += key + '=' + value;

                index++;
            });

            return parameters;
        }

        function initialize() {
            $rootScope.$on('$routeChangeStart', function (event, next) {
                var authorized;
                var history = $window.history;
                var parameters = [];

                if (next.access !== undefined) {
                    parameters = paramsToKeyValue(next.pathParams);

                    return authorize(next.access.loginRequired,
                        next.access.requiredPermissions, 
                        next.access.permissionCheckType,
                        $location.path(),
                        parameters)
                        .then(function (data) {
                            authorized = data;

                            if (authorized == authorized.loginRequired) {
                                toaster.pop('warning', 'Not Authorized', 'Sorry but you are logged in.');

                                $location.path('/');
                            } else if (authorized == authorized.notAuthorized) {
                                toaster.pop('warning', 'Not Authorized', 'Sorry but you are not authorized to view this page.');

                                history.back();
                            }

                            return authorized;
                        });
                }
            });
        }

        function authorize(loginRequired, requiredPermissions, permissionCheckType, uri, parameters) {
            var authorized = false;

            return dataService.getAuthorization(loginRequired, requiredPermissions, permissionCheckType, uri, parameters).then(function (data) {
                authorized = data;

                return authorized;
            });
        }

    }
})();