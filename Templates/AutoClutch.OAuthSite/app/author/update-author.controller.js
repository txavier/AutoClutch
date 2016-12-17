(function () {
    'use strict';

    angular
        .module('app')
        .controller('UpdateAuthorController', UpdateAuthorController);

    UpdateAuthorController.$inject = ['$scope', '$routeParams', '$log', '$location', 'toaster', 'dataService'];

    function UpdateAuthorController($scope, $routeParams, $log, $location, toaster, dataService) {
        var vm = this;

        vm.entityDataStore = 'authors';
        vm.returnPath = '/authors';
        vm.idName = 'authorId';
        vm.author = {};
        vm.updateEntity = updateEntity;

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

            searchEntities(entitiesSearchCriteria).then(function (data) { vm.author = data; });
        }

        function searchEntities(searchCriteria) {
            return dataService.searchEntities(vm.entityDataStore, searchCriteria).then(function (data) {
                var entity = data[0];

                return entity;
            });
        }

        $scope.$watch('vm.author.fileModel', function (current, original) {
            $log.info('vm.author.fileModel was changed.');

            if (current) {
                vm.author.imageBase64String = current.data;
            }
        });

        function updateEntity(id, entity) {
            return dataService.updateEntity(vm.entityDataStore, id, entity).then(function (data) {
                toaster.pop('success', 'Save successful.');

                $location.path(vm.returnPath);
            }, function (errors) {
                toaster.pop('warning', 'Unable to continue.');
            });
        }

    }

})();