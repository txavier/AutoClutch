// v1.0
(function () {
    'use strict';

    angular
        .module('app')
        .factory('sharedService', sharedService);

    sharedService.$inject = [];

    function sharedService() {
        var service = {
            setObject: setObject
        };

        return service;

        function setObject(model, propertyName, propertyNameId, entityArray, id) {
            model[propertyName] = entityArray[getIndexBy(entityArray, propertyNameId, id)];

            return model[propertyName];
        }

        function getIndexBy(array, name, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i][name] == value) {
                    return i;
                }
            }
        }

    }
})();