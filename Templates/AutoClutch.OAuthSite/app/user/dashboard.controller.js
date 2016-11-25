(function () {
    'use strict';

    angular
        .module('app')
        .controller('DashboardController', DashboardController);

    DashboardController.$inject = ['$scope', '$routeParams', '$location', 'dataService'];

    function DashboardController($scope, $routeParams, $location, dataService) {
        var vm = this;

        vm.defaultImageUrl = '../s.discogs.com/images/default-release-cd.png';
        vm.actionFigure = {};

        activate();

        function activate() {
            var actionFigureSearchCriteria = {
                page: 1,
                perPage: 30,
                sort: null,
                search: null,
                searchFields: null,
                expand: null,
                q: null,
                fields: null
            };

            getActionFigures(actionFigureSearchCriteria);
        }

        function getActionFigures(searchCriteria) {
            return dataService.searchEntities('actionFigures', searchCriteria).then(function (data) {
                vm.actionFigures = data;

                return vm.actionFigures;
            });
        }

        function addEntity(actionFigure) {
            return dataService.addEntity('actionFigures', actionFigure).then(function (data) {
                $location.path('/home');
            });
        }

    }

})();