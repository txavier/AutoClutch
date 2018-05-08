(function () {
    'use strict';

    angular
        .module('app')
        .controller('UpdateWorkOrderController', UpdateWorkOrderController);

    UpdateWorkOrderController.$inject = ['$scope', '$log', '$window', '$location', '$routeParams', '$timeout', 'dataService', 'authenticationService',
        'contractService', 'documentUploadService', 'toaster', 'sharedService', 'errorService'];

    function UpdateWorkOrderController($scope, $log, $window, $location, $routeParams, $timeout, dataService, authenticationService,
        contractService, documentUploadService, toaster, sharedService, errorService) {
        var vm = this;

        vm.entityDataStore = 'workOrders'
        vm.updateEntity = updateEntity;
        vm.history = $window.history;
        vm.disableAll = false;
        vm.contract = {};
        vm.workOrder = {};
        vm.workOrderStatuses = [];
        vm.status = {};
        vm.payments = [];
        vm.workOrderEmail = {};
        vm.sendEmailUpdate = false;
        vm.loggedInUser = {};
        vm.sectionName = null;
        vm.contractNumber = null;
        vm.saveClicked = false;
        vm.myForm = {};
        vm.disableIfEngineerAndCanceledOrClosed = false;
        vm.disableIfEngineer = false;
        vm.cmmsWorkOrderNumberEditable = false;
        vm.setObject = setObject;
        vm.myForm = {};
        vm.set = null;

        activate();

        function activate() {
            setRouteVariables($routeParams.contractNumber, $routeParams.sectionName, $routeParams.currentPage);

            vm.contractNumber = $routeParams.contractNumber;

            vm.sectionName = $routeParams.sectionName;

            vm.currentPage = $routeParams.currentPage;

            vm.set = $routeParams.set;

            console.log($location.search());

            vm.workOrderNumber = $routeParams.workOrderNumber;

            getContract($routeParams.contractNumber, false).then(function (data) {
                getPayments(vm.contract.contractId);
                getSelectedWorkOrder(vm.workOrderNumber);
                setViewDisabledBoxes();
                getInitialWorkOrderEmail(vm.workOrderNumber).then(function () {
                    vm.myForm.$setPristine();
                });
            });
        }

        function setObject(model, propertyName, propertyNameId, entityArray, id) {
            var result = sharedService.setObject(model, propertyName, propertyNameId, entityArray, id);

            return result;
        }

        function setRouteVariables(contractNumber, sectionName, pageNumber) {
            vm.sectionName = sectionName;

            vm.contractNumber = contractNumber;

            vm.currentPage = pageNumber;
        }

        function getInitialWorkOrderEmail(workOrderNumber) {
            return dataService.getInitialWorkOrderEmail(workOrderNumber).then(function (data) {
                vm.workOrderEmail = data;
            });
        }

        function getPayments(contractId) {
            var paymentSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 100000,
                orderBy: null,
                searchText: null,
                includeProperties: null,
                q: 'contractId==' + contractId
            }

            return dataService.searchEntitiesOData('payments', paymentSearchCriteria).then(function (data) {
                vm.payments = data;

                return vm.payments;
            });
        }


        function getSelectedWorkOrder(workOrderNumber) {
            if (!workOrderNumber) {
                return null;
            }

            vm.status.isFirstOpen = false;

            vm.status.workOrderOpen = true;

            vm.status.workOrderDetailsOpen = true;

            vm.workOrder = vm.contract.workOrders[vm.contract.workOrders.getIndexBy('workOrderNumber', workOrderNumber)];

            if (!vm.workOrder.cmmsWorkOrderNumber) {
                vm.cmmsWorkOrderNumberEditable = true;
            }

            // Get work order statuses for the work order status drop down.
            getWorkOrderStatuses(vm.workOrder);
        }

        function getWorkOrderStatuses(workOrder) {
            return dataService.searchEntitiesOData('workOrderStatuses', { orderBy: 'no' }).then(function (data) {
                vm.workOrderStatuses = data;

                if (workOrder) {
                    vm.workOrder.workOrderStatus = vm.workOrderStatuses[vm.workOrderStatuses.getIndexBy('workOrderStatusId', workOrder.workOrderStatusId)];
                }

                return vm.workOrderStatuses;
            });
        }

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
        }

        function setViewDisabledBoxes() {
            // The contract status 'Closed' is contract status ID 5.
            // The contract status 'Cancelled' is contract status ID 7.
            // If this is a closed or cancelled contract then we are not allowing the 
            // modification of any of the elements.
            if (vm.contract.contractStatusId == 5 || vm.contract.contractStatusId == 7) {
                vm.disableAll = true;
            }

            authenticationService.getLoggedInUser().then(function (data) {

                vm.loggedInUser = data;

                if (vm.loggedInUser.engineerRole) {
                    vm.disableIfEngineer = true;
                }

                // The work order status 'Cancelled' is work order status ID 3.
                // The work order status 'Closed' is work order status ID 2 (PBI 620).
                if (vm.workOrder && vm.workOrder.workOrderStatusId && (vm.workOrder.workOrderStatusId == 3 || vm.workOrder.workOrderStatusId == 2)) {
                    // If this is a section chief then they are allowed to change
                    // the information.
                    if (vm.loggedInUser.sectionChiefRole) {
                        vm.disableAll = true;

                        vm.disableIfEngineerAndCanceledOrClosed = false;
                    }
                    else {
                        vm.disableAll = true;

                        vm.disableIfEngineerAndCanceledOrClosed = true;
                    }

                }
            });
            return vm.disableAll;
        }

        // Based on this contract make html elements visible or invisible.
        function setViewVisibleElements(contract) {

            // ContractTypeId 1 is Open Market Order.
            if (contract.contractTypeId == 1) {
                vm.openMarketOrderInvisible = true;
            }
        }

        function getContract(id) {
            var contractSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 1,
                orderBy: null,
                searchText: null,
                includeProperties: 'engineerContracts($expand=engineer), contractor, contractStatus, contractType, contractCategory, workOrders, '
                    + 'workOrders($expand=location), payments($expand=paymentType), changeOrders($expand=changeOrderType), section,'
                    + 'workOrders($expand=serviceType), workOrders($expand=repairType),contract11,type',
                q: 'contractNumber="' + id + '"'
            }

            return dataService.searchEntitiesOData('contracts', contractSearchCriteria).then(function (data) {
                vm.contract = data[0];

                return vm.contract;
            });
        }

        function updateEntity(entityDataStore, id, entity) {
            if (!vm.myForm.$valid) {
                var errors = [];

                for (var key in vm.myForm.$error) {
                    for (var index = 0; index < vm.myForm.$error[key].length; index++) {
                        errors.push(vm.myForm.$error[key][index].$name + ' is required.');
                    }
                }

                toaster.pop('warning', 'Information Missing', 'The ' + errors[0]);

                vm.saveClicked = false;

                return;
            }

            if (vm.sendEmailUpdate && (!vm.workOrderEmail.to || vm.workOrderEmail.to.length == 0)) {
                toaster.pop('warning', 'Information Missing', 'The "to" field is required if you are sending an email.');

                vm.saveClicked = false;

                return;
            }

            // If the work order status was set to canceled then the cancelComments are required.
            if (vm.workOrder.workOrderStatus.name == 'Canceled' && !vm.workOrder.cancelComments) {
                toaster.pop('warning', 'Information Missing', 'The cancelled comment is required.');

                vm.saveClicked = false;

                return;
            }

            var tempWorkOrderStatus = entity.workOrderStatus;

            var tempPayment = entity.payment;

            var tempEngineer = entity.engineer;

            var tempLocation = entity.location;

            var tempRepairType = entity.repairType;

            var tempWorkOrderStatus = entity.workOrderStatus;

            var tempServiceType = entity.serviceType;

            var tempPayment = entity.payment;

            entity.engineer = null;

            entity.location = null;

            entity.repairType = null;

            entity.workOrderStatus = null;

            entity.serviceType = null;

            entity.payment = null;

            // Update the work order.
            return dataService.updateEntityOData(entityDataStore, id, entity, entityDataStore != 'contracts').then(function () {
                vm.workOrderEmail.workOrderId = entity.workOrderId;

                // Send the email.
                if (vm.sendEmailUpdate) {
                    return dataService.sendWorkOrderEmail(vm.workOrderEmail).then(function () {
                        toaster.pop('success', 'The work order email has been sent.');
                    }, function () {
                        toaster.pop('warning', 'An error occured while trying to email this work order.');

                        // If server side validation fails or the updating goes wrong then keep
                        // the user on the page and show all the values that were set to null.
                        entity.workOrderStatus = tempWorkOrderStatus;

                        entity.payment = tempPayment;

                        entity.engineer = tempEngineer;

                        entity.location = tempLocation;

                        entity.repairType = tempRepairType;

                        entity.serviceType = tempServiceType;

                        entity.payment = tempPayment;
                    });
                }

                $log.log('Successfully updated.');

                entity.workOrderStatus = tempWorkOrderStatus;

                entity.payment = tempPayment;

                vm.saveClicked = false;

                // After a successful saving ensure that the user
                // can no longer make any changes to the work order
                // cmms number field.
                vm.cmmsWorkOrderNumberEditable = false;

                activate();
            }, function (error) {
                errorService.handleError(error, false, entityDataStore, null, true);

                // If server side validation fails or the updating goes wrong then keep
                // the user on the page and show all the values that were set to null.
                entity.workOrderStatus = tempWorkOrderStatus;

                entity.payment = tempPayment;

                entity.engineer = tempEngineer;

                entity.location = tempLocation;

                entity.repairType = tempRepairType;

                entity.serviceType = tempServiceType;

                entity.payment = tempPayment;
            });
        }

    }

})();