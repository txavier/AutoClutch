(function () {
    'use strict';

    angular
        .module('app')
        .factory('authInterceptorService', authInterceptorService);
    
    authInterceptorService.$inject = ['$q', '$location', '$sessionStorage'];
    
    function authInterceptorService($q, $location, $sessionStorage) {
        var authInterceptorServiceFactory = {
            request: _request,
            responseError: _responseError,
        };

        return authInterceptorServiceFactory;

        function _request(config) {
            config.headers = config.headers || {};

            var authDataToken = $sessionStorage.get('accessToken');
            if (authDataToken) {
                config.headers.Authorization = 'Bearer ' + authDataToken;
            }

            return config;
        }

        function _responseError(rejection) {
            if (rejection.status === 401) {
                $location.path('/login');
            }

            return $q.reject(rejection);
        }
    }

})();