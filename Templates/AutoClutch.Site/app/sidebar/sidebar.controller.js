(function () {
    'use strict';

    angular
        .module('app')
        .controller('sidebarController', sidebarController);

    sidebarController.$inject = ['$scope', '$log', '$q', '$location', 'authenticationService', 'dataService'];

    function sidebarController($scope, $log, $q, $location, authenticationService, dataService) {
        var vm = this;

        vm.loggedInUser = {};
        vm.goToContract = goToContract;
        vm.searchText = null;
        vm.contract = { contractNumber: '' };
        vm.searchText = '';

        activate();

        function activate() {

            searchContracts();

            getLoggedInUser();

            return vm;
        }

        function searchContracts(searchText) {
            var searchCriteria = {
                currentPage: 1,
                itemsPerPage: 99999,
                orderBy: 'contractNumber',
                searchText: searchText,
                includeProperties: 'section',
                fields: 'contractNumber, new(section.sectionId, section.name) as section',
                q: null
            }

            return dataService.searchEntities('contracts', searchCriteria).then(function (data) {
                vm.contracts = data.$values;

                return vm.contracts;
            });
        }

        function goToContract(contract) {
            // If we have a contract number and we dont have a section 
            // then this is not a contract object but it is actually
            // search text.  Use the search text to search for the contract.
            if (!contract.section) {
                vm.searchText = contract;

                //vm.searchText = 'contractNumber.Contains("' + vm.searchText + '") and section.sectionId=1';

                searchContracts(vm.searchText).then(function (data) {
                    if (data.length == 1) {
                        contract = data[0];

                        $location.path('/' + contract.section.name + '/contracts/' + contract.contractNumber);

                        searchContracts();
                    }
                });
            }
            else {
                $location.path('/' + contract.section.name + '/contracts/' + contract.contractNumber);
            }
        }

        function getLoggedInUser() {
            //return authenticationService.getLoggedInUser().then(function (data) {
            //    vm.loggedInUser = data;

            //    return vm.loggedInUser;
            //});

            var loggedInUser = {};

            var promisedValue = $q(function (resolve, reject) {
                setTimeout(function () { resolve(loggedInUser); }, 1);
            });

            return promisedValue;
        }
    }

})();