(function () {
    'use strict';

    angular
        .module('app')
        .controller('WorkOrderHistoriesController', WorkOrderHistoriesController);

    WorkOrderHistoriesController.$inject = ['$scope', '$log', '$routeParams', 'dataService'];

    function WorkOrderHistoriesController($scope, $log, $routeParams, dataService) {
        var vm = this;

        vm.entityDataStore = 'workOrderHistories';
        vm.entities = [];
        vm.searchEntities = searchEntities;
        vm.deleteEntity = deleteEntity;
        vm.totalItems = 0;
        vm.itemsPerPage = 10;
        vm.currentPage = 1;
        vm.pageChanged = pageChanged;
        vm.setSortOrder = setSortOrder;
        vm.orderBy = null;
        vm.searchText = null;
        vm.searchCriteria = {};
        vm.searchCriteria.searchText = null;
        vm.includeProperties = null;
        vm.q = null;

        activate();

        function activate() {
            vm.contractNumber = $routeParams.contractNumber;

            vm.sectionName = $routeParams.sectionName;

            vm.workOrderNumber = $routeParams.workOrderNumber;

            vm.q = 'workOrder/workOrderNumber = "' + $routeParams.workOrderNumber + '"';

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties, vm.q);
            searchEntities(vm.searchCriteria);
            searchEntitiesCount(vm.searchCriteria);

            return vm;
        }

        function setSortOrder(orderBy) {
            if (vm.orderBy.indexOf(' desc') === -1) {
                vm.orderBy = vm.orderBy + ' desc';
            } else {
                vm.orderBy = orderBy.replace('desc', '');
            }

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties, vm.q);

            searchEntities(vm.searchCriteria);
        }

        function searchEntities(searchCriteria) {
            return dataService.searchEntitiesOData(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.entities = data;

                return vm.entities;
            });
        }

        function searchEntitiesCount(searchCriteria) {
            return dataService.searchEntitiesCountOData(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.totalItems = data || 0;

                return vm.totalItems;
            });
        }

        function setSearchCriteria(currentPage, itemsPerPage, orderBy, searchText, includeProperties, q) {
            vm.searchCriteria = {
                currentPage: currentPage,
                itemsPerPage: itemsPerPage,
                orderBy: orderBy,
                searchText: searchText,
                includeProperties: includeProperties,
                q: q
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
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties, vm.q);

            searchEntities(vm.searchCriteria);
        }
    }

})();