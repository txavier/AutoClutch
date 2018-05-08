(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddDeductionController', AddDeductionController);

    AddDeductionController.$inject = ['$scope', '$log', '$routeParams', '$location', '$timeout', '$window', 'documentUploadService', 'dataService', 'authenticationService',
        'contractService', 'toaster', 'sharedService'];

    function AddDeductionController($scope, $log, $routeParams, $location, $timeout, $window, documentUploadService, dataService, authenticationService, contractService,
        toaster, sharedService) {
        var vm = this;

        vm.entityDataStore = 'deductions'
        vm.deduction = {};
        vm.contractId = null;
        vm.deductionTypes = [];
        vm.payments = [];
        vm.addDeduction = addDeduction;
        vm.upload = upload;
        vm.fileInfo = {};
        vm.getFileInfoMessage = getFileInfoMessage;
        vm.sectionName = null;
        vm.contractNumber = null;
        vm.paymentChanged = paymentChanged;
        vm.showFuturePaymentNumber = false;
        vm.setObject = setObject;
        vm.myForm = {};

        activate();

        function activate() {
            setRouteVariables($routeParams.contractNumber, $routeParams.sectionName);

            getContract($routeParams.contractNumber).then(getPayments);

            getDeductionTypes();
        }

        function setObject(model, propertyName, propertyNameId, entityArray, id) {
            var result = sharedService.setObject(model, propertyName, propertyNameId, entityArray, id);

            return result;
        }

        function paymentChanged() {
            if (vm.deduction.payment.paymentId === 0) {
                vm.showFuturePaymentNumber = true;

                vm.deduction.payment.futurePayment = true;

                vm.deduction.payment.valueForPaymentNumber = vm.payments[vm.payments.length - 2].paymentNumber + 1;
            }
            else {
                vm.showFuturePaymentNumber = false;

                vm.deduction.payment.futurePayment = undefined;
            }
        }

        function setRouteVariables(contractNumber, sectionName) {
            vm.sectionName = sectionName;

            vm.contractNumber = contractNumber;
        }

        /**
         * https://github.com/danialfarid/ng-file-upload
         * https://github.com/stewartm83/angular-fileupload-sample
        */
        function upload(files) {
            documentUploadService.upload(files, vm.fileInfo).then(function (data) {
                vm.deduction.filename = data.filename;

                vm.deduction.file = data.file;

                vm.deduction.fileType = data.fileType;

                vm.deduction.fileId = data.fileId;
            });
        }

        function getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile) {
            var result = documentUploadService.getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile);

            return result;
        }

        $scope.$watch('vm.deduction.tempDropFile', function () {
            if (vm.deduction.tempDropFile != null && vm.deduction.tempDropFile != undefined && vm.deduction.tempDropFile.filename == null) {
                var files = [vm.deduction.tempDropFile];

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
                includeProperties: 'payments',
                q: 'contractNumber="' + contractNumber + '"'
            };

            return dataService.searchEntitiesOData('contracts', contractSearchCriteria).then(function (data) {
                vm.contract = data[0];

                return vm.contract;
            });
        }

        function getPayments() {
            var searchCriteria = {
                orderBy: null,
                q: 'contractId=' + vm.contract.contractId
            };

            return dataService.searchEntitiesOData('payments', searchCriteria).then(function (data) {
                vm.payments = data;

                vm.payments.push({ paymentNumber: 'Apply to Future Payment', paymentId: 0, contractId: vm.contract.contractId });

                vm.myForm.$setPristine();

                return vm.payments;
            });
        }

        function getDeductionTypes() {
            return dataService.getEntitiesOData('deductionTypes').then(function (data) {
                vm.deductionTypes = data;

                return vm.deductionTypes;
            });
        }

        function addDeduction(deduction) {
            if (!vm.myForm.$valid) {
                var errors = [];

                for (var key in vm.myForm.$error) {
                    for (var index = 0; index < vm.myForm.$error[key].length; index++) {
                        errors.push(vm.myForm.$error[key][index].$name + ' is required.');
                    }
                }

                toaster.pop('warning', 'Information Missing', 'The ' + errors[0]);

                return;
            }

            // If this is going to include a new future payment...
            if (vm.deduction.payment.valueForPaymentNumber) {
                vm.deduction.payment.paymentNumber = vm.deduction.payment.valueForPaymentNumber;

                return dataService.addEntityOData('payments', vm.deduction.payment).then(function (data) {
                    deduction.paymentId = data;

                    addEntity();
                });
            }
            else {
                addEntity();
            }

            function addEntity() {
                if (deduction.payment) {
                    var tempPayment = deduction.payment;

                    deduction.payment = null;
                }

                // Remove the tempDropFile property so that OData does not complain
                // about it being in the .net model.
                deduction.tempDropFile = undefined;

                return dataService.addEntityOData(vm.entityDataStore, deduction, true).then(function (data) {
                    $location.path('/' + $routeParams.sectionName + '/contracts/' + $routeParams.contractNumber + '/payments');
                }, function (error) {
                    $log.error('An error occurred.');

                    // On an error, get the temporary values back in order to set up 
                    // the view so that the user can make corrections as needed to 
                    // correct the values for a subsequent submittal of data.
                    deduction.payment = tempPayment;
                });
            }
        }
    }

})();