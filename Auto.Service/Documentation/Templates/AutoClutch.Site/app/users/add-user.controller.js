(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddUserController', AddUserController);

    AddUserController.$inject = ['$scope', '$location', 'dataService', 'toaster'];

    function AddUserController($scope, $location, dataService, toaster) {
        var vm = this;

        vm.entityDataStore = 'users';
        vm.addEntity = addEntity;
        vm.entity = {};
        vm.addUserForm = {};

        activate();

        function activate() {
        }

        function addEntity(entity) {
            if (!vm.addUserForm.$valid) {
                var errors = [];

                for (var key in vm.addUserForm.$error) {
                    for (var index = 0; index < vm.addUserForm.$error[key].length; index++) {
                        errors.push(vm.addUserForm.$error[key][index].$name + ' is required.');
                    }
                }

                toaster.pop('warning', 'Information Missing', 'The ' + errors[0]);

                return;
            }

            return dataService.addEntity(vm.entityDataStore, entity).then(function (data) {
                toaster.pop('success', 'Save successful.');

                $location.path('/users');
            }, function (errors) {
                toaster.pop('warning', 'Unable to continue.');
            });
        }
    }

})();