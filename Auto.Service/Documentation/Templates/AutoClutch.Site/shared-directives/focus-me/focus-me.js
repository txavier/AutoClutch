// http://stackoverflow.com/questions/14833326/how-to-set-focus-on-input-field
angular
    .module('shared.directives')
    .directive('focusMe', focusMe);

function focusMe($timeout) {
    return {
        link: function (scope, element, attrs) {
            scope.$watch(attrs.focusMe, function (value) {
                if (value === true) {
                    console.log('value=', value);
                    //$timeout(function() {
                    element[0].focus();
                    scope[attrs.focusMe] = false;
                    //});
                }
            });
        }
    };
};