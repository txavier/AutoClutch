(function () {
    'use strict';

    angular
        .module('shared.directives')
        .directive('depHomeProcurement', depHomeProcurement);

    function depHomeProcurement() {
        var directive = {
            restrict: 'EA',
            templateUrl: 'app/home/home-procurement.html',
            scope: {},
            link: link,
            controller: homeProcurementController,
            controllerAs: 'vm',
            bindToController: true // because the scope is isolated
        };

        return directive;

        function link(scope, element, attrs, vm) {
        }
    }

    homeProcurementController.$inject = ['$scope', '$log', 'dataService', 'authenticationService'];

    function homeProcurementController($scope, $log, dataService, authenticationService) {
        var vm = this;
        
        vm.loggedInUser = {};
        vm.procurementReceivingReportsCount = null;
        vm.searchProcurementReceivingReportsCount = searchProcurementReceivingReportsCount;

        activate();

        function activate() {
            getLoggedInUser().then(function (data) {
                searchProcurementReceivingReportsCount();
            });
        }

        function getLoggedInUser() {
            return authenticationService.getLoggedInUser().then(function (data) {
                vm.loggedInUser = data;

                return vm.loggedInUser;
            });
        }

        function searchProcurementReceivingReportsCount() {
            return dataService.getDashboardMetric('searchProcurementReceivingReportsCount', vm.loggedInUser.engineerId).then(function (data) {
                vm.procurementReceivingReportsCount = data;

                return vm.procurementReceivingReportsCount;
            });
        }

    }

})();