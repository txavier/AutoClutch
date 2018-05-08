(function () {
    'use strict';

    angular
        .module('app')
        .controller('contractReportController', contractReportController);

    contractReportController.$inject =['$scope', '$routeParams', '$q', 'dataService', 'authenticationService'];


        function contractReportController($scope, $routeParams, $q, dataService, authenticationService) {
            var vm = this;

            vm.engineers = [];
            vm.parameters = [];
            vm.loggedInUser = {};

            activate();

            function activate() {
                authenticationService.getLoggedInUser().then(function (data) {
                    vm.loggedInUser = data;

                    getEngineers($routeParams.sectionName).then(getParameters);
                });
            }

            function getParameters(engineers) {
                angular.forEach(engineers, function (value, key) {
                    this.push({ key: 'Engineer', value: value.engineerId })
                }, vm.parameters);
                    
                return vm.parameters;
            }

            function getEngineers(sectionName) {
                if (vm.loggedInUser.engineerRole) {
                    vm.engineers = [vm.loggedInUser];

                    var promisedValue = $q(function (resolve, reject) {
                        setTimeout(function () { resolve(vm.engineers); }, 1);
                    });

                    return promisedValue;
                }
                else {
                    var searchCriteria = { q: 'section/name="' + sectionName + '"' }

                    return dataService.searchEntitiesOData('engineers', searchCriteria, true).then(function (data) {
                        vm.engineers = data;

                        return vm.engineers;
                    });
                }
            }

    }

})();