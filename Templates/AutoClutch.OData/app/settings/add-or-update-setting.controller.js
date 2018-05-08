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
        vm.myForm = {};

        activate();

        function activate() {
            getEntity($routeParams.id);
            
            vm.myForm.$setPristine;

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
            if(!setting.settingId || setting.settingId == 0) {
                return dataService.addEntityOData(vm.entityDataStore, setting, true, setting.settingKey + ' has been updated.').then(function () {
                    $window.history.back();
                });
            } else {
                return dataService.updateEntityOData(vm.entityDataStore, setting.settingId, setting, true, setting.settingKey + ' has been updated.').then(function () {
                    $window.history.back();
                });
            }
        }

    }
})();