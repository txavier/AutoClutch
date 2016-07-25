(function () {
    'use strict';

    angular
        .module('app')
        .factory('authenticationService', authenticationService);

    authenticationService.$inject = ['$q', 'dataService'];

    function authenticationService($q, dataService) {
        var loggedInUser = null;

        var service = {
            getLoggedInUser: getLoggedInUser
        };

        return service;

        function getLoggedInUser() {
            if (loggedInUser) {
                var promisedValue = $q(function (resolve, reject) {
                    setTimeout(function () { resolve(loggedInUser); }, 1);
                });

                return promisedValue;
            }

            return dataService.getLoggedInUser().then(function (data) {
                loggedInUser = data;

                return loggedInUser;
            });
        }

    }
})();