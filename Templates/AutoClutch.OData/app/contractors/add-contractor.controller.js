(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddContractorController', AddContractorController);

    AddContractorController.$inject = [
        '$scope',
        '$location',
        'dataService',
        'toaster',
        'authenticationService'
    ];

    function AddContractorController($scope, $location, dataService, toaster, authenticationService) {
        var vm = this;

        vm.entityDataStore = 'contractors';
        vm.entity = {};
        vm.add = add;
        vm.myForm = {};

        activate();

        function activate() {
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
                $location.path('/contractors');
            }, function (error) {
            });
        }

    }
})();