(function () {
    'use strict';

    angular
        .module('app')
        .controller('ViewBlogEntryController', ViewBlogEntryController);

    ViewBlogEntryController.$inject = ['$scope', '$routeParams', '$sce', 'dataService'];

    function ViewBlogEntryController($scope, $routeParams, $sce, dataService) {
        var vm = this;

        vm.entityDataStore = 'blogEntries';
        vm.blogEntries = [];

        activate();

        function activate() {
            var blogEntriesSearchCriteria = {
                page: 1,
                perPage: 30,
                sort: null,
                search: null,
                searchFields: null,
                includeProperties: 'author',
                q: 'blogEntryId eq ' + $routeParams.blogEntryId,
                fields: null
            };

            getBlogEntries(blogEntriesSearchCriteria);
        }

        function getBlogEntries(searchCriteria) {
            return dataService.searchEntities(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.blogEntry = data[0];

                vm.blogEntry.safeBlogBodyHtml = $sce.trustAsHtml(vm.blogEntry.blogBodyHtml);

                return vm.blogEntries;
            });
        }

    }

})();