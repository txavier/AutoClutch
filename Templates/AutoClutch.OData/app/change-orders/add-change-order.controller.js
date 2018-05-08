(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddChangeOrderController', AddChangeOrderController);

    AddChangeOrderController.$inject = ['$scope', '$log', '$window', '$routeParams', 'dataService', 'authenticationService', 'contractService', 'toaster'];

    function AddChangeOrderController($scope, $log, $window, $routeParams, dataService, authenticationService, contractService, toaster) {
        var vm = this;

        vm.entityDataStore = 'changeOrders'
        vm.changeOrder = { identificationDate: new Date() };
        vm.changeOrderTypes = [];
        //vm.changeOrderTypeChange = changeOrderTypeChange;
        vm.history = $window.history;
        vm.log = $log.log;
        vm.addChangeOrder = addChangeOrder;
        vm.initializeNewChangeOrder = initializeNewChangeOrder;
        vm.disableAll = false;
        vm.sectionName = null;
        vm.contractNumber = null;
        vm.myForm = {};

        activate();

        function activate() {
            setRouteVariables($routeParams.contractNumber, $routeParams.sectionName);

            getContract($routeParams.contractNumber).then(function (data) {
                initializeNewChangeOrder();
            }).then(setViewDisabledBoxes);

            getChangeOrderTypes();
        }

        function setRouteVariables(contractNumber, sectionName) {
            vm.sectionName = sectionName;

            vm.contractNumber = contractNumber;
        }

        function setViewDisabledBoxes() {
            var contract = vm.changeOrder.contract;

            // The contract status 'Closed' is contract status ID 5.
            // The contract status 'Cancelled' is contract status ID 7.
            // If this is a closed or cancelled contract then we are not allowing the 
            // modification of any of the elements.
            if (contract.contractStatusId == 5 || contract.contractStatusId == 7) {
                vm.disableAll = true;
            }

            return vm.disableAll;
        }

        //function changeOrderTypeChange(changeOrderType) {
        //    vm.changeOrder.changeOrderTypeId = changeOrderType.changeOrderTypeId;

        //    vm.changeOrder.changeOrderNumber = changeOrderType.coValue;
        //}

        function getChangeOrderTypes() {
            return dataService.getEntitiesOData('changeOrderTypes').then(function (data) {
                vm.changeOrderTypes = data;
                vm.myForm.$setPristine();
                return vm.changeOrderTypes;
            });
        }

        function getContract(contractId) {
            return dataService.getEntity('contracts', contractId).then(function (data) {
                vm.changeOrder.contract = data;

                return vm.changeOrder.contract;
            });
        }

        function getContract(id) {
            var contractSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 1,
                orderBy: null,
                searchText: null,
                includeProperties: 'engineerContracts($expand = engineer),contractor,contractStatus,contractType,contractCategory,workOrders,'
                    + 'workOrders($expand=location),payments($expand=paymentType),changeOrders($expand=changeOrderType),section',
                q: 'contractNumber="' + id + '"'
            }

            return dataService.searchEntitiesOData('contracts', contractSearchCriteria).then(function (data) {
                vm.changeOrder.contract = data[0];

                // Set the contract service contract information for use in the 
                // 'pushOut' sidebar.
                contractService.setContractId(vm.changeOrder.contract.contractId);

                contractService.setContractWorkOrders(vm.changeOrder.contract.workOrders == null ? null : vm.changeOrder.contract.workOrders);

                contractService.setContractNumber(vm.changeOrder.contract.contractNumber);

                contractService.setPayments(vm.changeOrder.contract.payments == null ? null : vm.changeOrder.contract.payments);

                contractService.setChangeOrders(vm.changeOrder.contract.changeOrders == null ? null : vm.changeOrder.contract.changeOrders);
                
                contractService.setEvaluations(vm.changeOrder.contract.evaluations == null ? null : vm.changeOrder.contract.evaluations);

                contractService.setSection(vm.changeOrder.contract.section);

                return vm.changeOrder.contract;
            });
        }

        $scope.$on('$locationChangeStart', function (event) {
            // Reset the contract service contract information for resetting the 
            // 'pushOut' sidebar.
            contractService.setContractId(0);

            contractService.setContractWorkOrders([]);

            contractService.setContractNumber(null);
        });

        function initializeNewChangeOrder() {
            vm.changeOrder.contractId = vm.changeOrder.contractId;

            // There is no reason to have the contract number and contractid
            // on the same table.  The contractNumber field needs to be removed
            // from this model.
            vm.changeOrder.contractNumber = vm.changeOrder.contract.contractNumber;

            if (!vm.changeOrder.changeOrderTypeId) {
                return vm.changeOrder.contractId;
            }

            return dataService.getInitialChangeOrder(vm.changeOrder.contract.contractId, vm.changeOrder.changeOrderTypeId).then(function (data) {
                vm.changeOrder.changeOrderNumber = data.changeOrderNumber;

                return vm.changeOrder.contractId;
            });
        }

        function addChangeOrder(changeOrder) {
            if (!vm.myForm.$valid) {
                var errors = [];

                for (var key in vm.myForm.$error) {
                    for (var index = 0; index < vm.myForm.$error[key].length; index++) {
                        errors.push(vm.myForm.$error[key][index].$name + ' is required.');
                    }
                }

                toaster.pop('warning', 'Information Missing', 'The ' + errors[0]);

                return;
            }

            var tempContract = changeOrder.contract;

            changeOrder.contractId = tempContract.contractId;

            changeOrder.contract = null;

            //var tempChangeOrderType = changeOrder.changeOrderType;

            //changeOrder.changeOrderTypeId = tempChangeOrderType.changeOrderTypeId;

            //changeOrder.changeOrderType = null;

            return dataService.addEntityOData(vm.entityDataStore, changeOrder, true).then(function (data) {
                vm.history.back();

                changeOrder.contract = tempContract;

                //changeOrder.changeOrderType = tempChangeOrderType;
            }, function (error) {
                vm.log('An error occurred.');

                changeOrder.contract = tempContract;

                //changeOrder.changeOrderType = tempChangeOrderType;
            });
        }

    }

})();