(function () {
    'use strict';

    angular
        .module('app')
        .controller('addOrUpdateSettingController', addOrUpdateSettingController);

    addOrUpdateSettingController.$inject = ['$scope', '$log', '$routeParams', '$location', '$window', 'dataService', 'toaster'];

    function addOrUpdateSettingController($scope, $log, $routeParams, $location, $window, dataService, toaster) {
        var vm = this;

        vm.entityDataStore = 'settings';
        vm.setting = {};
        vm.addOrUpdate = addOrUpdate;

        activate();

        function activate() {
            getEntity($routeParams.id);

            return vm;
        }

        function getEntity(settingId) {
            if (!settingId) {
                return;
            }

            return dataService.getEntity(vm.entityDataStore, settingId).then(function (data) {
                vm.setting = data;

                return vm.setting;
            });
        }

        function addOrUpdate(setting) {
            return dataService.addOrUpdateEntity(vm.entityDataStore, setting, true, setting.settingKey + ' has been updated.').then(function () {
                $window.history.back();
            });
        }

    }
})();