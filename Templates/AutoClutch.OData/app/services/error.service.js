// v1.0
(function () {
    'use strict';

    angular
        .module('app')
        .factory('errorService', errorService);

    errorService.$inject = ['$log', '$window', 'toaster'];

    function errorService($log, $window, toaster) {
        var loggedInUser = null;

        var service = {
            handleError: handleError
        };

        return service;

        function getErrorMessage(error) {
            if (error.innerException) {
                var result = getErrorMessage(error.innerException);

                return result;
            }
            else if (error.data && error.data.innerException) {
                var result = getErrorMessage(error.data.innerException);

                return result;
            }
            else if (error.data && error.data.error) {
                var result = getErrorMessage(error.data.error);

                return result;
            }
            else if (error.innererror) {
                var result = getErrorMessage(error.innererror);

                return result;
            }
            else if (error.message) {
                // This must come after innererror because innererrors sometimes contain messages.
                var result = error.message;

                return result;
            }
            else if (error.data && error.data.exceptionMessage) {
                var result = error.data.exceptionMessage;

                return result;
            }
            else if (error.exceptionMessage) {
                var result = error.exceptionMessage;

                return result;
            }
            else if (error.data && error.data.messageDetail) {
                var result = error.data.messageDetail;

                return result;
            }
            else if (error.data && error.data.modelState && error.data.modelState[Object.keys(error.data.modelState)[0]]) {
                var result = error.data.modelState[Object.keys(error.data.modelState)[0]][0];

                return result;
            }
            else if (error.data && error.data.message) {
                var result = error.data.message;

                return result;
            }
            else if (error.data && error.data.Message) {
                var result = error.data.Message;

                return result;
            }
        }

        function handleError(error, showToaster, entityDataStore, failureMessage, logError) {

            var errorMessage = getErrorMessage(error);

            if (showToaster == undefined ? false : (showToaster || failureMessage)) {
                toaster.pop(
						{
						    type: 'error',
						    title: 'Problem',
						    timeout: 15000,
						    body: failureMessage || errorMessage,
						    showCloseButton: true
						});
            }

            if (error.data && error.data.stackTrace) {
                $log.error(error.data.stackTrace);
            }

            if (logError) {
                $window.JL('Angular').fatalException("Exception info", error);
            }

            return error;
        }

    }
})();