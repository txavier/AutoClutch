(function () {
    'use strict';

    angular
        .module('app')
        .controller('UsersController', UsersController);

    UsersController.$inject = ['$scope', 'dataService'];

    function UsersController($scope, dataService) {
        var vm = this;

        vm.users = [];
        vm.entityDataStore = 'users';

        activate();

        function activate() {
            getUsers();
        }

        function getUsers() {
            return dataService.getEntities(vm.entityDataStore, null).then(function (data) {
                vm.users = data.$values;

                return vm.users;
            });
        }

    }

})();