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
        vm.contract = {};
        vm.searchText = '';

        activate();

        function activate() {

            searchContracts();

            return vm;
        }

        function searchContracts(searchText) {
            var searchCriteria = {
                currentPage: 1,
                itemsPerPage: 99999,
                orderBy: 'contractNumber',
                searchText: searchText,
                includeProperties: 'section($select=sectionId,name)',
                fields: 'contractNumber,section',
                q: null
            }

            return dataService.searchEntitiesOData('contracts', searchCriteria).then(function (data) {
                vm.contracts = data;

                return vm.contracts;
            });
        }

        // http://localhost/$safeprojectname$/odata/paymentsOData?$expand=contract($select=contractNumber),contract($expand=section($select=name)),paymentType
        function goToContract(contract) {
            // If this is a cmms number then show the associated work order.
            $location.search('set', null);
            $location.search('currentPage', null);
            if (isNumber(contract.contractNumber) && contract.contractNumber.length === 7) {
                var workOrderSearchCriteria = {
                    currentPage: 1,
                    itemsPerPage: 99999,
                    orderBy: 'cmmsWorkOrderNumber',
                    searchText: null,
                    includeProperties: 'contract($expand=section),contract($select=section($select=name))',
                    fields: 'cmmsWorkOrderNumber, contract/section/name, workOrderNumber, contract($select=contractNumber), workOrderId',
                    q: 'cmmsWorkOrderNumber=="' + contract.contractNumber + '"'
                }

                return dataService.searchEntities('workOrders', workOrderSearchCriteria).then(function (data) {
                    vm.workOrders = data;

                    if (vm.workOrders.length === 1) {
                        contract.contractNumber = {
                            contractNumberReal: vm.workOrders[0].contractNumber,
                            contractNumber: vm.workOrders[0].cmmsWorkOrderNumber,
                            workOrderNumber: vm.workOrders[0].workOrderNumber,
                            workOrderId: vm.workOrders[0].workOrderId,
                            section: { name: vm.workOrders[0].name }
                        };

                        $location.path('/' + contract.contractNumber.section.name + '/contracts/' + contract.contractNumber.contractNumberReal
                            + '/work-orders/' + contract.contractNumber.workOrderNumber);
                    }
                    else {
                        goToSearchPage(contract);
                    }

                });
            }
                // If we have a contract number and we dont have a section 
                // then this is not a contract object but it is actually
                // search text.  Use the search text to search for the contract.
            else if (!contract.contractNumber || !contract.contractNumber.section) {
                goToSearchPage(contract);
            }
                // If this is a contract number show the contract.
            else {
                $location.path('/' + contract.contractNumber.section.name + '/contracts/' + contract.contractNumber.contractNumber);
            }
        }

        function goToSearchPage(contract) {
            if (contract.contractNumber == '') {
                contract = '';
            }

            vm.searchText = contract.contractNumber || '';

            $location.path('/search/' + vm.searchText);
        }

        function isNumber(value) {
            if (~~value === 0) {
                return false;
            }

            return true;
        }

    }

})();