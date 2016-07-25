(function () {
    'use strict';

    angular
        .module('app')
        .factory('errorService', errorService);

    errorService.$inject = ['$log', 'toaster'];

    function errorService($log, toaster) {
        var loggedInUser = null;

        var service = {
            handleError: handleError
        };

        return service;

        function handleError(error, showToaster, entityDataStore, failureMessage) {
        	if (showToaster == undefined ? false : (showToaster || failureMessage)) {
        		toaster.pop(
						{
							type: 'error',
							title: 'Problem',
							timeout: 15000,
							body: failureMessage || ((error.data.modelState != undefined && error.data.modelState[Object.keys(error.data.modelState)[0]]) ? error.data.modelState[Object.keys(error.data.modelState)[0]].$values[0] : (error.data ? error.data.message + ' ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message || '') : '')).toString(),
							showCloseButton: true
						});
        	}

        	$log.error('XHR failed for addOrUpdate ' + entityDataStore + ' at ' + new Date() + '. '
				+ (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

        	error = undefined; 

        	return error;
        }

    }
})();