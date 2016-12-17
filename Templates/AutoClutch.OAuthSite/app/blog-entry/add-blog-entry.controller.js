(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddBlogEntryController', AddBlogEntryController);

    AddBlogEntryController.$inject = ['$scope', '$routeParams', '$log', '$location', 'toaster', 'dataService'];

    function AddBlogEntryController($scope, $routeParams, $log, $location, toaster, dataService) {
        var vm = this;

        vm.entityDataStore = 'blogEntries';
        vm.blogEntry = {};
        vm.addEntity = addEntity;

        activate();

        function activate() {
        }

        $scope.$watch('vm.blogEntry.fileModel', function (current, original) {
            $log.info('vm.blogEntry.fileModel was changed.');

            if (current) {
                vm.blogEntry.imageBase64String = current.data;
            }
        });

        function addEntity(entity) {
            entity.authorId = 1;

            entity.fileModel = undefined;

            return dataService.addEntity(vm.entityDataStore, entity).then(function (data) {
                toaster.pop('success', 'Save successful.');

                $location.path('/blog-entries');
            }, function (errors) {
                toaster.pop('warning', 'Unable to continue.');
            });
        }

    }

})();