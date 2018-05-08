(function () {
	'use strict';

	angular
        .module('app')
        .controller('DeductionsController', DeductionsController);

	DeductionsController.$inject = ['$scope', '$log', '$routeParams', 'dataService'];

	function DeductionsController($scope, $log, $routeParams, dataService) {
		var vm = this;

		vm.entityDataStore = 'deductions';
		vm.entities = [];
		vm.searchEntities = searchEntities;
		vm.softDeleteEntity = softDeleteEntity;
		vm.deleteEntity = deleteEntity;
		vm.totalItems = 0;
		vm.itemsPerPage = 1000;
		vm.currentPage = 1;
		vm.pageChanged = pageChanged;
		vm.setSortOrder = setSortOrder;
		vm.orderBy = 'payment/paymentNumber';
		vm.searchText = null;
		vm.searchCriteria = {};
		vm.searchCriteria.searchText = null;
		vm.includeProperties = 'payment,deductionType,payment($expand=contract)';
		vm.q = null;

		activate();

		function activate() {
			vm.q = 'payment/contract/contractNumber="' + $routeParams.contractNumber + '"';

			setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties, vm.q);

			searchEntities(vm.searchCriteria);

			searchEntitiesCount(vm.searchCriteria);

			return vm;
		}

		function getDeductionsByContractNumber(contractNumber, searchCriteria) {
			return dataService.getDeductionsByContractNumber(contractNumber, searchCriteria).then(function (data) {
				vm.entities = data;

				return vm.entities;
			});
		}

		function getDeductionsByContractNumberCount(contractNumber, searchCriteria) {
			return dataService.getDeductionsByContractNumberCount(contractNumber, searchCriteria).then(function (data) {
				vm.totalItems = data || 0;

				return vm.totalItems;
			});
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

		function softDeleteEntity(id, entityDataStore) {
		    return dataService.softDeleteEntity(entityDataStore || vm.entityDataStore, id)
                .then(function (data) {
                    activate();
                });
		}

		function deleteEntity(id) {
			return dataService.deleteEntity(vm.entityDataStore, id)
                .then(function (data) {
                	searchEntities(vm.searchCriteria);
                });
		}

		function pageChanged() {
			setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties, vm.q);

			searchEntities(vm.searchCriteria);
		}
	}

})();