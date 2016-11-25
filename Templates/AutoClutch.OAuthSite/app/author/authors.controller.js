(function () {
    'use strict';

    angular
        .module('app')
        .controller('AuthorsController', AuthorsController);

    AuthorsController.$inject = ['$scope', '$routeParams', '$sce', 'dataService'];

    function AuthorsController($scope, $routeParams, $sce, dataService) {
        var vm = this;

        vm.entityDataStore = 'authors';
        vm.authors = [];

        activate();

        function activate() {
            var entitiesSearchCriteria = {
                page: 1,
                perPage: 30,
                sort: null,
                search: null,
                searchFields: null,
                expand: null,
                q: null,
                fields: null
            };

            searchEntities(entitiesSearchCriteria).then(function (data) { vm.authors = data; });
        }

        function searchEntities(searchCriteria) {
            return dataService.searchEntities(vm.entityDataStore, searchCriteria).then(function (data) {
                var entities = data;

                //for(var i = 0; i < vm.blogEntries.length; i++) {
                //    vm.blogEntries[i].safeBlogBodySummaryHtml = $sce.trustAsHtml(vm.blogEntries[i].blogBodySummaryHtml);
                //}

                return entities;
            });
        }

    }

})();