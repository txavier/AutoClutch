(function () {
    'use strict';

    angular
        .module('app')
        .controller('LocationController', LocationController);

    LocationController.$inject = [
        '$scope',
        '$log',
        '$routeParams',
        '$location',
        '$templateCache',
        '$route',
        'dataService',
        'toaster',
        'authenticationService',
        ];

    function LocationController($scope, $log, $routeParams, $location, $templateCache, $route, dataService, toaster, authenticationService) {
        var vm = this;

        vm.entityDataStore = 'locations';
        vm.entity = {};
        vm.addOrUpdate = addOrUpdate;
        vm.sections = [];
        vm.myForm = {};
        vm.setObject = setObject;
        vm.getNewLocation = getNewLocation;

        activate();

        function activate() {
            if ($routeParams.locationId) {
                getEntity($routeParams.locationId);
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

                return vm.entity;
            });
        }

        function getNewLocation(contractId, locationId) {
            return dataService.getNewLocation(parentLocationId, locationId).then(function (data) {
                vm.location.locationId = data;

                return vm.location.locationId;
            });
        }

        function setGeneralName(entity) {
            if (entity.isCfn) {
                entity.generalName = "CFN";
                entity.abbreviation = "CFN";
                return entity;
            }
            if (entity.isCfs) {
                entity.generalName = "CFS";
                entity.abbreviation = "CFS";
                return entity;
            }
            if (entity.other) {
                //entity.generalName = "OTHER";
                //entity.abbreviation = "OTHER";
                //return entity;
            }

            return entity;           
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

            entity = setGeneralName(entity);
            //var tempSection = entity.section;

            //if (tempSection && tempSection.sectionId) {
            //    entity.sectionId = tempSection.sectionId;
            //}
            //else if (tempSection && tempSection.name === "Please select...") {
            //    entity.sectionId = null;
            //}

            //entity.section = null;

            if (entity.locationId) {
                return dataService.updateEntityOData(vm.entityDataStore, entity.locationId, entity, true).then(function () {
                    $location.path('/locations');
                });
            }
            else {
                return dataService.addEntityOData(vm.entityDataStore, entity, true).then(function () {
                    $location.path('/locations');
                });
            }
        }

    }
})();