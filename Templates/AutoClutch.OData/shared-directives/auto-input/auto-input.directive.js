// Click to edit: http://icelab.com.au/articles/levelling-up-with-angularjs-building-a-reusable-click-to-edit-directive/
angular
    .module('shared.directives')
    .directive('xsAutoInput', xsAutoInput);

function xsAutoInput() {
    var directive = {
        restrict: 'EA',
        templateUrl: 'shared-directives/auto-input/auto-input.directive.html',
        require: 'ngModel',
        scope: {
            arrayDisplayKey: '@',
            arrayValueKey: '@',
            model: '=ngModel',
            object: '=',
            ngChange: '&',
            arrayOfObjects: '=',
            editable: '=',
            placeholder: '@',
            innerClass: '@',
            liveSearch: '=',
            title: '@',
            calendar: '@',
            arrayDisplayNum: '@',
            ngDisabled: '=ngDisabled'
        },
        link: link,
        controller: autoInputController,
        controllerAs: 'vm',
        bindToController: true, // because the scope is isolated
    };

    return directive;

    function link(scope, element, attrs, vm) {
    }
}

autoInputController.$inject = ['$scope', '$timeout'];

function autoInputController($scope, $timeout) {
    var vm = this;

    vm.editorEnabled = editorEnabled;
    vm.keyValue = {};
    vm.dropDown = false;
    vm.isCalendar = false;
    vm.updateModel = updateModel;

    activate();

    function activate() {
        setInputType();
    }

    function updateModel(item) {
        vm.ngModel = item;
        $timeout(vm.ngChange, 0);
    }

    function editorEnabled(model, set, cycle) {
        if (set == undefined && vm.keyValue[model] == undefined) {
            vm.keyValue[model] = false;
        }

        if (set !== undefined && cycle == undefined) {
            vm.keyValue[model] = set;
        }
        else if (cycle !== undefined && cycle) {
            vm.keyValue[model] = !vm.keyValue[model];
        }

        return vm.keyValue[model];
    }

    function setInputType() {
        // If the arrayOfObjects is set then this is a drop down.
        if (vm.arrayDisplayKey !== undefined) {
            vm.dropDown = true;
        }

        if (vm.calendar && vm.calendar == 'true') {
            // If this is a date and the model is set by default 
            // to undefined then set it to an empty object which
            // will be a date.
            if (!vm.model) {
                //vm.model = {};
            }

            vm.isCalendar = true;

            vm.dropDown = false;
        }
    }

}