(function () {
    'use strict';

    angular
        .module('app')
        .controller('historyController', historyController);

    historyController.$inject = ['$scope', '$log', '$window', '$http', '$sce', '$routeParams', 'dataService', 'toaster', 'authenticationService'];

    function historyController($scope, $log, $window, $http, $sce, $routeParams, dataService, toaster, authenticationService) {
        var vm = this;

        vm.typeFullName = '';
        vm.id = '';
        vm.objectHistories = [];

        activate();

        function activate() {
            vm.typeFullName = $routeParams.typeFullName;

            vm.id = $routeParams.id

            getHistory(vm.typeFullName, vm.id);
        }

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
        }

        function getHistory(typeFullName, id) {
            return dataService.getHistory(typeFullName, id).then(function (response) {
                    vm.objectHistories = response.data.$values;
                });

            return vm;
        }

    }

})();