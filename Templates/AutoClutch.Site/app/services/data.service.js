(function () {
    'use strict';

    angular
        .module('app')
        .factory('dataService', dataService);

    dataService.$inject = ['$http', '$log', '$q', 'toaster', 'errorService'];

    function dataService($http, $log, $q, toaster, errorService) {
        var apiUrl = 'api/';

        var service = {
            addEntity: addEntity,
            updateEntity: updateEntity,
            deleteEntity: deleteEntity,
            softDeleteEntity: softDeleteEntity,
            getEntity: getEntity,
            getEntities: getEntities,
            searchEntities: searchEntities,
            searchEntitiesCount: searchEntitiesCount,
            // End of standard methods.
            getLoggedInUser: getLoggedInUser,
            getFileByteStream: getFileByteStream,
            validate: validate,
            getHistory: getHistory,
            getAuthorization: getAuthorization,
            getLatestUserActionLogs: getLatestUserActionLogs,
            // End of semi-standard methods.
            getDashboardMetric: getDashboardMetric,
            getNewWorkOrderNumber: getNewWorkOrderNumber,
            getInitialPayment: getInitialPayment,
            getPaymentTypeProjectRetainage: getPaymentTypeProjectRetainage,
            getContractId: getContractId,
            getDeductionsByContractNumber: getDeductionsByContractNumber,
            getDeductionsByContractNumberCount: getDeductionsByContractNumberCount,
            getInitialContract: getInitialContract,
            getInitialRenewalContract: getInitialRenewalContract,
            getInitialChangeOrder: getInitialChangeOrder,
            addRenewalContract: addRenewalContract,
            getEngineers: getEngineers,
            getScopedPaymentTypes: getScopedPaymentTypes
        };

        return service;

        function getLatestUserActionLogs(take) {
            return $http.get(apiUrl + 'userActionLogs/getLatestUserActionLogs', { params: { take: take } })
                    .then(getLatestUserActionLogsComplete, getLatestUserActionLogsFailed);

            function getLatestUserActionLogsComplete(response) {
                return response.data;
            }

            function getLatestUserActionLogsFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        function getScopedPaymentTypes(contractNumber) {
            return $http.get(apiUrl + 'paymentTypes/getScopedPaymentTypes', { params: { contractNumber: contractNumber }, cache: true })
                    .then(getPaymentTypesComplete, getPaymentTypesFailed);

            function getPaymentTypesComplete(response) {
                return response.data;
            }

            function getPaymentTypesFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        function getAuthorization(loginRequired, requiredPermissions, permissionCheckType, uri, parameters) {
            var methodRoute = 'authorize';

            //if (parameters) {
            //    methodRoute = 'authorize/getWithParams';

            //    return $http.post(apiUrl + methodRoute + '?loginRequired=' + loginRequired + '&requiredPermissions=' + requiredPermissions +
            //        '&permissionCheckType=' + permissionCheckType + '&uri=' + uri, parameters)
            //        .then(getAuthorizationComplete, getAuthorizationFailed);
            //} else {
                return $http.get(apiUrl + methodRoute, {
                    params: {
                        loginRequired: loginRequired,
                        requiredPermissions: requiredPermissions,
                        permissionCheckType: permissionCheckType,
                        uri: uri,
                        parameters: parameters
                    },
                    cache: true
                }).then(getAuthorizationComplete, getAuthorizationFailed);
            //}            

            function getAuthorizationComplete(response) {
                return response.data;
            }

            function getAuthorizationFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        function getEngineers(sectionName) {
            return $http.get(apiUrl + 'engineers/getEngineers', { params: { sectionName: sectionName } })
                    .then(getEngineersComplete, getEngineersFailed);

            function getEngineersComplete(response) {
                return response.data;
            }

            function getEngineersFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        function addRenewalContract(entityDataStore, entity, showToaster, successMessage, failureMessage) {
            return $http.post(apiUrl + entityDataStore + '/addRenewalContract', entity)
                            .then(addRenewalContractComplete, addRenewalContractFailed);

            function addRenewalContractComplete(response) {
                if (showToaster == undefined ? false : showToaster) {
                    toaster.pop('success', 'Saved', successMessage || 'Saved successfully.');
                }

                return response.data;
            }

            function addRenewalContractFailed(error) {
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will
                // have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

        function getHistory(typeFullName, id) {
            return $http.get('api/histories', { params: { typeFullName: typeFullName, id: id }, cache: true })
                        .then(getHistoryCompleted, getHistoryFailed);

            function getHistoryCompleted(response) {
                return response;
            }

            function getHistoryFailed(error) {
                errorService.handleError(error);

                return error;
            }
        }

        function getInitialRenewalContract(originalContractNumber) {
            return $http.get('api/contracts/getInitialRenewalContract', { params: { originalContractNumber: originalContractNumber }, cache: true })
                           .then(getInitialRenewalContractComplete, getInitialRenewalContractFailed);

            function getInitialRenewalContractComplete(response) {
                return response.data;
            }

            function getInitialRenewalContractFailed(error) {
                errorService.handleError(error);

                return error;
            }
        }

        function getInitialContract(sectionName) {
            return $http.get('api/contracts/getInitialContract', { params: { sectionName: sectionName }, cache: true })
                        .then(getInitialContractComplete, getInitialContractFailed);

            function getInitialContractComplete(response) {
                return response.data;
            }

            function getInitialContractFailed(error) {
                errorService.handleError(error);

                return error;
            }
        }

        function getDeductionsByContractNumber(contractNumber, searchCriteria) {
            return $http.get('api/deductions/getDeductionsByContractNumber', {
                params:
                        {
                            contractNumber: contractNumber,
                            page: searchCriteria.currentPage,
                            perPage: searchCriteria.itemsPerPage,
                            sort: searchCriteria.orderBy,
                            search: searchCriteria.searchText,
                            searchFields: searchCriteria.searchTextFields,
                            expand: searchCriteria.includeProperties,
                            q: searchCriteria.q,
                            fields: searchCriteria.fields
                        }
            })
            .then(getDeductionsByContractNumberComplete, getDeductionsByContractNumberFailed);

            function getDeductionsByContractNumberComplete(response) {
                return response.data;
            }

            function getDeductionsByContractNumberFailed(error) {
                errorService.handleError(error);

                return error;
            }
        }

        function getDeductionsByContractNumberCount(contractNumber, searchCriteria) {
            return $http.get('api/deductions/getDeductionsByContractNumber/count', {
                params:
                        {
                            contractNumber: contractNumber,
                            search: searchCriteria.searchText,
                            searchFields: searchCriteria.searchTextFields,
                            q: searchCriteria.q
                        },
            })
            .then(getDeductionsByContractNumberCountComplete, getDeductionsByContractNumberCountFailed);

            function getDeductionsByContractNumberCountComplete(response) {
                return response.data;
            }

            function getDeductionsByContractNumberCountFailed(error) {
                errorService.handleError(error);

                return error;
            }
        }


        function getContractId(contractNumber) {
            return $http.get('api/contracts/getContractId', { params: { contractNumber: contractNumber } })
                        .then(getContractIdCompleted, getContractIdFailed);

            function getContractIdCompleted(response) {
                return response.data;
            }

            function getContractIdFailed(error) {
                errorService.handleError(error, showToaster || true, 'contracts');

                return error;
            }
        }

        function getPaymentTypeProjectRetainage(paymentTypeName, projectRetainage) {
            return $http.get('api/paymentTypes/getPaymentTypeProjectRetainage', { params: { paymentTypeName: paymentTypeName, projectRetainage: projectRetainage } })
                        .then(getPaymentTypeProjectRetainageCompleted, getPaymentTypeProjectRetainageFailed);

            function getPaymentTypeProjectRetainageCompleted(response) {
                return response.data;
            }

            function getPaymentTypeProjectRetainageFailed(error) {
                errorService.handleError(error);

                return error;
            }
        }

        function getInitialPayment(contractId) {
            return $http.get('api/payments/getInitialPayment', { params: { contractId: contractId } })
                        .then(getInitialPaymentCompleted, getInitialPaymentFailed);

            function getInitialPaymentCompleted(response) {
                return response.data;
            }

            function getInitialPaymentFailed(error) {
                errorService.handleError(error, showToaster || true, 'payments', failureMessage);

                return error;
            }
        }

        function getInitialChangeOrder(contractId, changeOrderTypeId) {
            return $http.get('api/changeOrders/getInitialChangeOrder', { params: { contractId: contractId, changeOrderTypeId: changeOrderTypeId } })
                        .then(getInitialChangeOrderCompleted, getInitialChangeOrderFailed);

            function getInitialChangeOrderCompleted(response) {
                return response.data;
            }

            function getInitialChangeOrderFailed(error) {
                errorService.handleError(error, showToaster || true, 'changeOrders', failureMessage);

                return error;
            }
        }

        function getNewWorkOrderNumber(contractId, locationId) {
            return $http.get('api/workOrders/getNewWorkOrderNumber', { params: { contractId: contractId, locationId: locationId }, cache: false })
                        .then(getNewWorkOrderNumberCompleted, getNewWorkOrderNumberFailed);

            function getNewWorkOrderNumberCompleted(response) {
                return response.data;
            }

            function getNewWorkOrderNumberFailed(error) {
                errorService.handleError(error, showToaster || true, 'workOrders', failureMessage);

                return error;
            }
        }

        function getFileByteStream(address, fileId) {
            return $http.get('api/files', { params: { address: address || '', fileId: fileId }, responseType: 'arraybuffer', cache: true })
                        .then(getFileByteStreamCompleted)
                        .catch(getFileByteStreamFailed);

            function getFileByteStreamCompleted(response) {
                return response;
            }

            function getFileByteStreamFailed(error) {
                $log.error('XHR failed for getFileByteStream. '
                 + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                return error;
            }
        }

        function getDashboardMetric(name, loggedInUserId) {
            return $http.get(apiUrl + 'dashboard', { params: { name: name, loggedInUserId: loggedInUserId }, cache: false })
                    .then(getDashboardMetricComplete)
                    .catch(getDashboardMetricFailed);

            function getDashboardMetricComplete(response) {
                return response.data;
            }

            function getDashboardMetricFailed(error) {
                $log.error('XHR failed.'
                    + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                error = undefined; return error;
            }
        }

        function getLoggedInUser() {
            return $http.get('api/engineers/getLoggedInUser', { cache: true })
                        .then(getLoggedInUserCompleted)
                        .catch(getLoggedInUserFailed);

            function getLoggedInUserCompleted(response) {
                return response.data;
            }

            function getLoggedInUserFailed(error) {
                $log.error('XHR failed for sendPickupSessionsEmailMessage. '
                  + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                return error;
            }
        }

        function validate(entityDataStore, fieldName, fieldValue, inspectionEntryId) {
            return $http.get(apiUrl + entityDataStore + '/validate/' + fieldName + '/' + fieldValue + '/' + inspectionEntryId, { ignoreLoadingBar: true })
                        .then(validateComplete)
                        .catch(validateFailed);

            function validateComplete(response) {
                return response.data;
            }

            function validateFailed(error) {
                $log.error('XHR failed for sendPickupSessionsEmailMessage. '
                  + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                return error;
            }
        }

        function getEntity(entityDataStore, id, includeProperties) {
            if (includeProperties) {
                return $http.get(apiUrl + entityDataStore, { params: { id: id, expand: includeProperties }, })
                            .then(getComplete)
                            .catch(getFailed);
            }
            else {
                return $http.get(apiUrl + entityDataStore + (id ? '/' + id : ''))
                                       .then(getComplete)
                                       .catch(getFailed);
            }
            function getComplete(response) {
                return response.data;
            }

            function getFailed(error) {
                $log.error('XHR failed for get ' + entityDataStore + '.'
                    + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                error = undefined; return error;
            }
        }

        function getEntities(entityDataStore, includeProperties) {
            return $http.get(apiUrl + entityDataStore, { params: includeProperties })
                        .then(getEntitiesComplete)
                        .catch(getEntitiesFailed);

            function getEntitiesComplete(response) {
                return response.data;
            }

            function getEntitiesFailed(error) {
                $log.error('XHR failed for get ' + entityDataStore + '.'
                    + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                error = undefined; return error;
            }
        }

        function searchEntities(entityDataStore, searchCriteria, cache) {
            return $http.get(apiUrl + entityDataStore, {
                params:
                        {
                            page: searchCriteria.currentPage,
                            perPage: searchCriteria.itemsPerPage,
                            sort: searchCriteria.orderBy,
                            search: searchCriteria.searchText,
                            searchFields: searchCriteria.searchTextFields,
                            expand: searchCriteria.includeProperties,
                            q: searchCriteria.q,
                            fields: searchCriteria.fields
                        },
                cache: cache == undefined ? false : cache
            })
            .then(searchComplete)
            .catch(searchFailed);

            function searchComplete(response) {
                return response.data;
            }

            function searchFailed(error) {
                $log.error('XHR failed for search ' + entityDataStore + '.'
                    + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                error = undefined; return error;
            }
        }

        function searchEntitiesCount(entityDataStore, searchCriteria, cache) {
            return $http.get(apiUrl + entityDataStore, {
                params:
                        {
                            count: true,
                            search: searchCriteria.searchText,
                            searchFields: searchCriteria.searchTextFields,
                            q: searchCriteria.q
                        },
                cache: cache == undefined ? false : cache
            })
            .then(searchCountComplete)
            .catch(searchCountFailed);

            function searchCountComplete(response) {
                return response.data;
            }

            function searchCountFailed(error) {
                $log.error('XHR failed for searchCount ' + entityDataStore + '.'
                    + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                error = undefined; return error;
            }
        }

        function updateEntity(entityDataStore, id, entity, showToaster, successMessage, failureMessage, ignoreLoadingBar) {
            return $http.put(apiUrl + entityDataStore + '/' + id, entity)
                            .then(updateEntityComplete, updateEntityFailed);

            function updateEntityComplete(response) {
                if (showToaster == undefined ? false : showToaster) {
                    toaster.pop('success', 'Saved', successMessage || 'Saved successfully.');
                }

                return response.data;
            }

            function updateEntityFailed(error) {
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will
                // have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

        function addEntity(entityDataStore, entity, showToaster, successMessage, failureMessage) {
            return $http.post(apiUrl + entityDataStore, entity)
                            .then(addEntityComplete)
                            .catch(addEntityFailed);

            function addEntityComplete(response) {
                if (showToaster == undefined ? false : showToaster) {
                    toaster.pop('success', 'Saved', successMessage || 'Saved successfully.');
                }

                return response.data;
            }

            function addEntityFailed(error) {
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will
                // have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

        function addOrUpdateEntity(entityDataStore, entity, showToaster, successMessage, failureMessage) {
            return $http.post(apiUrl + entityDataStore, entity)
                            .then(addOrUpdateComplete)
                            .catch(addOrUpdateFailed);

            function addOrUpdateComplete(response) {
                if (showToaster == undefined ? true : showToaster) {
                    toaster.pop('success', 'Saved', successMessage || 'Saved successfully.');
                }

                return response.data;
            }

            function addOrUpdateFailed(error) {
                if (showToaster == undefined ? true : (showToaster || failureMessage)) {
                    toaster.pop(
                            {
                                type: 'error',
                                title: 'Problem',
                                timeout: 15000,
                                body: failureMessage || 'Unable to continue, A problem occurred at ' + new Date() + '. '
                                    + ((error.data.ModelState != undefined && error.data.ModelState['']) ? error.data.ModelState[''] : (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : '')),
                                showCloseButton: true
                            });
                }

                $log.error('XHR failed for addOrUpdate ' + entityDataStore + '.'
                    + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                error = undefined; return error;
            }
        }

        function addOrUpdateEntities(entityDataStore, entities, showToaster, successMessage, failureMessage) {
            return $http.post(apiUrl + entityDataStore + '/postEntities', entities)
                            .then(addOrUpdateEntitiesComplete)
                            .catch(addOrUpdateEntitiesFailed);

            function addOrUpdateEntitiesComplete(response) {
                if (showToaster == undefined ? true : showToaster) {
                    toaster.pop('success', 'Saved', successMessage || 'Saved successfully.');
                }

                return response.data;
            }

            function addOrUpdateEntitiesFailed(error) {
                if (showToaster == undefined ? true : showToaster) {
                    toaster.pop(
                            {
                                type: 'error',
                                title: 'Problem',
                                timeout: 15000,
                                body: failureMessage || ('Unable to continue, A problem occurred at ' + new Date() + '. '
                                    + ((error.data.ModelState != undefined && error.data.ModelState['']) ? error.data.ModelState[''] : (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''))),
                                showCloseButton: true
                            });
                }

                $log.error('XHR failed for addOrUpdate ' + entityDataStore + '.'
                    + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                error = undefined; return error;
            }
        }

        function deleteEntity(entityDataStore, id) {
            return $http.delete(apiUrl + entityDataStore + '/' + id)
                        .then(deleteComplete, deleteFailed);

            function deleteComplete(response) {
                return response.data;
            }

            function deleteFailed(error) {
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will
                // have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

        function softDeleteEntity(entityDataStore, id) {
            return $http.delete(apiUrl + entityDataStore + '/' + id, {
                params: {
                    softDelete: true,
                },
            })
            .then(softDeleteComplete, softDeleteFailed);

            function softDeleteComplete(response) {
                return response.data;
            }

            function softDeleteFailed(error) {
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will
                // have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

    }
})();
