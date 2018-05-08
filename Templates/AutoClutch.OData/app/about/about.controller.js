(function () {
    angular
        .module('app')
        .controller('aboutController', aboutController);

    aboutController.$inject = ['$scope', '$routeParams', 'dataService'];

    function aboutController($scope, $routeParams, dataService) {
        var vm = this;
        var strArray = new Array();

        vm.version = null;
        vm.versionName = null;
        vm.versionCulture = null;
        vm.versionToken = null;

        activate();


        function activate() {
            getVersion().then(function () {
                replaceVersion();
            });  
        }

        function getVersion() {
            return dataService.getVersion().then(function (data) {
                vm.version = data;
            });
        } 

        function replaceVersion() {
            vm.version = vm.version.replace(/[,]+/g, "");
            vm.version = vm.version.split(" ");
            var temp = vm.version; 
            vm.versionName = temp[1];
            vm.versionCulture = temp[2];
            vm.versionToken = temp[3];
        }
    }
})();
