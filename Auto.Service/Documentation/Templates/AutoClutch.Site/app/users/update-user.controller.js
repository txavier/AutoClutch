(function () {
    'use strict';

    angular
        .module('app')
        .controller('UpdateUserController', UpdateUserController);

    UpdateUserController.$inject = ['$scope', '$location', '$routeParams', 'dataService', 'toaster'];

    function UpdateUserController($scope, $location, $routeParams, dataService, toaster) {
        var vm = this;

        vm.entityDataStore = 'users';
        vm.updateEntity = updateEntity;
        vm.id = null;
        vm.entity = {};
        vm.updateUserForm = {};

        activate();

        function activate() {
            vm.id = $routeParams.userId;

            getEntity(vm.id);
        }

        function getEntity(id) {
            return dataService.getEntity(vm.entityDataStore, id).then(function (data) {
                vm.entity = data;

                return vm.entity;
            });
        }

        function updateEntity(id, entity) {
            if (!vm.updateUserForm.$valid) {
                var errors = [];

                for (var key in vm.updateUserForm.$error) {
                    for (var index = 0; index < vm.updateUserForm.$error[key].length; index++) {
                        errors.push(vm.updateUserForm.$error[key][index].$name + ' is required.');
                    }
                }

                toaster.pop('warning', 'Information Missing', 'The ' + errors[0]);

                return;
            }

            return dataService.updateEntity(vm.entityDataStore, id, entity).then(function (data) {
                toaster.pop('success', 'Save successful.');

                $location.path('/users');
            }, function (errors) {
                toaster.pop('warning', 'Unable to continue.');
            });
        }
    }

})();