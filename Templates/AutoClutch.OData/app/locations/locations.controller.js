(function () {
    'use strict';

    angular
        .module('app')
        .controller('LocationsController', LocationsController);

    LocationsController.$inject = ['$scope', '$log', 'dataService'];

    function LocationsController($scope, $log, dataService) {
        var vm = this;

        vm.entityDataStore = 'locations';
        vm.entities = [];
        vm.searchEntities = searchEntities;
        vm.deleteEntity = deleteEntity;
        vm.totalItems = 0;
        vm.itemsPerPage = 10;
        vm.currentPage = 1;
        vm.pageChanged = pageChanged;
        vm.setSortOrder = setSortOrder;
        vm.orderBy = 'facility';
        vm.searchText = null;
        vm.searchCriteria = {};
        vm.searchCriteria.searchText = null;
        vm.includeProperties = null;

        activate();

        function activate() {
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties);
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

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties);

            searchEntities(vm.searchCriteria);
        }

        function searchEntities(searchCriteria) {
            return dataService.searchEntitiesOData(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.entities = data;

                return vm.entities;
            });
        }

        // Helper function for sorting by last name where the last name is after the first name in the list
        // i.e. 'George Bush'.
        function compare(a, b) {
            var splitA = a['name'].split(" ");
            var splitB = b['name'].split(" ");
            var lastA = splitA[splitA.length - 1];
            var lastB = splitB[splitB.length - 1];

            if (lastA < lastB) return -1;
            if (lastA > lastB) return 1;
            return 0;
        }

        function searchEntitiesCount(searchCriteria) {
            return dataService.searchEntitiesCountOData(vm.entityDataStore, searchCriteria).then(function (data) {
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