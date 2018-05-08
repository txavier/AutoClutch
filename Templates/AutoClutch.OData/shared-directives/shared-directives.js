angular
    .module('shared.directives', [
        'ngRoute',
        //,'ngResource'
        //,'ui.bootstrap'
        , 'ChartAngular'
        , 'utils.autofocus'
        , 'ngclipboard'
    ]);

angular
    .module('shared.directives')
    .config(config)
    .filter('tel', telFilter);

function config($routeProvider, $locationProvider) {
}

// ELMAH JSNLog for error and exception logging in ELMAH
angular
    .module('shared.directives')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('logToServerInterceptor');
    }]);


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