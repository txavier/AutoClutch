(function () {
    'use strict';

    angular
        .module('app')
        .controller('LayoutController', LayoutController);

    LayoutController.$inject = ['$scope', '$routeParams', 'dataService'];

    function LayoutController($scope, $routeParams, dataService) {
        var vm = this;

        vm.user = null;

        activate();

        function activate() {
            getLoggedInUser();
        }

        function getLoggedInUser() {
            return dataService.getLoggedInUser().then(function (data) {
                vm.user = data.value;

                return vm.user;
            });
        }

    }

})();