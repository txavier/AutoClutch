angular
    .module('shared.directives', [
        'ngRoute',
        //,'ngResource'
        //,'ui.bootstrap'
        , 'ChartAngular'
        , 'utils.autofocus'
        , 'logToServer'
        //, 'datetime'
    ]);

angular
    .module('shared.directives')
    .config(config);

function config($routeProvider, $locationProvider) {
}

// ELMAH JSNLog for error and exception logging in ELMAH
angular
    .module('shared.directives')
    .config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('logToServerInterceptor');
    }]);