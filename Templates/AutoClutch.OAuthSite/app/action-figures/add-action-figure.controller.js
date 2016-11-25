(function () {
    angular
        .module('app')
        .controller('AddActionFigureController', AddActionFigureController);

    AddActionFigureController.$inject = ['$scope', '$routeParams', '$location', 'dataService'];

    function AddActionFigureController($scope, $routeParams, $location, dataService) {
        var vm = this;

        vm.defaultImageUrl = '../s.discogs.com/images/default-release-cd.png';
        vm.actionFigure = {};
        vm.addEntity = addEntity;

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
        }

        function addEntity(actionFigure) {
            return dataService.addEntity('actionFigures', actionFigure).then(function (data) {
                $location.path('/home');
            });
        }

    }

})();