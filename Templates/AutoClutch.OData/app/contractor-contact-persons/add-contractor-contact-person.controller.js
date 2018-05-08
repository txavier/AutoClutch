(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddContractorContactPersonController', AddContractorContactPersonController);

    AddContractorContactPersonController.$inject = [
        '$scope',
        '$location',
        '$routeParams',
        'dataService',
        'toaster',
        'authenticationService'
    ];

    function AddContractorContactPersonController($scope, $location, $routeParams, dataService, toaster, authenticationService) {
        var vm = this;

        vm.entityDataStore = 'contractorContactPersons';
        vm.entity = {};
        vm.add = add;
        vm.contractorName = '';

        activate();

        function activate() {
            vm.contractorName = $routeParams.contractorName;

            setupInitialContractorContactPerson(vm.contractorName);

            return vm;
        }

        function setupInitialContractorContactPerson(contractorName) {
            return dataService.searchEntitiesOData('contractors', { q: 'name == "' + contractorName + '"' }).then(function (data) {
                vm.entity.contractorId = data[0].contractorId;
            });

            return vm.entity;
        }

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
        }

        function add(entity) {
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

            return dataService.addEntityOData(vm.entityDataStore, entity, true).then(function () {
                $location.path('/contractors/' + vm.contractorName + '/contractor-contact-persons');
            }, function (error) {
            });
        }

    }
})();