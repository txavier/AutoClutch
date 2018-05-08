(function () {
    'use strict';

    angular
        .module('app')
        .controller('UpdatePaymentController', UpdatePaymentController);

    UpdatePaymentController.$inject = [
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

    function UpdatePaymentController(
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
        vm.updateEntity = updateEntity;
        vm.history = $window.history;
        vm.disableAll = false;
        vm.upload = upload;
        vm.fileInfo = {};
        vm.deletePaymentFile = deletePaymentFile;
        vm.getFileInfoMessage = getFileInfoMessage;
        vm.contract = {};
        vm.openMarketOrderInvisible = false;
        vm.sectionName = null;
        vm.contractNumber = null;
        vm.user = {};
        vm.paymentTypeChange = paymentTypeChange;
        vm.projectRetainageDisplay = null;
        vm.runBusinessRules = runBusinessRules;
        vm.contractDetailsChanged = contractDetailsChanged;
        vm.setObject = setObject;
        vm.myForm = {};

        activate();

        function activate() {
            getContract($routeParams.contractNumber, false).then(function (data) {
                setRouteVariables($routeParams.contractNumber, $routeParams.sectionName);

                getSelectedPayment($routeParams.paymentId);

                // The update payment view needs payment types for the payment type dropdown.
                getScopedPaymentTypes($routeParams.contractNumber).then(getSelectedPaymentType);

                setViewVisibleElements(vm.contract);

                getLoggedInUserRole();
            }).then(function () {
                vm.myForm.$setPristine();
            }).then(setViewDisabledBoxes);
        }


        function setObject(model, propertyName, propertyNameId, entityArray, id) {
            var result = sharedService.setObject(model, propertyName, propertyNameId, entityArray, id);

            return result;
        }

        function getProjectRetainageDisplay() {
            if (!vm.payment || !vm.payment.contract) {
                return;
            }

            vm.projectRetainageDisplay = (vm.payment.projectRetainage == undefined || vm.payment.projectRetainage == null || vm.payment.paymentType.name == "Standard") ?
                ((vm.payment.contract.projectRetainage || vm.contract.projectRetainage) * 100) : (vm.payment.projectRetainage * 100) + '%';

            return vm.projectRetainageDisplay;
        }

        function paymentTypeChange() {
            getPaymentTypeProjectRetainage(vm.payment.paymentType.name, vm.contract.projectRetainage).then(getProjectRetainageDisplay);
        }

        function getLoggedInUserRole() {
            return authenticationService.getLoggedInUser().then(function (data) {
                vm.user = data;
            });
        }

        function setRouteVariables(contractNumber, sectionName) {
            vm.sectionName = sectionName;

            vm.contractNumber = contractNumber;
        }

        function getSelectedPaymentType() {
            if (!vm.payment) {
                return;
            }

            vm.payment.paymentType = vm.paymentTypes[vm.paymentTypes.getIndexBy('paymentTypeId', vm.payment.paymentTypeId)];

            return vm.payment.paymentType;
        }

        function getSelectedPayment(paymentId) {
            if (!paymentId) {
                return null;
            }

            vm.contract = vm.payment.contract;

            vm.payment = vm.payment.contract.payments[vm.payment.contract.payments.getIndexBy('paymentId', paymentId)];

            vm.payment.contract = vm.contract;

            if (!vm.payment) {
                return null;
            }

            getProjectRetainageDisplay();

            vm.payment.contract = {};

            vm.payment.contract.contractNumber = vm.contract.contractNumber;

            // This is needed for the update receiving payment directive which 
            // is used for blanket order contract types (i.e. contractTypeId === 4).
            vm.payment.contract.contractTypeId = vm.contract.contractTypeId;

            vm.payment.contract.contractor = vm.contract.contractor;
        }

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
        }

        function deletePaymentFile(paymentId) {
            return dataService.deletePaymentFile(paymentId).then(function (data) {
                activate();
            });
        }

        /**
         * https://github.com/danialfarid/ng-file-upload
         * https://github.com/stewartm83/angular-fileupload-sample
        */
        function upload(files) {
            return documentUploadService.upload(files, vm.fileInfo).then(function (data) {
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

                upload(files).then(function (data) {
                    // Autosave after the file has been dropped.
                    updateEntity('payments', vm.payment.paymentId, vm.payment, true);
                });
            }
        });

        function setViewDisabledBoxes() {
            if (!vm.payment || !vm.payment.contract) {
                return;
            }

            var contract = vm.payment.contract;

            // The contract status 'Closed' is contract status ID 5.
            // The contract status 'Cancelled' is contract status ID 7.
            // If this is a closed or cancelled contract then we are not allowing the 
            // modification of any of the elements.
            if (contract.contractStatusId == 5 || contract.contractStatusId == 7) {
                vm.disableAll = true;
            }

            return vm.disableAll;
        }

        // Based on this contract make html elements visible or invisible.
        function setViewVisibleElements(contract) {

            // ContractTypeId 1 is Open Market Order.
            if (contract.contractTypeId == 1) {
                vm.openMarketOrderInvisible = true;
            }
        }

        function contractDetailsChanged(payment) {
            activate();

            paymentTypeChange();

            runBusinessRules(payment);
        }

        function runBusinessRules(payment) {
            if (payment && payment.paymentId) {
                // If the paymentAmount value is a string and there is a dollar sign take away the dollar sign.
                if ((typeof vm.payment.paymentAmount) === "string" && vm.payment.paymentAmount.indexOf('$') > -1) {
                    vm.payment.paymentAmount = vm.payment.paymentAmount.replace("$", "").replace(",", "");
                }

                return dataService.runBusinessRules(vm.payment).then(function (data) {
                    vm.payment.lineADisplay = data.lineADisplay;

                    vm.payment.lineBDisplay = data.lineBDisplay;

                    vm.payment.lineCDisplay = data.lineCDisplay;

                    vm.payment.lineEDisplay = data.lineEDisplay;

                    vm.payment.lineFDisplay = data.lineFDisplay;

                    vm.payment.lineGDisplay = data.lineGDisplay;

                    vm.payment.lineHDisplay = data.lineHDisplay;

                    vm.payment.lineIDisplay = data.lineIDisplay;

                    return vm.payment;
                });
            }
        }

        function getPaymentTypeProjectRetainage(paymentTypeName, projectRetainage) {
            return dataService.getPaymentTypeProjectRetainage(paymentTypeName, projectRetainage).then(function (data) {
                vm.payment.projectRetainage = data;

                runBusinessRules(vm.payment);

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
                includeProperties: 'engineerContracts($expand=engineer), contractor, contractStatus, contractType, contractCategory, workOrders,' +
                    'workOrders($expand=location), payments($expand=paymentType), payments, section',
                q: 'contractNumber="' + id + '"'
            }

            return dataService.searchEntitiesOData('contracts', contractSearchCriteria).then(function (data) {
                vm.payment.contract = data[0];

                return vm.payment.contract;
            });
        }

        function updateEntity(entityDataStore, id, entity, doNotRedirectAfterSave) {
            var tempDeductions = entity.deductions;

            entity.deductions = undefined;

            var tempWorkOrders = entity.workOrders;

            entity.workOrders = undefined;

            var tempContract = entity.contract;

            entity.contract = null;

            entity.tempDropFile = undefined;

            var tempPaymentType = entity.paymentType;

            entity.paymentType = null;

            return dataService.updateEntityOData(entityDataStore, id, entity, entityDataStore != 'contracts').then(function () {
                entity.engineerContracts = vm.engineerContracts;

                $log.log('Successfully updated.');

                if (!doNotRedirectAfterSave) {
                    if (entityDataStore == 'payments') {
                        $location.path('/' + vm.contract.section.name + '/contracts/' + vm.contract.contractNumber + '/payments');
                    }
                    else {
                        $location.path('/' + vm.contract.section.name + '/contracts/' + vm.contract.contractNumber);
                    }
                }

                entity.deductions = tempDeductions;

                entity.workOrders = tempWorkOrders;

                entity.contract = tempContract || entity.contract;

                entity.paymentType = tempPaymentType || entity.paymentType;
            });
        }

    }

})();