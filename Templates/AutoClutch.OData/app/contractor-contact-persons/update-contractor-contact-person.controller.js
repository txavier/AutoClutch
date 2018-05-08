(function () {
    'use strict';

    angular
        .module('app')
        .controller('UpdateContractorContactPersonController', UpdateContractorContactPersonController);

    UpdateContractorContactPersonController.$inject = [
        '$scope',
        '$routeParams',
        '$location',
        'dataService',
        'toaster',
        'authenticationService'
    ];

    function UpdateContractorContactPersonController($scope, $routeParams, $location, dataService, toaster, authenticationService) {
        var vm = this;

        vm.entityDataStore = 'contractorContactPersons';
        vm.entity = {};
        vm.update = update;
        vm.entityId = null;
        vm.contractorName = '';
        vm.myForm = {};

        activate();

        function activate() {
            vm.entityId = $routeParams.contractorContactPersonId;

            vm.contractorName = $routeParams.contractorName;

            getEntity(vm.entityId);

            return vm;
        }

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
                $location.path('/contractors/' + vm.contractorName + '/contractor-contact-persons');
            }, function (error) {
            });

        }

    }
})();