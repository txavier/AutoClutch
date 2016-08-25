// http://willseitz-code.blogspot.com/2013/01/javascript-to-deserialize-json-that.html
(function () {
    'use strict';

    angular
        .module('app')
        .factory('jsonPointerParseService', jsonPointerParseService);

    jsonPointerParseService.$inject = [];

    function jsonPointerParseService() {
        var hashOfObjects = {};

        var service = {
            pointerParse: pointerParse
        };

        return service;

        function collectIds(obj) {
            if (jQuery.type(obj) === "object") {
                if (obj.hasOwnProperty("$id")) {
                    hashOfObjects[obj.$id] = obj;
                }
                for (var prop in obj) {
                    collectIds(obj[prop]);
                }
            } else if (jQuery.type(obj) === "array") {
                obj.forEach(function (element) {
                    collectIds(element);
                });
            }
        }

        function setReferences(obj) {
            if (jQuery.type(obj) === "object") {
                for (var prop in obj) {
                    if (jQuery.type(obj[prop]) === "object" &&
                        obj[prop].hasOwnProperty("$ref")) {
                        obj[prop] = hashOfObjects[obj[prop]["$ref"]];
                    } else {
                        setReferences(obj[prop]);
                    }
                }
            } else if (jQuery.type(obj) === "array") {
                obj.forEach(function (element, index, array) {
                    if (jQuery.type(element) === "object" &&
                        element.hasOwnProperty("$ref")) {
                        array[index] = hashOfObjects[element["$ref"]];
                    } else {
                        setReferences(element);
                    }
                });
            }
        }

        // Set the max depth of your object graph because JSON.stringify will not be able to
        // serialize a large object graph back to 
        function setMaxDepth(obj, depth) {
            // If this is not an object or array just return there is no need to 
            // set its max depth.
            if (jQuery.type(obj) !== "array" && jQuery.type(obj) !== "object") {
                return obj;
            }

            var newObj = {};

            // If this object was an array we want to return the same type in order
            // to keep this variable's consistency.
            if (jQuery.type(obj) === "array") {
                newObj = [];
            }

            // For each object or array cut off its tree at the depth value by 
            // recursively diving into the tree.
            angular.forEach(obj, function (value, key) {
                if (depth == 1) {
                    newObj = null;
                }
                else if (jQuery.type(value) === "array") {
                    if (setMaxDepth(value, depth - 1)) {
                        newObj[key] = setMaxDepth(value, depth - 1)
                    } else {
                        newObj = [];
                    }
                } else if (jQuery.type(value) === "object") {
                    if (setMaxDepth(value, depth - 1)) {
                        newObj[key] = setMaxDepth(value, depth - 1)
                    } else {
                        newObj = {};
                    }
                } else {
                    newObj[key] = value;
                }
            }, newObj);

            //angular.forEach(newObj, function (value, key) {
            //    if (newObj[key] == '__null') {
            //        newObj[key] = null;
            //    }
            //}, newObj);

            return newObj;
        }

        function pointerParse(obj, depth) {
            var newObj = obj;

            hashOfObjects = {};
            collectIds(newObj);
            setReferences(newObj);

            if (depth) {
                newObj = setMaxDepth(newObj, depth);
            }

            return newObj;
        }

    }
})();