// Handling the downloading of a pdf: http://stackoverflow.com/questions/21628378/angularjs-display-blob-pdf-in-an-angular-app
(function () {
    'use strict';

    angular
        .module('shared.directives')
        .directive('access', access);

    function access() {
        var directive = {
            restrict: 'A',
            //templateUrl: 'app/reports/report.html',
            //scope: {
            //    format: '@',
            //    parameters: '=',
            //    reportServerName: '@',
            //    reportPath: '@'
            //},
            link: link,
            controller: accessController,
            //controllerAs: 'vm',
            //bindToController: true      // Because the scope is isolated.
        };

        return directive;

        function link(scope, element, attrs, vm) {
        }
    }

    accessController.$inject = ['$scope', '$routeParams', '$element', '$attrs', '$location', 'authorizationService'];

    function accessController($scope, $routeParams, $element, $attrs, $location, authorizationService) {
        var vm = this;

        vm.roles = '';
        vm.attrs = $attrs;
        vm.element = $element;
        vm.routeParams = $routeParams;

        activate();

        function activate() {
            vm.roles = vm.attrs.access;

            if (vm.roles.split(',').length > 0) {
                determineVisibility(true);
            }
        }

        function makeEnabled() {
            vm.element.removeClass('disabled');
            vm.element.removeClass('not-active');
            vm.element.removeClass('box1-disabled');
        }

        function makeDisabled() {
            vm.element.addClass('disabled');
            vm.element.addClass('not-active');
            vm.element.addClass('box1-disabled');
        }

        function makeVisible() {
            vm.element.removeClass('hidden');
        }

        function makeHidden() {
            vm.element.addClass('hidden');
        }

        function determineVisibility(resetFirst) {
            var result;
            if (resetFirst) {
                makeVisible();
            }

            var parameters = authorizationService.paramsToKeyValue(vm.routeParams);

            var parameters = authorizationService.paramsToStringKeyValue(vm.routeParams);

            authorizationService.authorize(true, vm.roles, vm.attrs.accessPermissionType, $location.path(), parameters).then(function (data) {
                result = data;

                if (result === 'authorized') {
                    makeVisible();
                } else if (result === 'readOnly') {
                    makeVisible();

                    makeDisabled();
                } else if (result === 'notAuthorized' && (!vm.attrs.accessHidden || vm.attrs.accessHidden === 'false')) {
                    makeDisabled();
                } else if (result === 'notAuthorized' && vm.attrs.accessHidden && vm.attrs.accessHidden === 'true') {
                    makeHidden();
                } else {
                    makeVisible();

                    makeEnabled();
                }

            });
        }

    }

})();