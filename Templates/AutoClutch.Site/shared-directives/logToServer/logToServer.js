﻿// Create new module logToServer with new $log service
angular.module('logToServer', [])
.service('$log', function () {
    this.log = function (msg) {
        JL('Angular').trace(msg);
    }
    this.debug = function (msg) {
        JL('Angular').debug(msg);
    }
    this.info = function (msg) {
        JL('Angular').info(msg);
    }
    this.warn = function (msg) {
        JL('Angular').warn(msg);
    }
    this.error = function (msg) {
        JL('Angular').error(msg);
    }
})
.factory('$exceptionHandler', function () {
    return function (exception, cause) {
        JL('Angular').fatalException(cause, exception);
        throw exception;
    };
})
.factory('logToServerInterceptor', ['$q', function ($q) {
    var myInterceptor = {
        'request': function (config) {
            config.msBeforeAjaxCall = new Date().getTime();
            return config;
        },
        'response': function (response) {
            if (response.config.warningAfter) {
                var msAfterAjaxCall = new Date().getTime();
                var timeTakenInMs =
                      msAfterAjaxCall - response.config.msBeforeAjaxCall;
                if (timeTakenInMs > response.config.warningAfter) {
                    JL('Angular.Ajax').warn({
                        timeTakenInMs: timeTakenInMs,
                        config: response.config,
                        data: response.data
                    });
                }
            }
            return response;
        },
        'responseError': function (rejection) {
            var errorMessage = "timeout";
            if (rejection.status != 0) {
                errorMessage = rejection.data.ExceptionMessage;
            }
            JL('Angular.Ajax').fatalException({
                errorMessage: errorMessage,
                status: rejection.status,
                config: rejection.config
            }, rejection.data);
            return $q.reject(rejection);
        }
    };
    return myInterceptor;
}]);
