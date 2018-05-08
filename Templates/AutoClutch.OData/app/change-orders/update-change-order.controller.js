(function () {
    'use strict';

    angular
        .module('app')
        .controller('UpdateChangeOrderController', UpdateChangeOrderController);

    UpdateChangeOrderController.$inject = [
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
        'toaster'
    ];

    function UpdateChangeOrderController(
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
        toaster) {
        var vm = this;

        vm.entityDataStore = 'changeOrders'
        vm.updateEntity = updateEntity;
        vm.history = $window.history;
        vm.disableAll = false;
        vm.contract = {};
        vm.changeOrder = {};
        // The time extension change order type does not show all of the fields.
        // This variable, vm.teimeExtensionInvisible, is used to hide them.
        vm.timeExtensionInvisible = false;
        vm.timeExtensionVisible = false;
        vm.user = {};
        vm.upload = upload;
        vm.fileInfo = {};
        vm.getFileInfoMessage = getFileInfoMessage;
        vm.sectionName = null;
        vm.contractNumber = null;
        vm.deleteChangeOrderFile = deleteChangeOrderFile;
        vm.myForm = {};

        activate();

        function activate() {
            setRouteVariables($routeParams.contractNumber, $routeParams.sectionName);

            getContract($routeParams.contractNumber, false).then(function (data) {
                getSelectedChangeOrder($routeParams.changeOrderId).then(function (data) {
                    setTimeExtensionInvisible(vm.changeOrder);
                    setTimeExtensionVisible(vm.changeOrder);
                });
                setViewDisabledBoxes();
                getLoggedInUserRole();
            });
        }

        function deleteChangeOrderFile(changeOrderId) {
            return dataService.deleteChangeOrderFile(changeOrderId).then(function (data) {
                activate();
            });
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
            return documentUploadService.upload(files, vm.fileInfo).then(function (data) {
                vm.changeOrder.filename = data.filename;

                vm.changeOrder.file = data.file;

                vm.changeOrder.fileType = data.fileType;

                vm.changeOrder.memoPDFFileId = data.fileId;
            });
        }

        function getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile) {
            var result = documentUploadService.getFileInfoMessage(filename, file, fileId, fileInfo, tempDropFile);

            return result;
        }

        $scope.$watch('vm.changeOrder.tempDropFile', function () {
            if (vm.changeOrder.tempDropFile != null
                    && vm.changeOrder.tempDropFile != undefined
                    && vm.changeOrder.tempDropFile.filename == null) {
                var files = [vm.changeOrder.tempDropFile];

                upload(files);

                //upload(files).then(function (data) {
                //    // Autosave after the file has been dropped.
                //    updateEntity('changeOrders', vm.changeOrder.changeOrderId, vm.changeOrder, true);
                //});
            }
        });
        // End file handling methods.

        function getLoggedInUserRole() {
            return authenticationService.getLoggedInUser().then(function (data) {
                vm.user = data;
            });
        }

        // The time extension change order type does not show all of the fields.
        function setTimeExtensionInvisible(changeOrder) {

            // The change order type id 3 is a time extension.
            if (changeOrder.changeOrderTypeId == 3) {
                // This variable, vm.teimeExtensionInvisible, is used to hide them.
                vm.timeExtensionInvisible = true;
            }
        }

        function setTimeExtensionVisible(changeOrder) {
            if (changeOrder.changeOrderTypeId == 3) {
                vm.timeExtensionVisible = true;
            }
        }

        function getSelectedChangeOrder(changeOrderId) {
            if (!changeOrderId) {
                return null;
            }

            var contractSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 1,
                orderBy: null,
                searchText: null,
                includeProperties: 'changeOrderType',
                q: 'changeOrderId=' + changeOrderId
            }

            return dataService.searchEntitiesOData('changeOrders', contractSearchCriteria).then(function (data) {
                vm.changeOrder = data[0];

                vm.changeOrder.contract = vm.contract;

                getChangeOrderTypes();

                return vm.changeOrder;
            });
        }

        function getChangeOrderTypes() {
            return dataService.searchEntitiesOData('changeOrderTypes').then(function (data) {
                vm.changeOrderTypes = data;


                return vm.changeOrderTypes;
            }).then(function () { vm.myForm.$setPristine(); });
        }

        Array.prototype.getIndexBy = function (name, value) {
            for (var i = 0; i < this.length; i++) {
                if (this[i][name] == value) {
                    return i;
                }
            }
        }

        function setViewDisabledBoxes() {
            // The contract status 'Closed' is contract status ID 5.
            // The contract status 'Cancelled' is contract status ID 7.
            // If this is a closed or cancelled contract then we are not allowing the 
            // modification of any of the elements.
            if (vm.contract.contractStatusId == 5 || vm.contract.contractStatusId == 7) {
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
        function getContract(id) {
            var contractSearchCriteria = {
                currentPage: 1,
                itemsPerPage: 1,
                orderBy: null,
                searchText: null,
                includeProperties: 'engineerContracts($expand = engineer),contractor,contractStatus,contractType,contractCategory,workOrders,'
                    + 'workOrders($expand=location),payments($expand=paymentType),changeOrders($expand=changeOrderType),section',
                q: 'contractNumber="' + id + '"'
            }

            return dataService.searchEntitiesOData('contracts', contractSearchCriteria).then(function (data) {
                vm.contract = data[0];

                // Set the contract service contract information for use in the 
                // 'pushOut' sidebar.
                contractService.setContractId(vm.contract.contractId);

                contractService.setContractWorkOrders(vm.contract.workOrders == null ? null : vm.contract.workOrders);

                contractService.setContractNumber(vm.contract.contractNumber);

                contractService.setPayments(vm.contract.payments == null ? null : vm.contract.payments);

                contractService.setChangeOrders(vm.contract.changeOrders == null ? null : vm.contract.changeOrders);

                contractService.setEvaluations(vm.contract.evaluations == null ? null : vm.contract.evaluations);

                contractService.setSection(vm.contract.section);

                return vm.contract;
            });
        }

        function updateEntity(entityDataStore, id, entity, doNotRedirectAfterSave) {
            // ContractTypeId 1 is Open Market Order.  Open market orders do not require files.
            if (entity.contract.contractTypeId != 1) {
                // If this is a change order that has a registered date but 
                // does not include the necessary pdf's then do not allow 
                // the user to save until these are available.
                if (entity.registered && (!entity.memoPDF && !entity.memoPDFFileId && !entity.tempDropFile)) {
                    toaster.pop('warning', 'Information Missing', 'The change order PDF is required.');

                    return;
                }
            }

            var tempContract = entity.contract;

            // For change orders there is no need to save changes to the contract 
            // object.
            entity.contract = null;

            entity.changeOrderType = undefined;

            var tempTempDropFile = entity.tempDropFile;

            entity.tempDropFile = undefined;

            return dataService.deltaUpdateEntityOData(entityDataStore, id, entity, entityDataStore != 'contracts').then(function () {
                entity.engineerContracts = vm.engineerContracts;

                $log.log('Successfully updated.');

                if (!doNotRedirectAfterSave) {
                    if (entityDataStore == 'changeOrders') {
                        $location.path('/' + vm.contract.section.name + '/contracts/' + vm.contract.contractNumber + '/change-orders');
                    }
                    else {
                        $location.path('/' + vm.contract.section.name + '/contracts/' + vm.contract.contractNumber);
                    }
                }

                entity.contract = tempContract || entity.contract;

                entity.tempDropFile = tempTempDropFile;
            });
        }

    }

})();