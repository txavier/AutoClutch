(function () {
    'use strict';

    angular
        .module('app')
        .controller('ContractCategoryController', ContractCategoryController);

    ContractCategoryController.$inject = [
        '$scope',
        '$log',
        '$routeParams',
        '$location',
        '$templateCache',
        '$route',
        'dataService',
        'toaster',
        'authenticationService',
        'jsonPointerParseService'];

    function ContractCategoryController($scope, $log, $routeParams, $location, $templateCache, $route, dataService, toaster, authenticationService, jsonPointerParseService) {
        var vm = this;

        vm.entityDataStore = 'contractCategories';
        vm.entity = {};
        vm.addOrUpdate = addOrUpdate;
        vm.sections = [];
        vm.myForm = {};
        vm.setObject = setObject;
        vm.getNewContractCategory = getNewContractCategory;
        //vm.tempNumber = null;
        //vm.tempDesc = null;

        activate();

        function activate() {
            if ($routeParams.contractCategoryId) {
                getEntity($routeParams.contractCategoryId);
            }
            vm.myForm.$setPristine;
            return vm;
        }

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
        }
        function setObject(model, propertyName, propertyNameId, entityArray, id) {
            var result = sharedService.setObject(model, propertyName, propertyNameId, entityArray, id);

            return result;
        }
        function getEntity(id) {
            if (!id) {
                return;
            }

            return dataService.getEntity(vm.entityDataStore, id).then(function (data) {
                vm.entity = data;

                //vm.tempNumber = vm.entity.reportingCategoryNumber;

                //vm.tempDesc = vm.entity.reportingCategoryDescription;

                //vm.entity.reportingCategoryNumber = vm.entity.reportingCategoryNumber.trim();

                //vm.entity.reportingCategoryDescription = vm.entity.reportingCategoryDescription.trim();

                return vm.entity;
            });
        }

        function getNewContractCategory(contractId, contractCategoryId) {
            return dataService.getNewContractCategory(parentContractCategoryId, contractCategoryId).then(function (data) {
                vm.contractCategory.contractCategoryId = data;

                return vm.contractCategory.contractCategoryId;
            });
        }

        function addOrUpdate(entity) {
            if (!vm.myForm.$valid) {
                var errors = [];

                for (var key in vm.myForm.$error) {
                    for (var index = 0; index < vm.myForm.$error[key].length; index++) {
                        errors.push(vm.myForm.$error[key][index].$name + ' is required.');
                    }
                }

                toaster.pop('warning', 'Information Missing', 'The ' + errors[0]);

                return;
            }

            var tempSection = entity.section;

            if (tempSection && tempSection.sectionId) {
                entity.sectionId = tempSection.sectionId;
            }
            else if (tempSection && tempSection.name === "Please select...") {
                entity.sectionId = null;
            }

            entity.section = null;

            if (entity.contractCategoryId) {
                //entity.reportingCategoryNumber = '	' + entity.reportingCategoryNumber + '	';
                //entity.reportingCategoryDescription = '	' + entity.reportingCategoryDescription + '	';

                return dataService.updateEntity(vm.entityDataStore, entity.contractCategoryId, entity, true).then(function () {
                    $location.path('/contractCategories');
                });
            }
            else {
                //entity.reportingCategoryNumber = '	' + entity.reportingCategoryNumber + '	';
                //entity.reportingCategoryDescription = '	' + entity.reportingCategoryDescription + '	';

                return dataService.addEntity(vm.entityDataStore, entity, true).then(function () {
                    $location.path('/contractCategories');
                });
            }
        }

    }
})();