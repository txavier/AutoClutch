(function () {
    angular
        .module('app')
        .controller('UpdateActionFigureController', UpdateActionFigureController);

    UpdateActionFigureController.$inject = ['$scope', '$routeParams', '$location', 'dataService'];

    function UpdateActionFigureController($scope, $routeParams, $location, dataService) {
        var vm = this;

        vm.defaultImageUrl = '../s.discogs.com/images/default-release-cd.png';
        vm.actionFigure = {};
        vm.actionFigureId = null;

        activate();

        function activate() {
            vm.actionFigureId = $routeParams.actionFigureId;

            if (vm.actionFigureId) {
                var actionFigureSearchCriteria = {
                    page: 1,
                    perPage: 30,
                    sort: null,
                    search: null,
                    searchFields: null,
                    expand: null,
                    q: 'actionFigureId == ' + vm.actionFigureId,
                    fields: null
                };

                getActionFigures(actionFigureSearchCriteria);
            }
        }

        function getActionFigures(searchCriteria) {
            return dataService.searchEntities('actionFigures', searchCriteria).then(function (data) {
                vm.actionFigure = data.$values[0];

                return vm.actionFigure;
            });
        }

        function updateEntity(actionFigure) {
            return dataService.updateEntity('actionFigures', actionFigure.actionFigureId, actionFigure).then(function (data) {
                $location.path('/home');
            });
        }

    }

})();