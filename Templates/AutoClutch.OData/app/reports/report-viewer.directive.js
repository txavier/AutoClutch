(function () {
    'use strict';

    angular
        .module('app')
        .controller('ReportViewerController', ReportViewerController);

    ReportViewerController.$inject = ['$scope'];

    function ReportViewerController($scope) {
        var vm = this;

        activate();

        function activate() {
            return vm;
        }
    }

})();