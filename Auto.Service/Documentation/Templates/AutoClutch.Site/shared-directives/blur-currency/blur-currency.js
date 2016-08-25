// http://ghost.scriptwerx.io/angularjs-currency-formatted-input/
(function () {

    'use strict';

    function blurCurrency($filter) {

        function link(scope, el, attrs, ngModelCtrl) {

            function formatter(value) {
                value = value ? parseFloat(value.toString().replace(/[^0-9._-]/g, '')) || 0 : 0;
                var formattedValue = $filter('currency')(value);
                el.val(formattedValue);

                ngModelCtrl.$setViewValue(value);
                //scope.$apply();

                return formattedValue;
            }
            ngModelCtrl.$formatters.push(formatter);

            //el.bind('focus', function () {
            //    //el.val('');
            //});

            el.bind('blur', function () {
                formatter(el.val());
            });
        }

        return {
            require: '^ngModel',
            scope: true,
            link: link
        };
    }
    blurCurrency.$inject = ['$filter'];

    angular
      .module('shared.directives')
      .directive('blurCurrency', blurCurrency);

})();