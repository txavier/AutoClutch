(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddAuthorController', AddAuthorController);

    AddAuthorController.$inject = ['$scope', '$routeParams', '$log', '$location', 'toaster', 'dataService'];

    function AddAuthorController($scope, $routeParams, $log, $location, toaster, dataService) {
        var vm = this;

        vm.entityDataStore = 'authors';
        vm.returnPath = '/authors';
        vm.author = {};
        vm.addEntity = addEntity;

        activate();

        function activate() {
        }

        $scope.$watch('vm.author.fileModel', function (current, original) {
            $log.info('vm.author.fileModel was changed.');

            if (current) {
                vm.author.imageBase64String = current.data;
            }
        });

        function addEntity(entity) {
            entity.fileModel = undefined;

            return dataService.addEntity(vm.entityDataStore, entity).then(function (data) {
                toaster.pop('success', 'Save successful.');

                $location.path(vm.returnPath);
            }, function (errors) {
                toaster.pop('warning', 'Unable to continue.');
            });
        }

    }

})();