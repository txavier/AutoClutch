(function () {
    'use strict';

    angular
        .module('app')
        .factory('odataService', odataService);

    odataService.$inject = ['$http', '$log', '$q', 'toaster', 'errorService'];

    // Version 1.0.2
    function odataService($http, $log, $q, toaster, errorService) {
        var odataUrl = 'odata/';

        var controllerSuffix = 'OData';

        var service = {
            getContractId: getContractId,
            getInitialContract: getInitialContract,
            //---------------------------------------
            addEntity: addEntity,
            updateEntity: updateEntity,
            deltaUpdateEntity: deltaUpdateEntity,
            deleteEntity: deleteEntity,
            softDeleteEntity: softDeleteEntity,
            getEntity: getEntity,
            getEntities: getEntities,
            searchEntities: searchEntities,
            searchEntitiesCount: searchEntitiesCount,
        };

        return service;

        //---------------------------------------

        function getInitialContract(sectionName) {
            return $http.get(odataUrl + 'contracts' + controllerSuffix + '/contractService.GetInitialContract(sectionName=\'' + sectionName + '\')', { cache: false })
                        .then(getInitialContractComplete, getInitialContractFailed);

            function getInitialContractComplete(response) {
                return response.data;
            }

            function getInitialContractFailed(error) {
                errorService.handleError(error);

                return error;
            }
        }

        function getContractId(contractNumber,showToaster) {
            return $http.get(odataUrl + 'contracts' + controllerSuffix + '/contractService.GetContractId(contractNumber=\'' + contractNumber + '\')')
                        .then(getContractIdCompleted, getContractIdFailed);

            function getContractIdCompleted(response) {
                return response.data.value;
            }

            function getContractIdFailed(error) {
                errorService.handleError(error, showToaster || true, 'contracts');

                return error;
            }
        }

        //---------------------------------------

        function addEntity(entityDataStore, entity, showToaster, successMessage, failureMessage) {
            return $http.post(odataUrl + entityDataStore + controllerSuffix, entity).then(addEntityComplete, addEntityFailed);

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

        function deltaUpdateEntity(entityDataStore, id, entity, showToaster, successMessage, failureMessage, ignoreLoadingBar) {
            return $http.patch(odataUrl + entityDataStore + controllerSuffix + '(' + id + ')', entity)
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

        function updateEntity(entityDataStore, id, entity, showToaster, successMessage, failureMessage, ignoreLoadingBar) {
            return $http.put(odataUrl + entityDataStore + controllerSuffix + '(' + id + ')', entity)
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
            return $http.delete(apiUrl + entityDataStore + controllerSuffix + '/' + id, {
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
            return $http.delete(apiUrl + entityDataStore + controllerSuffix + '/' + id, {
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

        function getEntity(entityDataStore, id, includeProperties) {
            if (includeProperties) {
                return $http.get(odataUrl + entityDataStore + controllerSuffix + (id ? '(' + id + ')' : '') + '?$expand=' + includeProperties)
                            .then(getComplete, getFailed);
            }
            else {
                return $http.get(odataUrl + entityDataStore + controllerSuffix + (id ? '(' + id + ')' : ''))
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
            return $http.get(odataUrl + entityDataStore + controllerSuffix, { params: includeProperties })
                        .then(getEntitiesComplete, getEntitiesFailed);

            function getEntitiesComplete(response) {
                return response.data.value;
            }

            function getEntitiesFailed(error) {
                errorService.handleError(error);

                return $q.reject(error);
            }

        }

        // Replace all $values with ''.
        // OData SearchEntities.
        function searchEntities(entityDataStore, searchCriteria, cache) {
            if (!searchCriteria) {
                searchCriteria = {};
            }

            return $http.get(odataUrl + entityDataStore + controllerSuffix, {
                params:
                         {
                             $skip: searchCriteria.$skip || (searchCriteria == undefined || searchCriteria.currentPage == undefined ? null : (searchCriteria.currentPage - 1) * searchCriteria.itemsPerPage),
                             $top: searchCriteria.$top || (searchCriteria == undefined ? null : searchCriteria.itemsPerPage),
                             $orderby: searchCriteria.$orderby || (searchCriteria == undefined ? null : searchCriteria.orderBy),
                             search: searchCriteria == undefined ? null : searchCriteria.searchText,
                             searchFields: searchCriteria == undefined ? null : searchCriteria.searchTextFields,
                             $expand: searchCriteria.$expand || (searchCriteria == undefined ? null : searchCriteria.includeProperties),
                             $filter: searchCriteria.$filter || (searchCriteria == undefined || searchCriteria.q == undefined || searchCriteria.q === '' ? null : searchCriteria.q.replace(/\./g, '/').replace(/==/g, ' eq ').replace(/!=/g, ' ne ').replace(/=/g, ' eq ').replace(/"/g, "'")),
                             $select: searchCriteria.$select || (searchCriteria == undefined ? null : searchCriteria.fields)
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

        function searchEntitiesCount(entityDataStore, searchCriteria, cache) {
            return $http.get(odataUrl + entityDataStore + controllerSuffix, {
                params:
                        {
                            $count: true,
                            $top: 0,
                            search: searchCriteria == undefined ? null : searchCriteria.searchText,
                            searchFields: searchCriteria == undefined ? null : searchCriteria.searchTextFields,
                            $filter: searchCriteria.$filter || (searchCriteria == undefined || searchCriteria.q == undefined || searchCriteria.q === '' ? null : searchCriteria.q.replace(/\./g, '/').replace(/==/g, ' eq ').replace(/!=/g, ' ne ').replace(/=/g, ' eq ').replace(/"/g, "'"))
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
    }
})();