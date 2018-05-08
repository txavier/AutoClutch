// Version 1.0.0
// http://stackoverflow.com/questions/14070285/how-to-implement-history-back-in-angular-js
// http://jsfiddle.net/asgoth/WaRKv/
angular
    .module('shared.directives')
    .directive('boxButton', boxButton);

function boxButton() {
    var directive = {
        restrict: 'EA',
        templateUrl: 'shared-directives/box-button/box-button.directive.html',
        require: 'ngModel',
        scope: {
            title: '@',
            model: '=ngModel',
            ngChange: '&',
            arrayDisplayKey: '@',
            arrayDisplayNum: '@',
            arrayValueKey: '@',
            arrayOfObjects: '=',
            editable: '=',
            liveSearch: '=',
            calendar: '@',
            placeholder: '@',
            innerClass: '@',
            ngDisabled: '=ngDisabled',
            blockedOut: '=',
            bbType: '@',
            copyToClipboard: '=copyToClipboard'
        },
        link: link,
        controller: boxButtonController,
        controllerAs: 'vm',
        bindToController: true // because the scope is isolated
    };

    return directive;

    function link(scope, element, attrs, vm) {
    }
}

boxButtonController.$inject = ['$scope', '$timeout', '$filter'];

function boxButtonController($scope, $timeout, $filter) {
    var vm = this;

    vm.editorEnabled = editorEnabled;
    vm.keyValue = {};
    vm.updateModel = updateModel;
    vm.filterThis = filterThis;
    vm.dropDown = false;
    vm.isCalendar = false;

    activate();

    function activate() {
        vm.ngDisabled = vm.blockedOut ? vm.blockedOut : vm.ngDisabled;

        setInputType();
    }

    function updateModel(item) {
        vm.ngModel = item;
        $timeout(vm.ngChange, 0);
    }

    function filterThis(array, value, display, model) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][value] == model) {
                return array[i][display];
            }
        }
    }

    function editorEnabled(model, set, cycle) {
        if (vm.ngDisabled) {
            return;
        }
        if (!vm.ngDisabled || !vm.blockedOut) {
            if (set == undefined && vm.keyValue[model] == undefined) {
                vm.keyValue[model] = false;
            }

            if (set !== undefined && cycle == undefined) {
                vm.keyValue[model] = set;
            }
            else if (cycle !== undefined && cycle) {
                vm.keyValue[model] = !vm.keyValue[model];
            }
        }

        return vm.keyValue[model];
    }

    function setInputType() {
        // If the arrayOfObjects is set then this is a drop down.
        if (vm.arrayOfObjects !== undefined) {
            vm.dropDown = true;
        }

        if (vm.arrayDisplayKey !== undefined) {
            vm.dropDown = true;
        }

        if (vm.calendar && vm.calendar == 'true') {
            vm.isCalendar = true;

            vm.dropDown = false;
        }
    }

}