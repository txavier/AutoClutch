(function () {
    'use strict';

    angular
        .module('app')
        .controller('OpenPaymentsController', OpenPaymentsController);

    OpenPaymentsController.$inject = ['$scope', '$log', '$routeParams', '$filter', 'dataService', 'authenticationService'];

    function OpenPaymentsController($scope, $log, $routeParams, $filter, dataService, authenticationService) {
        var vm = this;

        vm.entityDataStore = 'payments';
        vm.entities = [];
        vm.deleteEntity = deleteEntity;
        vm.totalItems = 0;
        vm.itemsPerPage = 100;
        vm.currentPage = 1;
        vm.pageChanged = pageChanged;
        vm.setSortOrder = setSortOrder;
        vm.orderBy = 'contract/contractNumber,paymentNumber';
        vm.searchText = null;
        vm.searchCriteria = {};
        vm.searchCriteria.searchText = null;
        vm.query = '';
        vm.customQueryName = '';

        activate();

        function activate() {
            vm.customQueryName = $routeParams.customQueryName;

            vm.contractNumber = $routeParams.contractNumber;

            setQuery(vm.contractNumber);

            getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.query);

                searchEntities(vm.entityDataStore, vm.searchCriteria);

                searchEntitiesCount(vm.entityDataStore, vm.searchCriteria);
            });

            return vm;
        }

        function getLoggedInUser() {
            return authenticationService.getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                return vm.loggedInUser;
            });
        }

        function setQuery(contractNumber) {
            vm.query = 'contract/contractNumber="' + contractNumber + '"';
        }

        function setSortOrder(orderBy) {
            if (vm.orderBy.indexOf(' desc') === -1) {
                vm.orderBy = vm.orderBy + ' desc';
            } else {
                vm.orderBy = orderBy.replace('desc', '');
            }

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
                includeProperties: 'contract($expand = section),paymentType',
                q: query
            }

            if (vm.customQueryName === 'open') {
                setSearchCriteriaOpenPayments();
            }
            else if (vm.customQueryName === 'missingPdf') {
                setSearchCriteriaMissingPdfs();
            }

            return vm.searchCriteria;
        }


        function setSearchCriteriaMissingPdfs() {
            vm.searchCriteria.q = 'paymentOut!=null and paymentPDF==null and paymentPDFFileId==null';

            // If this user is in the database.
            if (vm.loggedInUser && vm.loggedInUser.sectionId) {
                vm.searchCriteria.q += ' and contract/sectionId=' + vm.loggedInUser.sectionId;
            }

            if (!isTopLevelUser() && vm.loggedInUser) {
                vm.searchCriteria.q += ' and contract/engineerId=' + vm.loggedInUser.engineerId;
            }

            return dataService.searchEntitiesCountOData('payments', vm.searchCriteria).then(function (data) {
                vm.missingPaymentPdf = data || 0;

                return vm.missingPaymentPdf;
            });
        }

        function setSearchCriteriaOpenPayments() {
            vm.searchCriteria.q = 'paymentOut==null';

            // If this user is in the database.
            if (vm.loggedInUser && vm.loggedInUser.sectionId) {
                vm.searchCriteria.q += ' and contract/sectionId=' + vm.loggedInUser.sectionId;
            }

            if (!isTopLevelUser() && vm.loggedInUser) {
                vm.searchCriteria.q += ' and contract/engineerId=' + vm.loggedInUser.engineerId;
            }
        }

        function isTopLevelUser() {
            if (!vm.loggedInUser) {
                return false;
            }

            var result = vm.loggedInUser.sectionChiefRole || vm.loggedInUser.adminRole || vm.loggedInUser.areaEngineerRole || vm.loggedInUser.divisionChiefRole;

            return result;
        }

        function deleteEntity(id) {
            return dataService.deleteEntity(vm.entityDataStore, id)
                .then(function (data) {
                    activate();
                });
        }

        function pageChanged() {
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.query);

            searchEntities(vm.entityDataStore, vm.searchCriteria);
        }
    }

})();