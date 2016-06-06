(function () {
    'use strict';

    angular
        .module('app')
        .controller('settingsController', settingsController);

    settingsController.$inject = ['$scope', '$log', 'dataService'];

    function settingsController($scope, $log, dataService) {
        var vm = this;

        vm.entityDataStore = 'settings';
        vm.entities = [];
        vm.searchEntity = searchEntity;
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

        activate();

        function activate() {
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText);
            searchEntity(vm.searchCriteria);
            searchEntityCount(vm.searchCriteria);

            return vm;
        }

        function setSortOrder(orderBy) {
             if (vm.orderBy == orderBy) {
                vm.orderBy = vm.orderBy + ' desc';
            } else {
                vm.orderBy = orderBy;
            }

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText);

            searchEntity(vm.searchCriteria);
        }

        function searchEntity(searchCriteria) {
            return dataService.searchEntity(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.entities = data;

                return vm.entities;
            });
        }

        function searchEntityCount(searchCriteria) {
            return dataService.searchEntityCount(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.totalItems = data || 0;

                return vm.totalItems;
            });
        }

        function setSearchCriteria(currentPage, itemsPerPage, orderBy, searchText) {
            vm.searchCriteria = {
                currentPage: currentPage,
                itemsPerPage: itemsPerPage,
                orderBy: orderBy,
                searchText: searchText,
            }

            return vm.searchCriteria;
        }

        function deleteEntity(id) {
            return dataService.deleteEntity(vm.entityDataStore, id)
                .then(function (data) {
                    searchEntity(vm.searchCriteria);
                });
        }

        function pageChanged() {
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText);

            searchEntity(vm.searchCriteria);
        }
    }

})();