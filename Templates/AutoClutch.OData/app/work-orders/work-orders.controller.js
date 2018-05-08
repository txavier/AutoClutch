(function () {
    'use strict';

    angular
        .module('app')
        .controller('WorkOrdersController', WorkOrdersController);

    WorkOrdersController.$inject = ['$scope', '$log', '$routeParams', '$filter', 'dataService', 'authenticationService', '$location'];

    function WorkOrdersController($scope, $log, $routeParams, $filter, dataService, authenticationService, $location) {
        var vm = this;

        vm.entityDataStore = 'workOrders';
        vm.entities = [];
        vm.deleteEntity = deleteEntity;
        vm.totalItems = 0;
        vm.itemsPerPage = 10;
        vm.currentPage = 1;
        vm.pageChanged = pageChanged;
        vm.setSortOrder = setSortOrder;
        vm.orderBy = 'contract/contractNumber,workOrderNumber desc';
        vm.searchText = null;
        vm.searchCriteria = {};
        vm.searchCriteria.searchText = null;
        vm.query = '';
        vm.selectedLocation = {};
        vm.location = [];
        vm.selectedWorkOrderStatus = {};
        vm.workOrderStatuses = [];
        vm.filterWorkorders = filterWorkorders;
        vm.loggedInUser = {};
        vm.contractNumber = null;
        vm.engineers = [];
        vm.selectedEngineer = {};

        activate();

        function activate() {
            vm.contractNumber = $routeParams.contractNumber;

            setQuery(vm.contractNumber);      

            getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                vm.currentPage = $location.search().currentPage ? $location.search().currentPage : 1;

                vm.orderBy = $location.search().set ? $location.search().set : 'contract/contractNumber,workOrderNumber desc';

                setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.query);

                searchEntities(vm.entityDataStore, vm.searchCriteria);

                searchEntitiesCount(vm.entityDataStore, vm.searchCriteria);

                getLocations();

                getWorkOrderStatuses();

                getEngineers();
            });

            return vm;
        }

        function setSearchCriteriaForAllWorkOrders() {
            if (vm.searchCriteria.q !== '') {
                vm.searchCriteria.q += ' and '
            }
            else {
                vm.searchCriteria.q = '';
            }

            vm.searchCriteria.q += 'workOrderStatusId=1';

            if (vm.loggedInUser && vm.loggedInUser.sectionId) {
                vm.searchCriteria.q += ' and contract/sectionId=' + vm.loggedInUser.sectionId;
            }

            // If this is not a top level user then only show them information for their 
            // contracts.
            if (!isTopLevelUser() && vm.loggedInUser) {
                vm.searchCriteria.q += ' and engineerId=' + vm.loggedInUser.engineerId;
            }
        }

        function getLoggedInUser() {
            return authenticationService.getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                return vm.loggedInUser;
            });
        }

        function isTopLevelUser() {
            if (!vm.loggedInUser) {
                return false;
            }

            var result = vm.loggedInUser.sectionChiefRole || vm.loggedInUser.adminRole || vm.loggedInUser.areaEngineerRole;

            return result;
        }

        function filterWorkorders() {
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.query);

            searchEntities(vm.entityDataStore, vm.searchCriteria);

            searchEntitiesCount(vm.entityDataStore, vm.searchCriteria);
        }

        function getWorkOrderStatuses() {
            var searchCriteria = {
                currentPage: vm.currentPage,
                itemsPerPage: 200,
                orderBy: null,
                searchText: null,
                includeProperties: null
            }

            return dataService.searchEntitiesOData('workOrderStatuses', searchCriteria, true).then(function (data) {
                vm.workOrderStatuses = [{ name: 'Nothing selected' }];

                var workOrderStatusData = $filter('orderBy')(data, 'name');

                vm.workOrderStatuses = vm.workOrderStatuses.concat(workOrderStatusData);

                return vm.workOrderStatuses;
            });
        }

        function getEngineers() {
            var searchCriteria = {
                currentPage: vm.currentPage,
                itemsPerPage: 200,
                orderBy: null,
                searchText: null,
                includeProperties: null
            };

            return dataService.searchEntitiesOData('engineers', searchCriteria, true).then(function (data) {
                vm.engineers = [{ name: 'Nothing selected' }];

                var engineersData = $filter('orderBy')(data, 'name');

                engineersData = $filter('filter')(engineersData, { sectionId: vm.loggedInUser.sectionId });

                vm.engineers = vm.engineers.concat(engineersData);

                return vm.engineers;
            });
        }

        function getLocations() {
            var searchCriteria = {
                currentPage: vm.currentPage,
                itemsPerPage: 200,
                orderBy: null,
                searchText: null,
                includeProperties: null
            };

            return dataService.searchEntitiesOData('locations', searchCriteria, true).then(function (data) {
                vm.locations = [{ facility: 'Nothing selected' }];

                var locationsData = $filter('orderBy')(data, 'facility');

                vm.locations = vm.locations.concat(locationsData);

                return vm.locations;
            });
        }

        function setQuery(contractId) {
            if (contractId) {
                vm.query = 'contract/contractNumber="' + contractId + '"';
            }
        }

        function setSortOrder(orderBy) {
            if (!['contract.engineer.name', 'cmmsWorkOrderNumber,workOrderNumber desc', 'workOrderStatus.name,workOrderNumber desc', 'location.facility,workOrderNumber desc', 'issuedDate,workOrderNumber desc', 'repairCompletionDate,workOrderNumber desc', 'repairDescription,workOrderNumber desc'].indexOf(orderBy) <= -1) {
                $location.search('set', null);
                orderBy = 'contract/engineer/name';
            }
            if (vm.orderBy.indexOf(' desc') === -1) {
                vm.orderBy = vm.orderBy + ' desc';
            } else {
                vm.orderBy = orderBy.replace('desc', '');
            }

            $location.search('set', vm.orderBy);

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.query);

            searchEntities(vm.entityDataStore, vm.searchCriteria);
        }

        function searchEntities(entityDataStore, searchCriteria) {
            return dataService.searchEntitiesOData(entityDataStore, searchCriteria).then(function (data) {
                vm.entities = data;

                return vm.entities;
            });
        }

        function searchEntitiesCount(entityDataStore, searchCriteria) {
            return dataService.searchEntitiesCountOData(entityDataStore, searchCriteria).then(function (data) {
                vm.totalItems = data || 0;

                return vm.totalItems;
            });
        }

        function setSearchCriteria(currentPage, itemsPerPage, orderBy, searchText, query) {
            vm.searchCriteria = {
                currentPage: currentPage,
                itemsPerPage: itemsPerPage,
                orderBy: orderBy,
                searchText: searchText,
                includeProperties: 'contract($expand=section),location,workOrderStatus,contract($expand=engineer)',
                q: query
            }

            if (vm.selectedLocation && vm.selectedLocation.locationId) {
                // If there was a previous query then put in the and operator.
                if (vm.searchCriteria.q !== '') {
                    vm.searchCriteria.q += ' and '
                }

                vm.searchCriteria.q += 'locationId =' + vm.selectedLocation.locationId;
            }

            if (vm.selectedWorkOrderStatus && vm.selectedWorkOrderStatus.workOrderStatusId) {
                // If there was a previous query then put in the and operator.
                if (vm.searchCriteria.q !== '') {
                    vm.searchCriteria.q += ' and '
                }

                vm.searchCriteria.q += 'workOrderStatusId =' + vm.selectedWorkOrderStatus.workOrderStatusId;
            }

            if (vm.selectedEngineer && vm.selectedEngineer.engineerId) {
                // If there was a previous query then put in the and operator.
                if (vm.searchCriteria.q !== '') {
                    vm.searchCriteria.q += ' and '
                }

                vm.searchCriteria.q += 'contract/engineerId =' + vm.selectedEngineer.engineerId;
            }

            if (!vm.contractNumber) {
                setSearchCriteriaForAllWorkOrders();
            }

            return vm.searchCriteria;
        }

        function deleteEntity(id) {
            return dataService.deleteEntity(vm.entityDataStore, id)
                .then(function (data) {
                    activate();
                });
        }

        function pageChanged() {
            $location.search('currentPage', vm.currentPage);

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.query);

            searchEntities(vm.entityDataStore, vm.searchCriteria);
        }
    }

})();