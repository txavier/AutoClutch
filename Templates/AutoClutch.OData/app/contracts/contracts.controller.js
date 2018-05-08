(function () {
    'use strict';

    angular
        .module('app')
        .controller('ContractsController', ContractsController);

    ContractsController.$inject = ['$scope', '$log', '$rootScope', '$location', '$filter', '$routeParams', 'dataService', 'authenticationService'];

    function ContractsController($scope, $log, $rootScope, $location, $filter, $routeParams, dataService, authenticationService) {
        var vm = this;

        vm.entityDataStore = 'contracts';
        vm.contracts = [];
        vm.deleteEntity = deleteEntity;
        vm.totalItems = 0;
        vm.itemsPerPage = 10;
        vm.currentPage = 1;
        vm.pageChanged = pageChanged;
        vm.setSortOrder = setSortOrder;
        vm.orderBy = 'contractNumber';
        vm.searchText = null;
        vm.searchCriteria = {};
        vm.searchCriteria.searchText = null;
        vm.engineers = [];
        vm.contractStatuses = [];
        vm.selectedEngineerId = null;
        vm.selectedContractStatusId = null;
        vm.loggedInUser = {};
        vm.disableEngineerDropDown = false;
        vm.filterContracts = filterContracts;
        vm.sectionName = '';
        vm.customQueryName = null;

        activate();

        function activate() {
            vm.sectionName = $routeParams.sectionName;

            vm.customQueryName = $routeParams.customQueryName;

            getEngineers().then(authenticationService.getLoggedInUser).then(function (data) {
                vm.loggedInUser = data;

                vm.currentPage = $location.search().currentPage ? $location.search().currentPage : 1;

                vm.orderBy = $location.search().set ? $location.search().set : 'contractNumber';

                if (!vm.customQueryName) {
                    initFilter(vm.selectedEngineerId, vm.selectedContractStatusId);
                    setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.sectionName);
                    searchEntities(vm.searchCriteria);
                    searchEntitiesCount(vm.searchCriteria);
                }
                else if (vm.customQueryName === 'expiringContracts') {
                    var getLoggedInUser = null;

                    if (!isTopLevelUser() && vm.loggedInUser) {
                        getLoggedInUser = vm.loggedInUser.engineerId;
                    }

                    dataService.getDashboardMetric('expiringContracts', getLoggedInUser).then(function (data) {
                        vm.contracts = data;

                        vm.totalItems = data.length;

                        return vm.contracts;
                    });
                }
                else if (vm.customQueryName === 'lowFundContracts') {
                    var getLoggedInUser = null;

                    if (!isTopLevelUser() && vm.loggedInUser) {
                        getLoggedInUser = vm.loggedInUser.engineerId;
                    }

                    dataService.getDashboardMetric('lowFundContracts', getLoggedInUser).then(function (data) {
                        vm.contracts = data;

                        vm.totalItems = data.length;

                        return vm.contracts;
                    });
                }
                else if (vm.customQueryName === 'missingSpec') {
                    var getLoggedInUser = null;

                    if (!isTopLevelUser() && vm.loggedInUser) {
                        getLoggedInUser = vm.loggedInUser.engineerId;
                    }

                    dataService.getDashboardMetric('missingSpec', getLoggedInUser).then(function (data) {
                        vm.contracts = data;

                        vm.totalItems = data.length;

                        return vm.contracts;
                    });
                }
            });

            getContractStatuses();

            return vm;
        }

        function isTopLevelUser() {
            if (!vm.loggedInUser) {
                return false;
            }

            var result = vm.loggedInUser.sectionChiefRole || vm.loggedInUser.adminRole || vm.loggedInUser.areaEngineerRole;

            return result;
        }

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
        }

        function filterContracts() {
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.sectionName);
            searchEntities(vm.searchCriteria);
            searchEntitiesCount(vm.searchCriteria);
        }

        function initFilter(selectedEngineerId, selectedContractStatusId) {
            if (vm.loggedInUser.engineerId
                && vm.loggedInUser.engineerRole
                && !selectedEngineerId) {
                vm.disableEngineerDropDown = true;
                vm.selectedEngineerId = vm.loggedInUser.engineerId;
            }
        }

        function getContractStatuses() {
            var searchCriteria = {
                currentPage: vm.currentPage,
                itemsPerPage: 100,
                orderBy: 'Sort',
                searchText: null,
                includeProperties: null
            }

            return dataService.searchEntitiesOData('contractStatuses', searchCriteria).then(function (data) {
                vm.contractStatuses = [{ name: 'Please select...' }];
                vm.contractStatuses = vm.contractStatuses.concat(data);

                return vm.contractStatuses;
            });
        }

        function getEngineers() {
            var searchCriteria = {

                currentPage: vm.currentPage,
                itemsPerPage: 100,
                orderBy: 'name',
                searchText: null,
                includeProperties: 'section'
            }

            return dataService.searchEntitiesOData('engineers', searchCriteria).then(function (data) {
                vm.engineers = [{ name: 'Please select...' }];

                data = $filter('filter')(data, { section: { name: vm.sectionName } });

                vm.engineers = vm.engineers.concat(data);

                return vm.engineers;
            });
        }

        function setSortOrder(orderBy) {
            if (['contractNumber', 'contractDescription', 'actualEndDateDisplay', 'contractBalanceDisplay', 'contractStatus.name'].indexOf(orderBy) <= -1) {
                $location.search('set', null);
                orderBy = 'contractNumber';
            }
            if (vm.orderBy.indexOf(' desc') === -1) {
                vm.orderBy = vm.orderBy + ' desc';
            } else {
                vm.orderBy = orderBy.replace('desc', '');
            }

            $location.search('set', vm.orderBy);

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.sectionName);

            searchEntities(vm.searchCriteria);
        }

        function searchEntities(searchCriteria) {
            return dataService.searchEntitiesOData(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.contracts = data;

                return vm.contracts;
            });
        }

        function searchEntitiesCount(searchCriteria) {
            return dataService.searchEntitiesCountOData(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.totalItems = data || 0;

                return vm.totalItems;
            });
        }

        function setSearchCriteria(currentPage, itemsPerPage, orderBy, searchText, sectionName) {
            vm.searchCriteria = {
                currentPage: currentPage,
                itemsPerPage: itemsPerPage,
                orderBy: orderBy,
                searchText: searchText,
                //includeProperties: 'contractStatus,section,type,contractType,payments,changeOrders.changeOrderType',
                includeProperties: 'contractStatus,section,type,contractType,payments,changeOrders($expand=changeOrderType)',
                //q: 'section.name="' + sectionName + '"'
                q: 'section/name="' + sectionName + '"'
            }

            if (vm.selectedEngineerId) {
                vm.searchCriteria.q += ' and engineerId=' + vm.selectedEngineerId;
            }

            if (vm.selectedContractStatusId) {
                vm.searchCriteria.q += ' and contractStatusId=' + vm.selectedContractStatusId;
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

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.sectionName);

            searchEntities(vm.searchCriteria);
        }

    }

})();