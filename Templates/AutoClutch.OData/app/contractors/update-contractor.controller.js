(function () {
    'use strict';

    angular
        .module('app')
        .controller('UpdateContractorController', UpdateContractorController);

    UpdateContractorController.$inject = [
        '$scope',
        '$routeParams',
        '$location',
        'dataService',
        'toaster',
        'authenticationService'
    ];

    function UpdateContractorController($scope, $routeParams, $location, dataService, toaster, authenticationService) {
        var vm = this;

        vm.entityDataStore = 'contractors';
        vm.entity = {};
        vm.update = update;
        vm.entityId = null;
        vm.myForm = {};

        activate();

        function activate() {
            getEntity($routeParams.contractorName);
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

        function getEntity(name) {
            if (!name) {
                return;
            }

            var searchCriteria = { q: 'name="' + name + '"' };

            return dataService.searchEntitiesOData(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.entity = data[0];

                vm.entityId = vm.entity.contractorId;

                return vm.entity;
            });
        }

        function update(entity) {
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

            return dataService.updateEntityOData(vm.entityDataStore, vm.entityId, entity, true).then(function () {
                $location.path('/contractors');
            }, function (error) {
            });

        }

    }
})();