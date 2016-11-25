(function () {
    'use strict';

    angular
        .module('app')
        .controller('BlogEntriesController', BlogEntriesController);

    BlogEntriesController.$inject = ['$scope', '$routeParams', '$sce', 'dataService'];

    function BlogEntriesController($scope, $routeParams, $sce, dataService) {
        var vm = this;

        vm.entityDataStore = 'blogEntries';
        vm.blogEntries = [];
        vm.loggedInUser = {};

        activate();

        function activate() {
            var blogEntriesSearchCriteria = {
                page: 1,
                perPage: 30,
                sort: null,
                search: null,
                searchFields: null,
                includeProperties: 'author',
                q: null,
                fields: null
            };

            getBlogEntries(blogEntriesSearchCriteria).then(getLoggedInUser).then(setEdit);
        }

        function setEdit(loggedInUser) {
            for (var i = 0; i < vm.blogEntries.length; i++) {
                if (loggedInUser.userName == vm.blogEntries[i].author.authorUsername) {
                    vm.blogEntries[i].isAuthorizedEditor = true;
                }
            }
        }

        function getLoggedInUser() {
            return dataService.getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                return vm.loggedInUser;
            });
        }

        function getBlogEntries(searchCriteria) {
            return dataService.searchEntities(vm.entityDataStore, searchCriteria).then(function (data) {
                vm.blogEntries = data;

                for(var i = 0; i < vm.blogEntries.length; i++) {
                    vm.blogEntries[i].safeBlogBodySummaryHtml = $sce.trustAsHtml(vm.blogEntries[i].blogBodySummaryHtml);
                }

                return vm.blogEntries;
            });
        }

    }

})();