(function () {
    'use strict';

    angular
        .module('app')
        .controller('AboutController', AboutController);

    AboutController.$inject = ['$scope', '$routeParams', 'dataService'];

    function AboutController($scope, $routeParams, dataService) {
        var vm = this;

        vm.version = null;

        activate();

        function activate() {
            getVersion();
        }

        function getVersion() {
            dataService.getVersion().then(function (data) {
                vm.version = data;
            });
        }

    }
})();
