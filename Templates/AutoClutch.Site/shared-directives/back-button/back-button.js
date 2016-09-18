// http://stackoverflow.com/questions/14070285/how-to-implement-history-back-in-angular-js
// http://jsfiddle.net/asgoth/WaRKv/
angular
    .module('shared.directives')
    .directive('backButton', backButton);

function backButton() {
    var directive = {
        restrict: 'E',
        template: "<input type='submit' value='Back' class='{{vm.innerClass}}' />",   //class='btn btn-primary pull-right'
        scope: {
            back: '@back',
            innerClass: '@'
        },
        link: link,
        controller: backButtonController,
        controllerAs: 'vm',
        bindToController: true // because the scope is isolated
    };

    return directive;

    function link(scope, element, attrs, vm) {
        $(element[0]).on('click', function () {
            vm.history.back();
        });
    }
}

backButtonController.$inject = ['$scope', '$window'];

function backButtonController($scope, $window) {
    // Injecting $scope just for comparison
    var vm = this;

    vm.history = $window.history;

    activate();

    function activate() {
    }
}