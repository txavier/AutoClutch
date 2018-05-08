(function () {
    'use strict';

    angular
        .module('app')
        .controller('EngineerController', EngineerController);

    EngineerController.$inject = [
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

    function EngineerController($scope, $log, $routeParams, $location, $templateCache, $route, dataService, toaster, authenticationService) {
        var vm = this;

        vm.entityDataStore = 'engineers';
        vm.entity = {};
        vm.addOrUpdate = addOrUpdate;
        vm.sections = [];
        vm.myForm = {};

        activate();

        function activate() {
            if ($routeParams.engineerId) {
                getEntity($routeParams.engineerId)
                    .then(getSections);
            }
            else {
                getSections();
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

        function getSections() {
            return dataService.getEntitiesOData('sections').then(function (data) {
                vm.sections = [{ name: 'Please select...' }];

                vm.sections = vm.sections.concat(data);

                return vm.sections;
            });
        }

        //function initSection() {
        //    vm.entity.section = vm.sections[vm.sections.getIndexBy("sectionId", vm.entity.sectionId)];

        //    return vm.entity.section;
        //}

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
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

            entity.section = null;

            if (entity.engineerId) {
                return dataService.updateEntityOData(vm.entityDataStore, entity.engineerId, entity, true).then(function () {
                    $location.path('/engineers');
                }, function (error) {
                    entity.section = tempSection;
                });
            }
            else {
                return dataService.addEntityOData(vm.entityDataStore, entity, true).then(function () {
                    $location.path('/engineers');
                }, function (error) {
                    entity.section = tempSection;
                });
            }
        }

    }
})();