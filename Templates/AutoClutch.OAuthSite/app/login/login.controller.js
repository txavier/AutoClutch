(function () {
    'use strict';

    angular
        .module('app')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['$scope', '$routeParams', 'dataService'];

    function LoginController($scope, $routeParams, dataService) {
        var vm = this;

        vm.defaultImageUrl = '../s.discogs.com/images/default-release-cd.png';
        vm.actionFigures = [
            {
                name: 'JetStorm1',
                make: { name: 'Takara' },
                imageUrl: '../s.discogs.com/file-store/TAK12274.jpg'
            },
            {
                name: 'Han Solo1',
                make: { name: 'Kenner'},
                imageUrl: vm.defaultImageUrl
            },
            {
                name: 'Ironhide1',
                make: { name: 'Takara' },
                imageUrl: vm.defaultImageUrl
            },
            {
                name: 'Iron Man Mark VIIIi',
                make: { name: 'Marvel' },
                imageUrl: '../s.discogs.com/file-store/BAN15824.jpg'
            }];

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

    }

})();