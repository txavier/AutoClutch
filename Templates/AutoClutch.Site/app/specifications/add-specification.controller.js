(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddSpecificationController', AddSpecificationController);

    AddSpecificationController.$inject = ['$scope', '$log', '$routeParams', '$location', '$timeout', 'documentUploadService', 'dataService', 'authenticationService', 'contractService'];

    function AddSpecificationController($scope, $log, $routeParams, $location, $timeout, documentUploadService, dataService, authenticationService, contractService) {
        var vm = this;

        vm.entityDataStore = 'contracts'
        vm.specification = {};
        vm.contractId = null;
        vm.updateContract = updateContract;
        vm.upload = upload;
        vm.sectionName = $routeParams.sectionName;
        vm.fileInfo = {};
        vm.getFileInfoMessage = getFileInfoMessage;

        activate();

        function activate() {
            getContract($routeParams.contractNumber);
        }

        /**
         * https://github.com/danialfarid/ng-file-upload
         * https://github.com/stewartm83/angular-fileupload-sample
        */
        function upload(files) {
            documentUploadService.upload(files, vm.fileInfo).then(function (data) {
                vm.contract.specificationFilename = data.filename;

                vm.contract.specificationFile = data.file;

                vm.contract.specificationFileType = data.fileType;

                vm.payment.specificationFileId = data.fileId;
            });
        }

        function getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile) {
            var result = documentUploadService.getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile);

            return result;
        }

        $scope.$watch('vm.contract.tempDropFile', function () {
            if (vm.contract.tempDropFile != null && vm.contract.tempDropFile.fileName == null) {
                var files = [vm.contract.tempDropFile];

                upload(files);
            }
        });

        function getContract(contractNumber) {
            // If the contract service has the payments object
            var contractSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 1,
                orderBy: null,
                searchText: null,
                includeProperties: null,
                q: 'contractNumber="' + contractNumber + '"'
            }

            return dataService.searchEntities('contracts', contractSearchCriteria).then(function (data) {
                vm.contract = data.$values[0];

                return vm.contract;
            });
        }

        function updateContract(contract) {
            return dataService.updateEntity(vm.entityDataStore, contract.contractId, contract, true).then(function (data) {
                $location.path('/' + $routeParams.sectionName + '/contracts/' + $routeParams.contractNumber);
            }, function (error) {
                $log.error('An error occurred.');
            });
        }

    }

})();