(function () {
    'use strict';

    angular
        .module('app')
        .controller('searchController', searchController);

    searchController.$inject = ['$scope', '$location', '$log', '$routeParams', '$analytics', 'dataService'];

    function searchController($scope, $location, $log, $routeParams, $analytics, dataService) {
        var vm = this;
        $location.search('set', null);
        $location.search('currentPage', null);

        vm.entityDataStore = 'contracts';
        vm.entities = [];
        vm.searchEntities = searchEntities;
        vm.deleteEntity = deleteEntity;
        vm.totalItems = 0;
        vm.itemsPerPage = 25;
        vm.currentPage = 1;
        vm.pageChanged = pageChanged;
        vm.setSortOrder = setSortOrder;
        vm.orderBy = null;
        vm.searchText = '';
        vm.searchCriteria = {};
        vm.searchCriteria.searchText = null;
        vm.filter = null;
        vm.output = '';
        vm.includeProperties = null,
        vm.json = '';
        vm.entityDataStores = [];
        vm.changeFields = changeFields;
        vm.search = search;
        vm.entityDataStores = [
              {
                  id: 'contracts',
                  name: 'Contracts', 
                  displayProperty: 'contractNumber',
                  url: '/:sectionName/contracts/:contractNumber',
                  includeProperties: 'section',
                  fields: [
                      { id: 'contractNumber', name: 'contractNumber', type: 'string' },
                      { id: 'engineer.name', name: 'engineer.name', type: 'string' }
                  ]
              },
              {
                  id: 'changeOrders',
                  name: 'ChangeOrders',
                  displayProperty: 'description', 
                  url: '/:sectionName/contracts/:contractNumber/change-orders/:changeOrderId',
                  includeProperties: 'changeOrderType,contract.section',
                  fields: [
                      { id: 'changeOrderNumber', name: 'changeOrderNumber', type: 'string'},
                      { id: 'changeOrderType.name', name: 'changeOrderType.name', type: 'string'}
                  ]
              },
              {
                  id: 'payments',
                  name: 'Payments',
                  displayProperty: 'paymentNumber', 
                  url: '/:sectionName/contracts/:contractNumber/payments/:paymentId',
                  includeProperties: 'contract.section',
                  fields: [
                      { id: 'paymentNumber', name: 'Payment Number', type: 'string' },
                  ]
              },
              {
                  id: 'workOrders',
                  name: 'Work Orders',
                  displayProperty: 'workOrderNumber',
                  url: '/:sectionName/contracts/:contractNumber/work-orders/:workOrderNumber',
                  includeProperties: 'contract.section', 
                  fields: [
                       { id: 'workOrderNumber', name: 'workOrderNumber', type: 'string' },
                       { id: 'contract.engineer.name', name: 'Engineer', type: 'string' },
                       { id: 'workOrderStatus.name', name: 'Work Order Status', type: 'string' },
                       { id: 'cmmsWorkOrderNumber', name: 'CMMS Work Order Number', type: 'string' }
                  ]
              }
        ];
        vm.fields = vm.entityDataStores[0].fields;
        vm.getDisplayProperty = getDisplayProperty;
        vm.getUrl = getUrl;
        vm.getIndexBy = getIndexBy;

        activate();

        function activate() {
            vm.searchText = $routeParams.searchText;
            vm.entityDataStore = $routeParams.entityDataStore || vm.entityDataStore;
            vm.output = $routeParams.q || vm.output;

            setFilter();
            search(false);

            return vm;
        }

        function getUrl(entityDataStoreId, entity) {
            switch (entityDataStoreId) {
                case 'contracts': {
                    if (entity.contractId) {
                        var url = vm.entityDataStores[vm.entityDataStores.getIndexBy('id', entityDataStoreId)].url;

                        url = url.replace(':contractNumber', entity['contractNumber']);

                        url = url.replace(':sectionName', entity['section']['name']);
                    }

                    break;
                }
                case 'workOrders': {
                    if (entity.workOrderId) {
                        var url = vm.entityDataStores[vm.entityDataStores.getIndexBy('id', entityDataStoreId)].url;

                        url = url.replace(':contractNumber', entity['contract']['contractNumber']);

                        url = url.replace(':sectionName', entity['contract']['section']['name']);

                        url = url.replace(':workOrderNumber', entity['workOrderNumber']);
                    }

                    break;
                }
                case 'payments': {
                    if (entity.paymentId) {
                        var url = vm.entityDataStores[vm.entityDataStores.getIndexBy('id', entityDataStoreId)].url;

                        url = url.replace(':contractNumber', entity['contract']['contractNumber']);

                        url = url.replace(':sectionName', entity['contract']['section']['name']);

                        url = url.replace(':paymentId', entity['paymentId']);
                    }

                    break;
                }
                case 'changeOrders': {
                    if (entity.changeOrderId) {
                        var url = vm.entityDataStores[vm.entityDataStores.getIndexBy('id', entityDataStoreId)].url;

                        url = url.replace(':contractNumber', entity['contract']['contractNumber']);

                        url = url.replace(':sectionName', entity['contract']['section']['name']);

                        url = url.replace(':changeOrderId', entity['changeOrderId']);
                    }

                    break;
                }
            }

            return url;
        }

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
        }

        function getIndexBy(array, name, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i][name] == value) {
                    return i;
                }
            }
        }

        function getDisplayProperty(entityDataStoreId) {
            var result = vm.entityDataStores[vm.entityDataStores.getIndexBy('id', entityDataStoreId)].displayProperty;

            return result;
        }

        function getIncludeProperties(entityDataStoreId) {
            var result = vm.entityDataStores[vm.getIndexBy(vm.entityDataStores, 'id', entityDataStoreId)].includeProperties;

            result = result || vm.includeProperties;

            return result;
        }

        function setFirstUrlPart() {
            if (entityDataStore === 'contracts') {
                vm.firstUrlPart = vm.entities[0].sectionName + '/' + entityDataStore;
            }
        }

        function search(all) {
            if (all) {
                for (var i = 0; i < vm.entityDataStores.length; i++) {
                    vm.entityDataStore = vm.entityDataStores[i].id;

                    setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties, vm.output);

                    vm.entities = [];

                    searchEntities(vm.searchCriteria, true);

                    searchEntitiesCount(vm.searchCriteria, true);
                }
            }
            else {
                setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText, vm.includeProperties, vm.output);

                searchEntities(vm.searchCriteria);

                searchEntitiesCount(vm.searchCriteria);
            }
        }

        function changeFields(entityDataStoreId) {
            vm.fields = vm.entityDataStores[vm.getIndexBy(vm.entityDataStores, 'id', entityDataStoreId)].fields;

            return vm.fields;
        }

        function setFilter() {
            var data = '{"group": {"operator": "and","rules": []}}';

            vm.json = null;

            vm.filter = JSON.parse(data);

            $scope.$watch('vm.filter', function (newValue) {
                vm.json = JSON.stringify(newValue, null, 2);
                vm.output = computed(newValue.group);
                vm.output = vm.output === '()' ? null : vm.output;
            }, true);
        }

        function htmlEntities(str) {
            return String(str).replace(/</g, '&lt;').replace(/>/g, '&gt;');
        }

        function filterInt(value) {
            if (/^(\-|\+)?([0-9]+|Infinity)$/.test(value))
                return Number(value);
            return NaN;
        }

        function getCondition(condition) {
            if (condition == "contains") {
                return ".contains(";
            } else {
                return condition;
            }
        }

        function requiresEndParenthesis(condition) {
            if (condition == "contains") {
                return true;
            } else {
                return false;
            }
        }

        function computed(group) {
            if (!group) return "";
            for (var str = "(", i = 0; i < group.rules.length; i++) {
                i > 0 && (str += " " + group.operator + " ");
                str += group.rules[i].group ?
                    computed(group.rules[i].group) :
                    group.rules[i].field.id + htmlEntities(getCondition(group.rules[i].condition)) + 
                        (group.rules[i].field.type !== 'string' ? group.rules[i].data : ("\"" + group.rules[i].data + "\""))
                        + (requiresEndParenthesis(group.rules[i].condition) ? ")" : "");
            }

            return str + ")";
        }

        function setSortOrder(orderBy) {
             if (vm.orderBy.indexOf(' desc') === -1) {
                vm.orderBy = vm.orderBy + ' desc';
            } else {
                vm.orderBy = orderBy.replace('desc', '');
            }

            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText);

            searchEntities(vm.searchCriteria);
        }

        function searchEntities(searchCriteria, all) {
            return dataService.searchEntitiesOData(vm.entityDataStore, searchCriteria).then(function (data) {
                if (all) {
                    vm.entities = vm.entities.concat(data);
                }
                else {
                    vm.entities = data;
                }

                return vm.entities;
            });
        }

        function searchEntitiesCount(searchCriteria, all) {
            return dataService.searchEntitiesCountOData(vm.entityDataStore, searchCriteria).then(function (data) {
                if (all) {
                    vm.totalItems += data || 0;
                }
                else {
                    vm.totalItems = data || 0;
                }

                $analytics.trackSiteSearch(vm.searchText, vm.entityDataStore, vm.totalItems);

                return vm.totalItems;
            });
        }

        function setSearchCriteria(currentPage, itemsPerPage, orderBy, searchText, includeProperties, q) {
            vm.searchCriteria = {
                currentPage: currentPage,
                itemsPerPage: itemsPerPage,
                orderBy: orderBy,
                searchText: searchText,
                includeProperties: getIncludeProperties(vm.entityDataStore),
                q: q
            }

            return vm.searchCriteria;
        }

        function deleteEntity(id) {
            return dataService.deleteEntity(vm.entityDataStore, id)
                .then(function (data) {
                    searchEntities(vm.searchCriteria);
                });
        }

        function pageChanged() {
            setSearchCriteria(vm.currentPage, vm.itemsPerPage, vm.orderBy, vm.searchText);

            searchEntities(vm.searchCriteria);
        }
    }

})();