(function () {
    'use strict';

    angular
        .module('app')
        .factory('dataService', dataService);

    dataService.$inject = ['$http', '$log', '$q', 'toaster', 'errorService'];

    // Version 1.0.1
    function dataService($http, $log, $q, toaster, errorService) {
        var apiUrl = 'api/';

        var odataUrl = 'odata/';

        var service = {
            addEntity: addEntity,
            updateEntity: updateEntity,
            deleteEntity: deleteEntity,
            softDeleteEntity: softDeleteEntity,
            getEntity: getEntity,
            getEntities: getEntities,
            searchEntities: searchEntities,
            searchEntitiesCount: searchEntitiesCount,
            searchEntitiesApi: searchEntitiesApi,
            searchEntitiesCountApi: searchEntitiesCountApi,
            // End of standard methods.
            getLoggedInUser: getLoggedInUser,
            getMe: getMe,
            getFileByteStream: getFileByteStream,
            validate: validate,
            getHistory: getHistory,
            getAuthorization: getAuthorization,
            getLatestUserActionLogs: getLatestUserActionLogs,
            // End of semi-standard methods.
            getDashboardMetric: getDashboardMetric,
            // Application specific methods.
            getGeoClientInformation: getGeoClientInformation,
            getVersion: getVersion
        };

        return service;

        function addEntity(entityDataStore, entity, showToaster, successMessage, failureMessage) {
            return $http.post(odataUrl + entityDataStore, entity).then(addEntityComplete, addEntityFailed);

            function addEntityComplete(response) {
                if (showToaster == undefined ? false : showToaster) {
                    toaster.pop('success', 'Saved', successMessage || 'Saved successfully.');
                }

                return response.data;
            }

            function addEntityFailed(error) {
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

        function updateEntity(entityDataStore, id, entity, showToaster, successMessage, failureMessage, ignoreLoadingBar) {
            return $http.put(odataUrl + entityDataStore + '(' + id + ')', entity)
                            .then(updateEntityComplete, updateEntityFailed);

            function updateEntityComplete(response) {
                if (showToaster == undefined ? false : showToaster) {
                    toaster.pop('success', 'Saved', successMessage || 'Saved successfully.');
                }

                return response.data;
            }

            function updateEntityFailed(error) {
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

        function deleteEntity(entityDataStore, id) {
            return $http.delete(apiUrl + entityDataStore + '/' + id, {
                params: {
                    softDelete: false,
                },
            }).then(deleteComplete, deleteFailed);

            function deleteComplete(response) {
                return response.data;
            }

            function deleteFailed(error) {
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will have it called.
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

                // If there is a failure method the below line will have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

        function getVersion() {
            return $http.get(apiUrl + 'about/getVersion')
                        .then(getVersionComplete)
                        .catch(getVersionFailed);

            function getVersionComplete(response) {
                return response.data;
            }

            function getVersionFailed(error) {
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

        function getEntity(entityDataStore, id, includeProperties) {
            if (includeProperties) {
                return $http.get(odataUrl + entityDataStore + (id ? '(' + id + ')' : '') + '?$expand=' + includeProperties)
                            .then(getComplete, getFailed);
            }
            else {
                return $http.get(odataUrl + entityDataStore + (id ? '(' + id + ')' : ''))
                                       .then(getComplete)
                                       .catch(getFailed);
            }
            function getComplete(response) {
                return response.data;
            }

            function getFailed(error) {
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

        function getEntityApi(entityDataStore, id, includeProperties) {
            if (includeProperties) {
                return $http.get(apiUrl + entityDataStore, { params: { id: id, expand: includeProperties }, })
                            .then(getComplete, getFailed);
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
                errorService.handleError(error, showToaster || true, entityDataStore, failureMessage);

                // If there is a failure method the below line will have it called.
                // http://stackoverflow.com/questions/28076258/reject-http-promise-on-success
                return $q.reject(error);
            }
        }

        // OData GetEntities.
        function getEntities(entityDataStore, includeProperties) {
            return $http.get(odataUrl + entityDataStore, { params: includeProperties })
                        .then(getEntitiesComplete, getEntitiesFailed);

            function getEntitiesComplete(response) {
                return response.data.value;
            }

            function getEntitiesFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        function getEntitiesApi(entityDataStore, includeProperties) {
            return $http.get(apiUrl + entityDataStore, { params: includeProperties })
                        .then(getEntitiesComplete, getEntitiesFailed);

            function getEntitiesComplete(response) {
                return response.data.$values;
            }

            function getEntitiesFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        // Replace all $values with ''.
        // OData SearchEntities.
        function searchEntities(entityDataStore, searchCriteria, cache) {
            //return $http.get('api/users');

            return $http.get(odataUrl + entityDataStore, {
                params:
                         {
                             $skip: searchCriteria == undefined || searchCriteria.currentPage == undefined ? null : (searchCriteria.currentPage - 1) * searchCriteria.itemsPerPage,
                             $top: searchCriteria == undefined ? null : searchCriteria.itemsPerPage,
                             $orderby: searchCriteria == undefined ? null : searchCriteria.orderBy,
                             search: searchCriteria == undefined ? null : searchCriteria.searchText,
                             searchFields: searchCriteria == undefined ? null : searchCriteria.searchTextFields,
                             $expand: searchCriteria == undefined ? null : searchCriteria.includeProperties,
                             $filter: searchCriteria == undefined || searchCriteria.q == undefined ? null : searchCriteria.q.replace('=', ' eq '),
                             $select: searchCriteria == undefined ? null : searchCriteria.fields
                         },
                cache: cache == undefined ? false : cache
            })
            .then(searchComplete, searchFailed);

            function searchComplete(response) {
                return response.data.value;
            }

            function searchFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        function searchEntitiesApi(entityDataStore, searchCriteria, cache) {
            return $http.get(apiUrl + entityDataStore, {
                params:
                         {
                             page: searchCriteria == undefined ? null : searchCriteria.currentPage,
                             perPage: searchCriteria == undefined ? null : searchCriteria.itemsPerPage,
                             sort: searchCriteria == undefined ? null : searchCriteria.orderBy,
                             search: searchCriteria == undefined ? null : searchCriteria.searchText,
                             searchFields: searchCriteria == undefined ? null : searchCriteria.searchTextFields,
                             expand: searchCriteria == undefined ? null : searchCriteria.includeProperties,
                             q: searchCriteria == undefined ? null : searchCriteria.q,
                             fields: searchCriteria == undefined ? null : searchCriteria.fields
                         },
                cache: cache == undefined ? false : cache
            })
            .then(searchComplete, searchFailed);

            function searchComplete(response) {
                return response.data.$values;
            }

            function searchFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        function searchEntitiesCount(entityDataStore, searchCriteria, cache) {
            return $http.get(odataUrl + entityDataStore, {
                params:
                        {
                            $count: true,
                            search: searchCriteria == undefined ? null : searchCriteria.searchText,
                            searchFields: searchCriteria == undefined ? null : searchCriteria.searchTextFields,
                            q: searchCriteria == undefined ? null : searchCriteria.q
                        },
                cache: cache == undefined ? false : cache
            })
            .then(searchCountComplete, searchCountFailed);

            function searchCountComplete(response) {
                return response.data['@odata.count'];
            }

            function searchCountFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        function searchEntitiesCountApi(entityDataStore, searchCriteria, cache) {
            return $http.get(apiUrl + entityDataStore, {
                params:
                        {
                            count: true,
                            search: searchCriteria == undefined ? null : searchCriteria.searchText,
                            searchFields: searchCriteria == undefined ? null : searchCriteria.searchTextFields,
                            q: searchCriteria == undefined ? null : searchCriteria.q
                        },
                cache: cache == undefined ? false : cache
            })
            .then(searchCountComplete, searchCountFailed);

            function searchCountComplete(response) {
                return response.data;
            }

            function searchCountFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        //-------------------------------

        function getLoggedInUser() {
            return $http.get('odata/users/usersService.GetLoggedInUser', { cache: true })
                        .then(getLoggedInUserCompleted, getLoggedInUserFailed);

            function getLoggedInUserCompleted(response) {
                return response.data;
            }

            function getLoggedInUserFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

        function getMe() {
            return $http.get('api/Me', { cache: true })
                        .then(getMeCompleted, getMeFailed);

            function getMeCompleted(response) {
                return response.data;
            }

            function getMeFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
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

        function getAuthorization(loginRequired, requiredPermissions, permissionCheckType, uri, parameters) {
            var methodRoute = 'authorize';

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

            function getAuthorizationComplete(response) {
                return response.data;
            }

            function getAuthorizationFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }
        }

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

        //-------------------------------

        function getDashboardMetric(name, loggedInUserId) {
            return $http.get(apiUrl + 'dashboard', { params: { name: name, loggedInUserId: loggedInUserId }, cache: false })
                    .then(getDashboardMetricComplete, getDashboardMetricFailed);

            function getDashboardMetricComplete(response) {
                return response.data;
            }

            function getDashboardMetricFailed(error) {
                $log.error('XHR failed.'
                    + (error.data ? error.data.message + ': ' : '') + (error.data ? error.data.message + ': ' + (error.data.messageDetail || error.data.ExceptionMessage || error.data.Message) : ''));

                error = undefined; return error;
            }
        }

        //-------------------------------

        function getGeoClientInformation(addressLine1, borough) {
            return $http.get(apiUrl + 'restaurantLocations/getGeoClientInformation', { params: { addressLine1: addressLine1, borough: borough }, ignoreLoadingBar: true, cache: true })
                    .then(getGeoClientInformationComplete, getGeoClientInformationFailed);

            function getGeoClientInformationComplete(response) {
                return response.data;
            }

            function getGeoClientInformationFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }

        }

    }
})();
