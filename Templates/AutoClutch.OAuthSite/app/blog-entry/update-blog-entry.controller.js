(function () {
    'use strict';

    angular
        .module('app')
        .controller('UpdateBlogEntryController', UpdateBlogEntryController);

    UpdateBlogEntryController.$inject = ['$scope', '$routeParams', '$log', '$location', 'toaster', 'dataService'];

    function UpdateBlogEntryController($scope, $routeParams, $log, $location, toaster, dataService) {
        var vm = this;

        vm.entityDataStore = 'blogEntries';
        vm.blogEntry = {};
        vm.updateEntity = updateEntity;

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

                return vm.blogEntry;
            });
        }

        $scope.$watch('vm.blogEntry.fileModel', function (current, original) {
            $log.info('vm.blogEntry.fileModel was changed.');

            if (current) {
                vm.blogEntry.imageBase64String = current.data;
            }
        });

        function updateEntity(id, entity) {
            return dataService.updateEntity(vm.entityDataStore, id, entity).then(function (data) {
                toaster.pop('success', 'Save successful.');

                $location.path('/blog-entries');
            }, function (errors) {
                toaster.pop('warning', 'Unable to continue.');
            });
        }

    }

})();