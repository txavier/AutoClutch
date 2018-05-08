(function () {
    'use strict';

    angular
        .module('app')
        .controller('ReportingCategoryController', ReportingCategoryController);

    ReportingCategoryController.$inject = [
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

    function ReportingCategoryController($scope, $log, $routeParams, $location, $templateCache, $route, dataService, toaster, authenticationService, jsonPointerParseService) {
        var vm = this;

        vm.entityDataStore = 'reportingCategories';
        vm.entity = {};
        vm.addOrUpdate = addOrUpdate;
        vm.sections = [];
        vm.myForm = {};
        vm.setObject = setObject;
        vm.getNewReportingCategory = getNewReportingCategory;
        vm.tempNumber = null;
        vm.tempDesc = null;

        activate();

        function activate() {
            if ($routeParams.reportingCategoryId) {
                getEntity($routeParams.reportingCategoryId);
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

                vm.tempNumber = vm.entity.reportingCategoryNumber;

                vm.tempDesc = vm.entity.reportingCategoryDescription;

                vm.entity.reportingCategoryNumber = vm.entity.reportingCategoryNumber.trim();

                vm.entity.reportingCategoryDescription = vm.entity.reportingCategoryDescription.trim();

                return vm.entity;
            });
        }

        function getNewReportingCategory(contractId, reportingCategoryId) {
            return dataService.getNewReportingCategory(parentReportingCategoryId, reportingCategoryId).then(function (data) {
                vm.reportingCategory.reportingCategoryId = data;

                return vm.reportingCategory.reportingCategoryId;
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

            if (entity.reportingCategoryId) {
                entity.reportingCategoryNumber = '	' + entity.reportingCategoryNumber + '	';
                entity.reportingCategoryDescription = '	' + entity.reportingCategoryDescription + '	';

                return dataService.updateEntity(vm.entityDataStore, entity.reportingCategoryId, entity, true).then(function () {
                    $location.path('/reportingCategories');
                });
            }
            else {
                entity.reportingCategoryNumber = '	' + entity.reportingCategoryNumber + '	';
                entity.reportingCategoryDescription = '	' + entity.reportingCategoryDescription + '	';

                return dataService.addEntity(vm.entityDataStore, entity, true).then(function () {
                    $location.path('/reportingCategories');
                });
            }
        }

    }
})();