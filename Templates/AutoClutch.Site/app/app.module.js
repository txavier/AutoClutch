(function () {
    'use strict';

    angular
    .module('app', [
        'ngRoute'
        , 'shared.directives'
        , 'nya.bootstrap.select'
        , 'ui.bootstrap'
        , 'angular-loading-bar'
        , 'toaster'
        , 'ngAnimate'
        , 'ui.bootstrap.datetimepicker'
        , 'chart.js'
        , 'ngFileUpload'
    ]);

    angular
        .module('app')
        .filter('escape', escapeFilter)
        .filter('tel', telFilter)
        .run(runBlock);
        
    runBlock.$inject = ['authorizationService'];

    function runBlock(authorizationService) {
        authorizationService.initialize();
    }

    function escapeFilter() {
        return window.encodeURIComponent;
    };

    // http://jsfiddle.net/jorgecas99/S7aSj/
    function telFilter() {
        return function (tel) {
            if (!tel) { return ''; }

            var value = tel.toString().trim().replace(/^\+/, '');

            if (value.match(/[^0-9]/)) {
                return tel;
            }

            var country, city, number;

            switch (value.length) {
                case 10: // +1PPP####### -> C (PPP) ###-####
                    country = 1;
                    city = value.slice(0, 3);
                    number = value.slice(3);
                    break;

                case 11: // +CPPP####### -> CCC (PP) ###-####
                    country = value[0];
                    city = value.slice(1, 4);
                    number = value.slice(4);
                    break;

                case 12: // +CCCPP####### -> CCC (PP) ###-####
                    country = value.slice(0, 3);
                    city = value.slice(3, 5);
                    number = value.slice(5);
                    break;

                default:
                    return tel;
            }

            if (country == 1) {
                country = "";
            }

            number = number.slice(0, 3) + '-' + number.slice(3);

            return (country + " (" + city + ") " + number).trim();
        }
    };

})();