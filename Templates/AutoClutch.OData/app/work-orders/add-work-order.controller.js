(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddWorkOrderController', AddWorkOrderController);

    AddWorkOrderController.$inject = [
        '$scope',
        '$log',
        '$routeParams',
        '$window',
        '$location',
        '$filter',
        'dataService',
        'authenticationService',
        'contractService',
        
        'toaster',
        'sharedService'
    ];

    function AddWorkOrderController(
        $scope,
        $log,
        $routeParams,
        $window,
        $location,
        $filter,
        dataService,
        authenticationService,
        contractService,
       
        toaster,
        sharedService) {
        var vm = this;

        vm.$log = $log;

        vm.entityDataStore = 'workOrders'
        vm.workOrder = { contract: {} };
        vm.engineers = [];
        vm.contractTypes = [];
        vm.locations = [];
        vm.repairTypes = [];
        vm.serviceTypes = [];
        vm.addEntity = addEntity;
        vm.getNewWorkOrderNumber = getNewWorkOrderNumber;
        vm.history = $window.history;
        vm.log = $log;
        vm.disableAll = false;
        vm.workOrderEmail = {};
        vm.sendEmailUpdate = true;
        vm.setObject = setObject;
        vm.myForm = {};

        activate();

        function activate() {
            initializeNewWorkOrder($routeParams.contractNumber);

            getContract(vm.workOrder.contract.contractNumber).then(function (data) {
                setViewDisabledBoxes(vm.workOrder.contract);

                getInitialWorkOrderEmail(vm.workOrder.contract.contractId);
            });

            getEngineers();

            getContractTypes();

            getLocations();

            getRepairTypes();

            getServiceTypes();
        }

        function setObject(model, propertyName, propertyNameId, entityArray, id) {
            var result = sharedService.setObject(model, propertyName, propertyNameId, entityArray, id);

            return result;
        }

        function getInitialWorkOrderEmail(contractId) {
            dataService.getInitialWorkOrderEmail(null, contractId).then(function (data) {
                vm.workOrderEmail = data;
            });
        }

        function setViewDisabledBoxes(contract) {
            // The contract status 'Closed' is contract status ID 5.
            // The contract status 'Cancelled' is contract status ID 7.
            // If this is a closed or cancelled contract then we are not allowing the 
            // modification of any of the elements.
            if (contract.contractStatusId == 5 || contract.contractStatusId == 7) {
                vm.disableAll = true;
            }

            return vm.disableAll;
        }

        function getContract(contractNumber) {
            var contractSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 1,
                orderBy: null,
                searchText: null,
                includeProperties: 'engineerContracts($expand=engineer), contractor, contractStatus, contractType, contractCategory, workOrders, workOrders($expand=location), payments($expand=paymentType), changeOrders($expand=changeOrderType), section',
                q: 'contractNumber="' + contractNumber + '"'
            }

            return dataService.searchEntitiesOData('contracts', contractSearchCriteria).then(function (data) {
                vm.workOrder.contract = data[0];

                // Set the contract id so that when saving the contractId
                // will be available to validate this workorder.
                vm.workOrder.contractId = vm.workOrder.contract.contractId;

                return vm.workOrder.contract;
            });
        }

        $scope.$on('$locationChangeStart', function (event) {
            // Reset the contract service contract information for resetting the 
            // 'pushOut' sidebar.
            contractService.setContractId(0);

            contractService.setContractWorkOrders([]);

            contractService.setContractNumber(null);
        });

        function getContractTypes() {
            return dataService.getEntitiesOData('contractTypes').then(function (data) {
                vm.contractTypes = data;

                return vm.contractTypes;
            });
        }

        function getEngineers() {
            return dataService.getEntitiesOData('engineers').then(function (data) {
                vm.engineers = data;

                return vm.engineers;
            });
        }

        function getLocations() {
            return dataService.getEntitiesOData('locations').then(function (data) {
                vm.locations = data;

                vm.locations = $filter('orderBy')(vm.locations, 'facility');

                return vm.locations;
            })
        }

        function getRepairTypes() {
            return dataService.getEntitiesOData('repairTypes').then(function (data) {
                vm.repairTypes = data;

                return vm.repairTypes;
            })
        }

        function getServiceTypes() {
            return dataService.getEntitiesOData('serviceTypes').then(function (data) {
                vm.serviceTypes = data;
                vm.myForm.$setPristine();
                return vm.serviceTypes;
            })
        }

        function initializeNewWorkOrder(contractId) {
            vm.workOrder.contract.contractNumber = $routeParams.contractNumber;

            vm.workOrder.issuedDate = new Date();
        }

        function getNewWorkOrderNumber(contractId, locationId) {
            return dataService.getNewWorkOrderNumber(contractId, locationId).then(function (data) {
                vm.workOrder.workOrderNumber = data;

                return vm.workOrder.workOrderNumber;
            });
        }

        function addEntity(entity) {
            // Remove some unnecessary properties so that the transmission time is
            // improved and for validation rules to pass.
            var tempContract = entity.contract;

            entity.contract = null;

            if (!entity.location) {
                toaster.pop('warning', 'Location', 'Please set the facility.');

                // On an error, get the temporary values back in order to set up 
                // the view so that the user can make corrections as needed to 
                // correct the values for a subsequent submittal of data.
                entity.contract = tempContract;

                return;
            }

            if (vm.sendEmailUpdate && (!vm.workOrderEmail.to || vm.workOrderEmail.to.length == 0)) {
                toaster.pop('warning', 'Information Missing', 'The "to" field is required if you are sending an email.');

                vm.saveClicked = false;

                return;
            }

            //entity.locationId = entity.location.locationId;

            var tempLocation = entity.location;

            entity.location = null;

            if (!entity.repairTypeId) {
                toaster.pop('warning', 'Repair Type', 'Please set the repair type.');

                // On an error, get the temporary values back in order to set up 
                // the view so that the user can make corrections as needed to 
                // correct the values for a subsequent submittal of data.
                entity.contract = tempContract;

                entity.location = tempLocation;

                return;
            }

            //entity.repairTypeId = entity.repairType.repairTypeId;

            var tempRepairType = entity.repairType;

            entity.repairType = null;

            if (!entity.serviceTypeId) {
                toaster.pop('warning', 'Service Type', 'Please set the service type.');

                // On an error, get the temporary values back in order to set up 
                // the view so that the user can make corrections as needed to 
                // correct the values for a subsequent submittal of data.
                entity.contract = tempContract;

                entity.location = tempLocation;

                entity.repairType = tempRepairType;

                return;
            }

            entity.serviceTypeId = entity.serviceType.serviceTypeId;

            var tempServiceType = entity.serviceType;

            entity.serviceType = null;

            return dataService.addEntityOData(vm.entityDataStore, entity, true).then(function (data) {
                entity.workOrderId = data.workOrderId;

                vm.workOrderEmail.workOrderId = entity.workOrderId;

                // Send the email.
                if (vm.sendEmailUpdate && vm.workOrderEmail.body) {
                    return dataService.sendWorkOrderEmail(vm.workOrderEmail).then(function () {
                        toaster.pop('success', 'The work order email has been sent.');

                        $location.path('/' + $routeParams.sectionName + '/contracts/' + $routeParams.contractNumber + '/work-orders');
                    });
                }

                $location.path('/' + $routeParams.sectionName + '/contracts/' + $routeParams.contractNumber + '/work-orders');
            }, function (error) {

                errorService.handleError(error, false, entityDataStore, null, true);

                // On an error, get the temporary values back in order to set up 
                // the view so that the user can make corrections as needed to 
                // correct the values for a subsequent submittal of data.
                entity.contract = tempContract;

                entity.location = tempLocation;

                entity.repairType = tempRepairType;

                entity.serviceType = tempServiceType;
            });
        }

    }

})();