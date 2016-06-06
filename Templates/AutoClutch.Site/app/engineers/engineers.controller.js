(function () {
    'use strict';

    angular
        .module('app')
        .controller('EngineersController', EngineersController);

    EngineersController.$inject = ['$scope', '$log', 'dataService', 'jsonPointerParseService'];

    function EngineersController($scope, $log, dataService, jsonPointerParseService) {
        var vm = this;

        vm.entityDataStore = 'engineers';
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
        vm.includeProperties = 'section';

        activate();

        function activate() {
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties);
            searchEntities(vm.searchCriteria);
            searchEntitiesCount(vm.searchCriteria);

            return vm;
        }

        function setSortOrder(orderBy) {
            if (vm.orderBy == orderBy) {
                vm.orderBy = vm.orderBy + ' desc';
            } else {
                vm.orderBy = orderBy;
            }

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties);

            searchEntity(vm.searchCriteria);
        }

        function searchEntities(searchCriteria) {
            return dataService.searchEntities(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.entities = data.$values;

                return vm.entities;
            });
        }

        function searchEntitiesCount(searchCriteria) {
            return dataService.searchEntitiesCount(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.totalItems = data || 0;

                return vm.totalItems;
            });
        }

        function setSearchCriteria(currentPage, itemsPerPage, orderBy, searchText, includeProperties) {
            vm.searchCriteria = {
                currentPage: currentPage,
                itemsPerPage: itemsPerPage,
                orderBy: orderBy,
                searchText: searchText,
                includeProperties: includeProperties
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
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties);

            searchEntities(vm.searchCriteria);
        }
    }

})();