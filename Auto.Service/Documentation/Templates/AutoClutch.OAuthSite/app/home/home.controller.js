(function () {
    angular
        .module('app')
        .controller('HomeController', HomeController);

    HomeController.$inject = ['$scope', '$routeParams', 'dataService'];

    function HomeController($scope, $routeParams, dataService) {
        var vm = this;

        vm.defaultImageUrl = '../s.discogs.com/images/default-release-cd.png';
        vm.actionFigure = {};

        activate();

        function activate() {
            vm.actionFigureId = $routeParams.actionFigureId;

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

        function getActionFigures(searchCriteria) {
            return dataService.searchEntities('actionFigures', searchCriteria).then(function (data) {
                vm.actionFigures = data;

                return vm.actionFigures;
            });
        }

    }

})();