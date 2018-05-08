(function () {
    'use strict';

    angular
        .module('app')
        .controller('AddPaymentController', AddPaymentController);

    AddPaymentController.$inject = [
        '$scope',
        '$log',
        '$window',
        '$location',
        '$routeParams',
        '$timeout',
        'dataService',
        'authenticationService',
        'contractService',
        'documentUploadService',
        'toaster',
        'sharedService'
    ];

    function AddPaymentController(
                $scope,
                $log,
                $window,
                $location,
                $routeParams,
                $timeout,
                dataService,
                authenticationService,
                contractService,
                documentUploadService,
                toaster,
                sharedService) {
        var vm = this;

        vm.entityDataStore = 'payments'
        vm.payment = {};
        vm.paymentTypes = [];
        vm.getPaymentTypeProjectRetainage = getPaymentTypeProjectRetainage;
        vm.addPayment = addPayment;
        vm.history = $window.history;
        vm.disableAll = false;
        vm.upload = upload;
        vm.fileInfo = {};
        vm.getFileInfoMessage = getFileInfoMessage;
        vm.sectionName = null;
        vm.contractNumber = null;
        vm.date = new Date();
        vm.setObject = setObject;
        vm.myForm = {};

        activate();

        function activate() {
            setRouteVariables($routeParams.contractNumber, $routeParams.sectionName);

            getContract($routeParams.contractNumber).then(function (data) {
                initializeNewPayment($routeParams.contractNumber);
            }).then(setViewDisabledBoxes);

            // The update payment view needs payment types for the payment type dropdown
            // based on the type of contract this is.
            getScopedPaymentTypes($routeParams.contractNumber);
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
                vm.payment.filename = data.filename;

                vm.payment.file = data.file;

                vm.payment.fileType = data.fileType;

                vm.payment.paymentPDFFileId = data.fileId;
            });
        }

        function getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile) {
            var result = documentUploadService.getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile);

            return result;
        }

        $scope.$watch('vm.payment.tempDropFile', function () {
            if (vm.payment.tempDropFile != null && vm.payment.tempDropFile != undefined && vm.payment.tempDropFile.filename == null) {
                var files = [vm.payment.tempDropFile];

                upload(files);
            }
        });

        function setViewDisabledBoxes() {
            var contract = vm.payment.contract;

            // The contract status 'Closed' is contract status ID 5.
            // The contract status 'Cancelled' is contract status ID 7.
            // If this is a closed or cancelled contract then we are not allowing the 
            // modification of any of the elements.
            if (contract.contractStatusId == 5 || contract.contractStatusId == 7) {
                vm.disableAll = true;
            }
            vm.myForm.$setPristine();

            return vm.disableAll;
        }

        function addPayment(payment) {
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

            var tempContract = payment.contract;

            payment.contract = null;

            var tempPaymentType = payment.paymentType;

            payment.paymentTypeId = payment.paymentType.paymentTypeId;

            payment.paymentType = null;

            payment.workOrders = undefined;

            payment.tempDropFile = undefined;

            return dataService.addEntityOData(vm.entityDataStore, payment, true).then(function (data) {
                $location.path('/' + $routeParams.sectionName + '/contracts/' + $routeParams.contractNumber + '/payments');
            }, function (error) {
                $log.error('An error occurred.');

                // On an error, get the temporary values back in order to set up 
                // the view so that the user can make corrections as needed to 
                // correct the values for a subsequent submittal of data.
                payment.contract = tempContract;

                payment.paymentType = tempPaymentType;
            });
        }

        function setObject(model, propertyName, propertyNameId, entityArray, id) {
            var result = sharedService.setObject(model, propertyName, propertyNameId, entityArray, id);

            return result;
        }

        function getPaymentTypeProjectRetainage(paymentTypeId, projectRetainage) {
            var paymentTypeName = vm.payment.paymentType.name;

            return dataService.getPaymentTypeProjectRetainage(paymentTypeName, projectRetainage).then(function (data) {
                vm.payment.projectRetainage = data;

                return vm.payment.projectRetainage;
            });
        }

        function getScopedPaymentTypes(contractNumber) {
            return dataService.getScopedPaymentTypes(contractNumber).then(function (data) {
                vm.paymentTypes = data;

                return vm.paymentTypes;
            });
        }

        function getContract(id) {
            var contractSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 1,
                orderBy: null,
                searchText: null,
                includeProperties: 'engineerContracts($expand = engineer), contractor, contractStatus, contractType, contractCategory, workOrders,'
                    + 'workOrders($expand = location), payments($expand = paymentType), changeOrders($expand = changeOrderType), section',
                q: 'contractNumber="' + id + '"'
            }

            return dataService.searchEntitiesOData('contracts', contractSearchCriteria).then(function (data) {
                vm.payment.contract = data[0];

                // Set the contract service contract information for use in the 
                // 'pushOut' sidebar.
                contractService.setContractId(vm.payment.contract.contractId);

                contractService.setContractWorkOrders(vm.payment.contract.workOrders == null ? null : vm.payment.contract.workOrders);

                contractService.setContractNumber(vm.payment.contract.contractNumber);

                contractService.setPayments(vm.payment.contract.payments == null ? null : vm.payment.contract.payments);

                contractService.setChangeOrders(vm.payment.contract.changeOrders == null ? null : vm.payment.contract.changeOrders);

                contractService.setEvaluations(vm.payment.contract.evaluations == null ? null : vm.payment.contract.evaluations);

                contractService.setSection(vm.payment.contract.section);

                return vm.payment.contract;
            });
        }

        function initializeNewPayment(contractId) {
            var tempContract = vm.payment.contract;

            return dataService.getInitialPayment(vm.payment.contract.contractId).then(function (data) {
                vm.payment = data;

                vm.payment.contract = tempContract;

                vm.payment.contractId = vm.payment.contract.contractId;

                vm.payment.paymentIn = vm.date;

                return vm.payment.contractId;
            });
        }

    }

})();