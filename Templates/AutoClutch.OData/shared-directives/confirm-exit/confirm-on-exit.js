// http://www.samjhill.com/blog/?p=525https://stackoverflow.com/questions/14852802/detect-unsaved-changes-and-alert-user-using-angularjs

(function () {
    'use strict';

    angular
        .module('shared.directives')
        .directive('confirmOnExit', confirmOnExit);

    function confirmOnExit() {
        return {
            link: function ($scope, elem, attrs) {
                window.onbeforeunload = function () {
                    if ($scope.vm.myForm.$dirty) {
                        return "All changes made will be lost if they are not saved. Are you sure you want to leave this page?";
                    }
                }
                $scope.$on('$locationChangeStart', function (event, next, current) {
                    if ($scope.vm.myForm.$dirty) {
                        if (!confirm("All changes made will be lost if they are not saved. Are you sure you want to leave this page?")) {
                            event.preventDefault();
                        }
                    }
                });
            }
        };
    }
})();
