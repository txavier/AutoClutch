﻿(function () {
    'use strict';

    angular
        .module('app')
        .controller('UserActionLogsController', UserActionLogsController);

    UserActionLogsController.$inject = ['$scope', '$log', 'dataService'];

    function UserActionLogsController($scope, $log, dataService) {
        var vm = this;

        vm.entityDataStore = 'userActionLogs';
        vm.entities = [];
        vm.searchEntities = searchEntities;
        vm.deleteEntity = deleteEntity;
        vm.totalItems = 0;
        vm.itemsPerPage = 10;
        vm.currentPage = 1;
        vm.pageChanged = pageChanged;
        vm.setSortOrder = setSortOrder;
        vm.orderBy = 'userActionLogId desc';
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

            searchEntity(vm.searchCriteria);
        }

        function searchEntities(searchCriteria) {
            return dataService.getRestrictedUserActionLogs(searchCriteria).then(function (data) {
                vm.entities = data;

                return vm.entities;
            });
        }

        function searchEntitiesCount(searchCriteria) {
            return dataService.getRestrictedUserActionLogsCount(searchCriteria).then(function (data) {
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