(function () {
    'use strict';

    angular
        .module('app')
        .controller('ViewAuthorController', ViewAuthorController);

    ViewAuthorController.$inject = ['$scope', '$routeParams', '$log', '$location', 'toaster', 'dataService'];

    function ViewAuthorController($scope, $routeParams, $log, $location, toaster, dataService) {
        var vm = this;

        vm.entityDataStore = 'authors';
        vm.returnPath = '/authors';
        vm.idName = 'authorId';
        vm.author = {};

        activate();

        function activate() {
            var entitiesSearchCriteria = {
                page: 1,
                perPage: 30,
                sort: null,
                search: null,
                searchFields: null,
                expand: null,
                q: vm.idName + ' eq ' + $routeParams[vm.idName],
                fields: null
            };

            searchEntities(entitiesSearchCriteria, vm.author);
        }

        function searchEntities(searchCriteria, entity) {
            return dataService.searchEntities(vm.entityDataStore, searchCriteria).then(function (data) {
                entity = data[0];

                return entity;
            });
        }

    }

})();