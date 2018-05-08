(function () {
    'use strict';

    /**
    * @desc Contract details directive for use above pages where the contract information is needed.
    * @example <dep-contract-details data-dep-contract-number="contractNumber"></dep-contract-details>
    */
    angular
        .module('shared.directives')
        .directive('moDateInput', moDateInput);

    function moDateInput() {
        var directive = {
            require: '^ngModel',
            restrict: 'A',
            link: link
        };

        return directive;

        function link(scope, element, attrs, ctrl) {
            var dateFormat = attrs.moDateInput;
            attrs.$observe('moDateInput', function (newValue) {
                if (dateFormat == newValue || !ctrl.$modelValue) return;
                dateFormat = newValue;
                ctrl.$modelValue = new Date(ctrl.$setViewValue);
            });

            ctrl.$formatters.unshift(function (modelValue) {
                scope = scope;
                if (!dateFormat || !modelValue) return "";
                var retVal = moment(modelValue).format(dateFormat);
                return retVal;
            });

            ctrl.$parsers.unshift(function (viewValue) {
                scope = scope;
                var date = moment(viewValue, dateFormat);
                return (date && date.isValid() && date.year() > 1950) ? date.toDate() : "";
            });
        }
    }

    moDateInputController.$inject = ['$scope'];

    function moDateInputController($scope) {
        var vm = this;

        activate();

        function activate() {
        }
    }

})();